using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Timing;

public class GameManager : MonoBehaviour {

	public GameObject GUICamera;
	public GameObject ballCamera;
	public GameObject overviewCamera;
	public GameObject spectatorCamera;
	public GameObject motherNode;
	public GameObject ball;
	public GameObject ballOverviewSprite;
	public GameObject overviewGUICamera;
	public GUIStyle style;
	public GUIStyle endGameTextStyle;
	public GUIStyle lapStyle;
	public GUIStyle resourceStyle;
	public Texture crystal;

	public bool BallView = false;
	private bool inPlacementArea = false;
	private GameObject currentPlacementBox = null;
	private bool areaOverlayVisible = true;
	
	private bool isSwapped = false;
	private bool isPlaying = false;

	private bool skipIt = false;

	private int points = 20;
	private int maxRounds = 2;
	private int currentRound = 1;
	private float pointGainTimer = 0.0f;
	private float pointGainInterval = 5.0f;
	private int pointGainAmount = 1;
	private int expectedTime = 120;
	private float textScale;

	private string message = "";
	private string endGameText = "";
	private string playerOneScoresText = "";
	private string playerTwoScoresText = "";
	
	private List<LapTime> PlayerOneLapTimes = new List<LapTime>();
	private List<LapTime> PlayerTwoLapTimes = new List<LapTime>();
	
	//End of lap
	private bool displayEndOfLapInfo = false;

	// Use this for initialization
	void Start () {

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		if(ball == null)
			ball = GameObject.FindGameObjectWithTag("TheBall");

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
		
		textScale = (style.fontSize * (Screen.width * 0.001f));
		style.fontSize = (int)textScale;
		textScale = (endGameTextStyle.fontSize * (Screen.width * 0.001f));
		endGameTextStyle.fontSize = (int)textScale;
		textScale = (lapStyle.fontSize * (Screen.width * 0.001f));
		lapStyle.fontSize = (int)textScale;
		textScale = (resourceStyle.fontSize * (Screen.width * 0.001f));
		resourceStyle.fontSize = (int)textScale;
		
	}
	
	[RPC]
	void ClientDisconnected(string name, int playerType)
	{
		ball.rigidbody.Sleep ();
		ball.GetComponent<AndroidControlScript> ().LockControls ();

		message = name + " has disconnected!"; 
		if (isPlaying) {
			message += " You win!";
		}
		message += " You will be returned to the menu in 5 seconds";

		Invoke("QuitGame", 5.0f);
	}


	void QuitGame()
	{
		if(isPlaying)
		{
			if(Network.isClient)
			{
				networkView.RPC("ClientDisconnected", RPCMode.Others, PlayerPrefs.GetString("PlayerName"), PlayerPrefs.GetInt("PlayerType"));
			}
		}
		Network.Disconnect();
		Application.LoadLevel(0);
	}
	void Update () {
		Debug.Log("IsBall: " + IsBall().ToString());
		Debug.Log("IsPLaying: " + isPlaying.ToString());
		Debug.Log("IsSwapped: " + isSwapped.ToString());
		
		if(ball == null)
			ball = GameObject.FindGameObjectWithTag("TheBall");
		
		if (Input.GetKey(KeyCode.Escape)) 
		{	
			QuitGame();
		}
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
			if (PlayerPrefs.GetInt("PlayerType") == 1) { //Trackplayer
				ballCamera.SetActive (false);
				GUICamera.SetActive (false);
				spectatorCamera.SetActive(false);
				overviewCamera.SetActive (true);
				overviewGUICamera.SetActive(true);
				isSwapped = true;
				isPlaying = true;
				networkView.RPC("SendPlayerNameToSpectator", RPCMode.AllBuffered, PlayerPrefs.GetString("PlayerName"), PlayerPrefs.GetInt("PlayerType"));
				networkView.RPC("SendPointsToSpectator", RPCMode.All, this.points, PlayerPrefs.GetInt ("PlayerType"));
			}
//			else if (GameObject.Find ("GlobalStorage").GetComponent<NetworkManager> ().GetId () == 0) {
			else if (PlayerPrefs.GetInt("PlayerType") == 0) { //Ballplayer
				ballCamera.SetActive (true);
				GUICamera.SetActive (true);
				overviewCamera.SetActive (false);
				overviewGUICamera.SetActive(false);
				spectatorCamera.SetActive(false);
				ball.GetComponent<KulanNetworkScript>().SetAsOwner();
				isPlaying = true;
				networkView.RPC("SendPlayerNameToSpectator", RPCMode.AllBuffered, PlayerPrefs.GetString("PlayerName"), PlayerPrefs.GetInt("PlayerType"));
				networkView.RPC("SendPointsToSpectator", RPCMode.All, this.points, PlayerPrefs.GetInt ("PlayerType"));
			}
			else { //Spectator
				ballCamera.SetActive(false);
				overviewCamera.SetActive(false);
				GUICamera.SetActive(false);
				overviewGUICamera.SetActive(false);
				spectatorCamera.SetActive(true);
				isPlaying = false;
				isSwapped = false;

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
			GetComponent<CountdownScript> ().StartCountDown (null);
		}
		
	}

	[RPC]
	public void SwapPlayers()
	{
		if (isPlaying) {
			if (isSwapped) {
				ballCamera.SetActive (true);
				GUICamera.SetActive (true);
				overviewCamera.SetActive (false);
				overviewGUICamera.SetActive(false);
				spectatorCamera.SetActive(false);
				ball.GetComponent<KulanNetworkScript> ().SetAsOwner ();
				ball.GetComponent<BallUtilityScript> ().ResetPosition ();
				isSwapped = false;
			} else {
				ballCamera.SetActive (false);
				GUICamera.SetActive (false);
				overviewCamera.SetActive (true);
				overviewGUICamera.SetActive(true);
				spectatorCamera.SetActive(false);
				isSwapped = true;
				ball.GetComponent<BallUtilityScript> ().ResetPosition ();
			}
		}
		GetComponent<LapTimerScript>().ResetTimer();
		GetComponent<LapTimerScript>().Pause();
		GetComponent<CountdownScript> ().StartCountDown (GetComponent<LapTimerScript>().Unpause);
		spectatorCamera.GetComponent<SpectatorCamScript> ().ResetToStart ();
		
		
		ballCamera.GetComponent<CameraPositioningScript> ().ResetToStart ();
		//Reactivate collectibles
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Collectable")) {
			go.GetComponent<CollectableScript> ().ResetCollectable ();
		}

	}
	
	public void EndOfLap(){
		displayEndOfLapInfo = true;
		BonusPoints();
		networkView.RPC ("SendLapTime", RPCMode.All, GetComponent<LapTimerScript>().GetLapTime(), PlayerPrefs.GetInt ("PlayerType"));
	}
	public void StartNewRound(){

		if (isPlaying && IsBall ()) {
			ball.rigidbody.Sleep();
			ball.GetComponent<AndroidControlScript>().LockControls();
			BonusPoints();
			
			
			networkView.RPC ("SendLapTime", RPCMode.All, GetComponent<LapTimerScript>().GetLapTime().GetMinutes(),GetComponent<LapTimerScript>().GetLapTime().GetSeconds(), PlayerPrefs.GetInt ("PlayerType"));
			
			if(currentRound == maxRounds)
			{
//				networkView.RPC ("ShowEndGameView", RPCMode.All, null);
				networkView.RPC("EndGame", RPCMode.All, CalculateWinner());
			}
			else
			{
				displayEndOfLapInfo = true;
				StartCoroutine(ResetRound());
			}
		}
	}
	
	private int CalculateWinner()
	{
		float playerOneAverageTime = 0;
		float playerTwoAverageTime = 0;
		for(int i = 0; i < maxRounds / 2; i ++)
		{
			playerOneAverageTime += PlayerOneLapTimes[i].GetLapTimeInSeconds();
			playerTwoAverageTime += PlayerTwoLapTimes[i].GetLapTimeInSeconds();
		}
		playerOneAverageTime /= maxRounds / 2;
		playerTwoAverageTime /= maxRounds / 2;
		
		if(playerOneAverageTime < playerTwoAverageTime)
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}
	
	private IEnumerator ResetRound(){
		yield return new WaitForSeconds(4);
		displayEndOfLapInfo = false;
		networkView.RPC ("SwapPlayers", RPCMode.All, null);
		networkView.RPC ("IncrementRound", RPCMode.All, null);
		if(currentRound > maxRounds){
			networkView.RPC ("EndGame", RPCMode.All, null);
		}
	} 
	
	[RPC]
	void SendPlayerNameToSpectator(string name, int playerType){
		
		spectatorCamera.GetComponent<SpectatorCamScript>().SetPlayerName(name, playerType);
	}
	
	[RPC]
	void EndGame(int winner){
//		ball.rigidbody.Sleep();
		ballCamera.SetActive(false);
		overviewCamera.SetActive(false);
		GUICamera.SetActive(false);
		overviewGUICamera.SetActive(false);
		spectatorCamera.SetActive(true);
		spectatorCamera.GetComponent<SpectatorCamScript>().ResetToStart();
		ball.GetComponent<BallUtilityScript>().ResetPosition();
		ball.rigidbody.WakeUp();
		
		ball.GetComponent<AndroidControlScript>().LockControls();
		if(isPlaying)
		{
			if(PlayerPrefs.GetInt("PlayerType") == winner)
				endGameText = "YOU WON!\n";
			else
				endGameText = "YOU SUCK!\n";
				
		}
		else
		{
			endGameText = "THANK YOU FOR WATCHING! PLAYER " + (winner + 1) + " WON!";
		}
		
		for(int i = 0; i < PlayerOneLapTimes.Count; i++){
			playerOneScoresText += PlayerOneLapTimes[i].ToString() + "\n";
		}
		for(int i = 0; i < PlayerTwoLapTimes.Count; i++){
			playerTwoScoresText += PlayerTwoLapTimes[i].ToString() + "\n";
		}
		
		StartCoroutine(DelayedExit());
	}
	
	IEnumerator DelayedExit()
	{
		yield return new WaitForSeconds(10);
		Network.Disconnect();
		Application.LoadLevel (0);
	}
	
	void BonusPoints(){
		int difference = expectedTime - (int)GetComponent<LapTimerScript>().GetLapTime().GetLapTimeInSeconds();
		if(difference > 0)
			AddPoints(difference);
	}

	[RPC]
	void IncrementRound(){
		currentRound++;
	}
	
	[RPC]
	void SendLapTime(int minutes, float seconds, int playerType){
	
		if(playerType == 0){
			PlayerOneLapTimes.Add(new LapTime(minutes, seconds));
		} else if (playerType == 1){
			PlayerTwoLapTimes.Add(new LapTime(minutes, seconds));
		} else {
			Debug.LogError ("Invalid lap time");
		}
	}
	
	[RPC]
	void ShowEndGameView(){
		//Nytt
		ballCamera.SetActive(false);
		overviewCamera.SetActive(false);
		GUICamera.SetActive(false);
		overviewGUICamera.SetActive(false);
		spectatorCamera.SetActive(true);
		spectatorCamera.GetComponent<SpectatorCamScript>().ResetToStart();
		ball.rigidbody.WakeUp();
	
	}

	public bool IsBall(){
		if(!isPlaying)
		{
			return false;
		}
		return !isSwapped;
	}

	void ShowOverviewAreas(){
		overviewGUICamera.camera.cullingMask = (1 << LayerMask.NameToLayer("OverviewGUI")) | (1 << LayerMask.NameToLayer("OverviewGUIAreas"));
		areaOverlayVisible = true;


	}

	public void AddPoints(int points){
		this.points += points;
		networkView.RPC("SendPointsToSpectator", RPCMode.All, this.points, PlayerPrefs.GetInt ("PlayerType"));
	}

	public void RemovePoints(int points){
		this.points -= points;
	}
	
	[RPC]
	void SendPointsToSpectator(int points, int playerType){
		spectatorCamera.GetComponent<SpectatorCamScript>().AddPoints(points, playerType);
	}

	public int GetPoints(){
		return points;
	}

	void OnGUI()
	{

		if(isPlaying){
			GUI.Label (new Rect (Screen.width * 0.045f, 0, 0, 0), points.ToString (), resourceStyle);
		}
		GUI.DrawTexture(new Rect (Screen.width * 0.005f, 0, Screen.width * 0.05f, Screen.width * 0.05f), crystal);
		if(!isPlaying){
			GUI.DrawTexture(new Rect (Screen.width * 0.995f - Screen.width * 0.05f, 0, Screen.width * 0.05f, Screen.width * 0.05f), crystal);
		}
		
		GUI.Label (new Rect(0, 40, 100, 50), message);
		endGameTextStyle.alignment = TextAnchor.UpperCenter;
		GUI.Label ( new Rect(Screen.width * 0.5f, Screen.height * 0.4f, 0.0f,0.0f), endGameText, endGameTextStyle);
		endGameTextStyle.alignment = TextAnchor.UpperRight;
		GUI.Label(new Rect(0, Screen.height * 0.5f, Screen.width * 0.5f - 20.0f, Screen.height * 0.5f), playerOneScoresText, endGameTextStyle);
		endGameTextStyle.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(Screen.width * 0.5f + 20.0f, Screen.height * 0.5f, Screen.width * 0.5f - 20.0f, Screen.height * 0.5f), playerTwoScoresText, endGameTextStyle);
		if(IsBall())
		{
			GUI.Label (new Rect(Screen.width * 0.5f, Screen.height * 0.1f, 0.0f,0.0f), "Lap " + (((currentRound - 1) / 2) + 1).ToString() + "/" + (maxRounds/2).ToString(), lapStyle);
		}
		if(displayEndOfLapInfo){
			Debug.Log(currentRound / 2 + " - " + PlayerOneLapTimes.Count);
			if(currentRound%2 == 1){
				GUI.Label( new Rect(Screen.width * 0.5f, Screen.height * 0.4f, 0.0f,0.0f), PlayerOneLapTimes[currentRound / 2].ToString(), style);
			}
			else{
				GUI.Label( new Rect(Screen.width * 0.5f, Screen.height * 0.4f, 0.0f,0.0f), PlayerTwoLapTimes[currentRound / 2 - 1].ToString(), style);
			}
		}
		
	}

	public GameObject GetPlacementBox(){
		if (skipIt) {
			skipIt = false;	
			return null;
		}
		return currentPlacementBox;

	}




}
