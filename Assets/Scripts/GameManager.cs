using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject GUICamera;
	public GameObject ballCamera;
	public GameObject overviewCamera;
	public GameObject spectatorCamera;
	public GameObject motherNode;
	public GameObject ball;
	public GameObject ballOverviewSprite;
	public GameObject overviewGUICamera;


	public bool BallView = false;
	private bool inPlacementArea = false;
	private GameObject currentPlacementBox = null;
	private bool areaOverlayVisible = true;
	private bool cancelZoom = false;
	
	private bool isSwapped = false;
	private bool isPlaying = false;

	private bool skipIt = false;

	private int points = 20;
	private int maxRounds = 4;
	private int currentRound = 1;
	private float pointGainTimer = 0.0f;
	private float pointGainInterval = 5.0f;
	private int pointGainAmount = 1;

	private string debugText = "";

	// Use this for initialization
	void Start () {

		Screen.sleepTimeout = SleepTimeout.NeverSleep;


		SetPlayerMode ();

		MeshRenderer[] meshRenderers;
		meshRenderers = motherNode.GetComponentsInChildren<MeshRenderer>();
		CapsuleCollider[] colliders;
		colliders = motherNode.GetComponentsInChildren<CapsuleCollider> ();
		foreach (MeshRenderer mr in meshRenderers)
		{
			mr.enabled = false;
		}
		foreach (CapsuleCollider cl in colliders) 
		{
			cl.enabled = false;
		}
	}


	void Update () {

		if(ball == null)
			ball = GameObject.FindGameObjectWithTag("TheBall");

		//Give the non-ball player extra points over time
		if (!IsBall () && isPlaying) {
			pointGainTimer += Time.deltaTime;
			if(pointGainTimer >= pointGainInterval){
				pointGainTimer -= pointGainInterval;
				AddPoints(pointGainAmount);
			}
		}

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			Click(Input.GetTouch(0).position);
		} else if (Input.GetMouseButtonDown (0)) {
			Click(Input.mousePosition);
		}

	}

	void Click(Vector2 position)
	{
		if (!IsBall ()) {
				if (!inPlacementArea && areaOverlayVisible) {
						Ray ray = overviewGUICamera.camera.ScreenPointToRay (position);
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit, 100, 1 << LayerMask.NameToLayer ("OverviewGUIAreas"))) {
								if (hit.collider.gameObject.name == "OverviewArea") {
										overviewCamera.GetComponent<OverviewCameraScript> ().LerpTo (hit.collider.gameObject.GetComponent<OverviewAreaRenderer> ().alignmentBox.position + hit.collider.gameObject.GetComponent<OverviewAreaRenderer> ().alignmentBox.up * 30, Quaternion.LookRotation (-hit.collider.gameObject.GetComponent<OverviewAreaRenderer> ().alignmentBox.up, hit.collider.gameObject.GetComponent<OverviewAreaRenderer> ().alignmentBox.forward), null);
										inPlacementArea = true;
										currentPlacementBox = hit.collider.gameObject.GetComponent<OverviewAreaRenderer> ().alignmentBox.gameObject;
										//Show back arrow
										overviewGUICamera.GetComponentInChildren<SpriteRenderer> ().enabled = true;
										overviewGUICamera.camera.cullingMask = 1 << LayerMask.NameToLayer ("OverviewGUI");
										areaOverlayVisible = false;
										skipIt = true;
										ballOverviewSprite.GetComponent<SpriteRenderer> ().enabled = false;
								}
						}


				} else {	
					Ray rayB = overviewGUICamera.camera.ScreenPointToRay (position);
					RaycastHit hitB;
					if (Physics.Raycast (rayB, out hitB, 1000, 1 << 11)) {
							if (hitB.collider.gameObject.name == "BackArrow") {
									overviewCamera.GetComponent<OverviewCameraScript> ().GoBackToStart (ShowOverviewAreas);	
									inPlacementArea = false;
									currentPlacementBox = null;
									//Hide back arrow
									overviewGUICamera.GetComponentInChildren<SpriteRenderer> ().enabled = false;
									//overviewGUICamera.camera.cullingMask = (1 << LayerMask.NameToLayer("OverviewGUI")) | (1 << LayerMask.NameToLayer("OverviewGUIAreas"));
									ballOverviewSprite.GetComponent<SpriteRenderer> ().enabled = true;
							}
					}
				}
		}

	}

	void SetPlayerMode ()
	{
		if (Network.isClient || Network.isServer) {
//			if (GameObject.Find ("GlobalStorage").GetComponent<NetworkManager> ().GetId () == 1) {
			if (PlayerPrefs.GetInt("PlayerType") == 1) {
				ballCamera.SetActive (false);
				GUICamera.SetActive (false);
				spectatorCamera.SetActive(false);
				overviewCamera.SetActive (true);
				overviewGUICamera.SetActive(true);
				isSwapped = true;
				isPlaying = true;
			}
//			else if (GameObject.Find ("GlobalStorage").GetComponent<NetworkManager> ().GetId () == 0) {
			else if (PlayerPrefs.GetInt("PlayerType") == 0) {
				ballCamera.SetActive (true);
				GUICamera.SetActive (true);
				overviewCamera.SetActive (false);
				overviewGUICamera.SetActive(false);
				spectatorCamera.SetActive(false);
				ball.GetComponent<KulanNetworkScript>().SetAsOwner();
				isPlaying = true;
			}
			else { //Spectator
				ballCamera.SetActive(false);
				overviewCamera.SetActive(false);
				GUICamera.SetActive(false);
				overviewGUICamera.SetActive(false);
				spectatorCamera.SetActive(true);
				isPlaying = false;

			}
		}
		else {
			if(BallView)
			{
				ballCamera.SetActive (true);
				GUICamera.SetActive (true);
				overviewCamera.SetActive (false);
				spectatorCamera.SetActive(false);
				overviewGUICamera.SetActive(false);
				isPlaying = true;
				isSwapped = false;
			} else {

				ballCamera.SetActive (false);
				GUICamera.SetActive (false);
				spectatorCamera.SetActive(false);
				overviewCamera.SetActive (true);
				overviewGUICamera.SetActive(true);
				isPlaying = true;
				isSwapped = true;
			}
			//This is normally called by the network manager, but needs to be done manually if we are playing single-player
			GetComponent<CountdownScript> ().StartCountDown ();
		}
		
	}

	[RPC]
	public void SwapPlayers()
	{
		debugText = "I am useless";
		if (isPlaying) {
			if (isSwapped) {
				debugText = "I am the new ball";
				ballCamera.SetActive (true);
				GUICamera.SetActive (true);
				overviewCamera.SetActive (false);
				overviewGUICamera.SetActive(false);
				ball.GetComponent<KulanNetworkScript> ().SetAsOwner ();
				ball.GetComponent<BallUtilityScript> ().ResetPosition ();
				isSwapped = false;
			} else {
				debugText = "I am the new trackmaster";
				ballCamera.SetActive (false);
				GUICamera.SetActive (false);
				overviewCamera.SetActive (true);
				overviewGUICamera.SetActive(true);
				isSwapped = true;
				ball.GetComponent<BallUtilityScript> ().ResetPosition ();
			}
		}

	}

	public void StartNewRound(){

		if (isPlaying && IsBall ()) {
			networkView.RPC ("SwapPlayers", RPCMode.All, null);
			networkView.RPC ("IncrementRound", RPCMode.All, null);
			if(currentRound > maxRounds){

				networkView.RPC ("EndGame", RPCMode.All, null);

			}
		}
		spectatorCamera.GetComponent<SpectatorCamScript> ().ResetToStart ();

		
		ballCamera.GetComponent<CameraPositioningScript> ().ResetToStart ();
		//Reactivate collectibles
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Collectable")) {
			go.GetComponent<CollectableScript> ().ResetCollectable ();
		}
	}

	[RPC]
	void IncrementRound(){
		currentRound++;
		debugText = "incremented";
	}

	[RPC]
	void EndGame(){

		//Change this!!!!!!!
		Application.LoadLevel (0);
	
	}

	public bool IsBall(){
		return !isSwapped;
	}

	void ShowOverviewAreas(){
		overviewGUICamera.camera.cullingMask = (1 << LayerMask.NameToLayer("OverviewGUI")) | (1 << LayerMask.NameToLayer("OverviewGUIAreas"));
		areaOverlayVisible = true;


	}

	public void AddPoints(int points){
		this.points += points;
	}

	public void RemovePoints(int points){
		this.points -= points;
	}

	public int GetPoints(){
		return points;
	}

	void OnGUI()
	{

		GUI.Label (new Rect (0, 0, 100, 50), points.ToString ());
		GUI.Label (new Rect (0, 50, 1000, 50), debugText);
		
	}

	public GameObject GetPlacementBox(){
		if (skipIt) {
			skipIt = false;	
			return null;
		}
		return currentPlacementBox;

	}




}
