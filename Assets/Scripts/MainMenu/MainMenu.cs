using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Sprite tiltOff, tiltOn;
	public GameObject hostOn, joinOn, tiltButtonOn, tiltButtonOff, StartButton, OptionButton, ExitButton;
	public GameObject NetworkManagerSc;
	bool tilt, optionsClicked = false;
	public bool lobbyActive = false;
	public bool initMenu = false;
	bool refreshServerList;
//	string nameString = SystemInfo.deviceName.ToString();
	string playerName = "Alex";
	// Use this for initialization
	void Start () {
		hostOn.SetActive (false);
		joinOn.SetActive (false);
		tiltButtonOn.SetActive(false);
		tiltButtonOff.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			Click(Input.GetTouch(0).position);
		} else if (Input.GetMouseButtonUp (0)) {
			Click(Input.mousePosition);
		}
		if (initMenu == true) {
			lobbyActive = false;
			StartButton.SetActive(true);
			OptionButton.SetActive(true);
			ExitButton.SetActive(true);
			initMenu = false;
		}
		//NetworkManagerSc.GetComponent<NetworkManager> ().StartCoroutine ("refreshHostList");
		//StartCoroutine("refreshHostList");
	}
	
	void Click(Vector2 position)
	{
		if (lobbyActive == false) {
		Ray ray = Camera.main.ScreenPointToRay (position);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject.name == "StartButton") {
					hostOn.SetActive (true);
					joinOn.SetActive (true);
					tiltButtonOn.SetActive (false);
					tiltButtonOff.SetActive (false);
					optionsClicked = false;
					refreshServerList = true;
			} else if (hit.collider.gameObject.name == "ExitButton") {
					Application.Quit ();
			} else if (hit.collider.gameObject.name == "HostButton") {
					SavePreferences ();
					//startserver();
					LobbyActive ();
					NetworkManagerSc.GetComponent<NetworkManager>().startServer();
			} else if (hit.collider.gameObject.name == "Join") {
					SavePreferences ();
					//joinserver();
					for (int i = 0; i < NetworkManagerSc.GetComponent<NetworkManager>().hostData.Length; i++) {
							NetworkManagerSc.GetComponent<NetworkManager> ().Connect (i);
					}
			} else if (hit.collider.gameObject.name == "OptionButton") {
					tiltButtonOn.SetActive (false);
					tiltButtonOff.SetActive (true);
					hostOn.SetActive (false);
					joinOn.SetActive (false);
					optionsClicked = true;
			} else if (hit.collider.gameObject.name == "TiltButton") {
					if (tilt) {
							hit.collider.gameObject.GetComponent<SpriteRenderer> ().sprite = tiltOff;
					} else {
							hit.collider.gameObject.GetComponent<SpriteRenderer> ().sprite = tiltOn;
					}
					tilt = !tilt;
			}
		} else{
				MenuReset ();
		}
		}
	}
	void MenuReset(){	
		hostOn.SetActive(false);
		joinOn.SetActive(false);
		tiltButtonOn.SetActive(false);
		tiltButtonOff.SetActive(false);
		optionsClicked = false;
		if (lobbyActive) {
			StartButton.SetActive(true);
			OptionButton.SetActive(true);
			ExitButton.SetActive(true);
			lobbyActive = false;
				}
		}
	void SavePreferences(){
		PlayerPrefs.SetInt ("Tilt", tilt ? 1 : 0);
		PlayerPrefs.SetString ("PlayerName", playerName);
	}
	void LobbyActive()
	{
		StartButton.SetActive(false);
		OptionButton.SetActive(false);
		ExitButton.SetActive(false);
		hostOn.SetActive(false);
		joinOn.SetActive(false);
		tiltButtonOn.SetActive(false);
		tiltButtonOff.SetActive(false);
		lobbyActive = true;
	}
	void OnGUI()
	{
		if (optionsClicked) {
			playerName = GUI.TextField (new Rect (350f, 125f, 150.0f, 30.0f), playerName);
				}
	
	}
}
