using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Sprite tiltOff, tiltOn, on, off;
	public GameObject hostOn, joinOn, tiltButtonOn, tiltButtonOff, StartButton, OptionButton, ExitButton, NameButton, BackButton, MuteButton, OnOffButton;
	public GameObject GlobalStorage;
	bool tilt, optionsClicked, nameClicked = false, startClicked = false;
	public bool lobbyActive = false;
	public bool initMenu = false;
	public GUIStyle nameInputFieldStyle;
	GUIStyle serverNameLabel;
	bool hostClicked = false;
	bool backButtonEnabled = false;
	bool refreshServerList;
	bool mute = false;
	bool serverListIsShown = false;
	bool duplicateServer = false;
	float textScale;
//Server list stuff
	int serverListOffestY = (int)(Screen.height * 0.1f);
	int serverListPosY = (int)(Screen.height * 0.1f);
//	string nameString = SystemInfo.deviceName.ToString();
	string playerName = "PlayerName";
	string serverName = "DefaultServerName";
	float serverListRefreshTimer = 0;

	// Use this for initialization
	void Start () {
		playerName = PlayerPrefs.GetString("PlayerName");
		if(playerName == "") playerName = "PlayerName";
		mute = PlayerPrefs.GetInt ("Mute") == 1 ? true : false;
		tilt = PlayerPrefs.GetInt ("Tilt") == 1 ? true : false;
		
	
		hostOn.SetActive (false);
		joinOn.SetActive (false);
		NameButton.SetActive (false);
		tiltButtonOn.SetActive(false);
		tiltButtonOff.SetActive(false);
		BackButton.SetActive (false);
		MuteButton.SetActive (false);
		OnOffButton.SetActive (false);

		textScale = (nameInputFieldStyle.fontSize * (Screen.width * 0.001f));
		nameInputFieldStyle.fontSize = (int)textScale;

		serverNameLabel = nameInputFieldStyle;
		
		InvokeRepeating("GetHostList",0.0f,2.0f);
	}
	
	void GetHostList(){
		MasterServer.RequestHostList (GlobalStorage.GetComponent<NetworkManager>().gameName);
	
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

		if (lobbyActive) {
			SetBackButtonEnable(true);
				}

		if (serverListIsShown) {
			serverListRefreshTimer += Time.deltaTime;
			if (serverListRefreshTimer > 1.0f) {
				serverListRefreshTimer = 0;
				GlobalStorage.GetComponent<NetworkManager>().StartCoroutine("refreshHostList");
			}
		}
		
		if(nameClicked && Input.GetKeyDown(KeyCode.KeypadEnter)){
			SavePreferences();
			MenuReset();
		}
		//NetworkManagerSc.GetComponent<NetworkManager> ().StartCoroutine ("refreshHostList");
		//StartCoroutine("refreshHostList");
	}
	
	void Click(Vector2 position)
	{

		Ray ray = Camera.main.ScreenPointToRay (position);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (lobbyActive == false) {
				if (hit.collider.gameObject.name == "StartButton") {
					if (!startClicked) {
							MenuReset();
							hostOn.SetActive (true);
							joinOn.SetActive (true);
							refreshServerList = true;
							startClicked = true;
					} else {
							MenuReset ();
					}

				} else if (hit.collider.gameObject.name == "ExitButton") {
						Application.Quit ();
				} else if (hit.collider.gameObject.name == "HostButton") {
						SavePreferences ();
						//startserver();
						MenuHide ();
						hostClicked = true;

				} else if (hit.collider.gameObject.name == "JoinButton") {
						SavePreferences ();
						PresentServerList ();
						
				} else if (hit.collider.gameObject.name == "OptionButton") {
						if (!optionsClicked) {
							tiltButtonOn.SetActive (false);
							tiltButtonOff.SetActive (true);
							hostOn.SetActive (false);
							joinOn.SetActive (false);
							NameButton.SetActive (true);
							MuteButton.SetActive(true);
							OnOffButton.SetActive(true);
							optionsClicked = true;
						} else {
								MenuReset ();
						}

				} else if (hit.collider.gameObject.name == "TiltButton") {
						if (tilt) {
								hit.collider.gameObject.GetComponent<SpriteRenderer> ().sprite = tiltOff;
						} else {
								hit.collider.gameObject.GetComponent<SpriteRenderer> ().sprite = tiltOn;
						}
						tilt = !tilt;
				} else if (hit.collider.gameObject.name == "NameButton") {
						nameClicked = true;
				} else if (hit.collider.gameObject.name == "MuteButton") {
					if (!mute) {
						OnOffButton.GetComponent<SpriteRenderer> ().sprite = on;
						AudioListener.pause = true;
					} else {
						OnOffButton.GetComponent<SpriteRenderer> ().sprite = off;
						AudioListener.pause = false;
					}
					mute = !mute;
				}else if (hit.collider.gameObject.name == "On/OffButton") {
					if (!mute) {
						hit.collider.gameObject.GetComponent<SpriteRenderer> ().sprite = on;
						AudioListener.pause = true;
					} else {
						hit.collider.gameObject.GetComponent<SpriteRenderer> ().sprite = off;
						AudioListener.pause = false;
					}
					mute = !mute;
				}

			} else {
				if (!nameClicked || !hostClicked){
						//MenuReset ();
						}
			}	
		}
		if (backButtonEnabled) {
			if (BackButtonClicked(position) && !lobbyActive) {
				MenuReset();
			}
		}
	}
	public bool BackButtonClicked(Vector2 position)
	{
		Ray ray = Camera.main.ScreenPointToRay (position);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
						if (hit.collider.gameObject.name == "BackButton") {
							SavePreferences();
							return true;
						}
				}
		return false;
	}

	void PresentServerList()
	{
		MenuHide ();
		serverListIsShown = true;
		GlobalStorage.GetComponent<NetworkManager>().refreshHostList();
		backButtonEnabled = true;
		SetBackButtonEnable (true);
	}

	public void MenuHide(){
		MenuReset ();
		StartButton.SetActive(false);
		OptionButton.SetActive(false);
		ExitButton.SetActive(false);
		serverListIsShown = false;
		}

	public void MenuReset(){	
		hostOn.SetActive(false);
		joinOn.SetActive(false);
		NameButton.SetActive (false);
		tiltButtonOn.SetActive(false);
		tiltButtonOff.SetActive(false);
		SetBackButtonEnable (false);
		optionsClicked = false;
		startClicked = false;
		nameClicked = false;
		hostClicked = false;
		serverListIsShown = false;
		MuteButton.SetActive (false);
		OnOffButton.SetActive (false);
		gameObject.GetComponent<SplashScreen> ().Hide ();
		gameObject.GetComponent<SplashScreen> ().SetText ("No Text");
		if (lobbyActive) {
			lobbyActive = false;
				}
		StartButton.SetActive(true);
		OptionButton.SetActive(true);
		ExitButton.SetActive(true);
		}
	void SavePreferences(){
		PlayerPrefs.SetInt ("Tilt", tilt ? 1 : 0);
		PlayerPrefs.SetInt ("Mute", mute ? 1 : 0);
		PlayerPrefs.SetString ("PlayerName", playerName);
		PlayerPrefs.Save();
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
		MuteButton.SetActive (false);
		OnOffButton.SetActive (false);
		lobbyActive = true;
	}

	public void SetBackButtonEnable(bool set)
	{
		backButtonEnabled = set;
		BackButton.SetActive(set);
		}


	void OnGUI()
	{
		if (optionsClicked) {
			if (nameClicked) {
				playerName = playerName.Replace("\n", "");
				MenuHide();
				SetBackButtonEnable(true);
				nameClicked = true;
				optionsClicked = true;
				GUI.SetNextControlName("nameControll");
				playerName = GUI.TextField (new Rect ((Screen.width * 0.5f) - (Screen.width * 0.15f), Screen.height * 0.5f, Screen.width * 0.3f, Screen.height * 0.1f), playerName, 12, nameInputFieldStyle);
				GUI.FocusControl("nameControll");
				playerName = playerName.Replace("\n", "");
				
			}
		}
		if (hostClicked) {

			SetBackButtonEnable(true);
			GUI.Label(new Rect (Screen.width * 0.5f, Screen.height * 0.1f, 0, 0), "Server Name: ", serverNameLabel);

			serverName = GUI.TextField (new Rect ((Screen.width * 0.5f) - (Screen.width * 0.15f), Screen.height * 0.3f, Screen.width * 0.3f, Screen.height * 0.1f), serverName, 20, nameInputFieldStyle);
			
			if (GUI.Button( new Rect (Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.2f, 30.0f), "OK", serverNameLabel)) {
				
				if(GlobalStorage.GetComponent<NetworkManager>().startServer(serverName)){
					LobbyActive ();
					hostClicked = false;
					SetBackButtonEnable(false);
				} else {
				
					duplicateServer = true;
					//do something here
				
				}
				
			}
			if (duplicateServer) {
				GUI.Label(new Rect (Screen.width * 0.5f, Screen.height * 0.65f, 0, 0), "Server name is taken", serverNameLabel);
			}
		}
		if (serverListIsShown) {
			GUI.Label(new Rect (Screen.width * 0.5f, Screen.height * 0.05f, 0, 0), "Click servers to join", serverNameLabel);
			if(GlobalStorage.GetComponent<NetworkManager>().hostData != null)
			{
				if (GlobalStorage.GetComponent<NetworkManager>().hostData.Length > 0) {
					for (int i = 0; i < GlobalStorage.GetComponent<NetworkManager>().hostData.Length; i++) {
						if(GUI.Button (new Rect (Screen.width * 0.4f, serverListPosY * i + serverListOffestY, Screen.width * 0.2f, serverListPosY), GlobalStorage.GetComponent<NetworkManager>().hostData [i].gameName, serverNameLabel)) {
							GlobalStorage.GetComponent<NetworkManager>().Connect (i);
							LobbyActive();
							serverListIsShown = false;
						}
					}
				}
			}
			else {
				GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 0, 0), "Waiting for servers...", serverNameLabel);
			}

		}
	}
}
