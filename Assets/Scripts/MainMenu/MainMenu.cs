using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Sprite tiltOff, tiltOn;
	public GameObject hostOn, joinOn, tiltButtonOn, tiltButtonOff, StartButton, OptionButton, ExitButton, NameButton, BackButton;
	public GameObject GlobalStorage;
	bool tilt, optionsClicked, nameClicked = false, startClicked = false;
	public bool lobbyActive = false;
	public bool initMenu = false;
	public GUIStyle nameInputFieldStyle;
	GUIStyle serverNameLabel;
	bool hostClicked = false;
	bool backButtonEnabled = false;
	bool refreshServerList;
	bool serverListIsShown = false;
//Server list stuff
	int serverListPosY = 40;
//	string nameString = SystemInfo.deviceName.ToString();
	string playerName = "PlayerName";
	string serverName = "DefaultServerName";
	// Use this for initialization
	void Start () {
		hostOn.SetActive (false);
		joinOn.SetActive (false);
		NameButton.SetActive (false);
		tiltButtonOn.SetActive(false);
		tiltButtonOff.SetActive(false);
		BackButton.SetActive (false);
		serverNameLabel = nameInputFieldStyle;
		serverNameLabel.fontSize = 40;
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
			if((int)Time.deltaTime % 2 == 0)
			{
				GlobalStorage.GetComponent<NetworkManager>().StartCoroutine("refreshHostList");
			}
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
												hostOn.SetActive (true);
												joinOn.SetActive (true);
												NameButton.SetActive (false);
												tiltButtonOn.SetActive (false);
												tiltButtonOff.SetActive (false);
												optionsClicked = false;
												nameClicked = false;
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
										hostClicked = true;
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
								}
			
						} else {
								if (!nameClicked || !hostClicked)
										MenuReset ();
						}


						
				}
		if (backButtonEnabled) {
			if (BackButtonClicked(position)) {
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

	}

	void MenuHide(){
		MenuReset ();
		StartButton.SetActive(false);
		OptionButton.SetActive(false);
		ExitButton.SetActive(false);
		
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
		if (lobbyActive) {

			lobbyActive = false;
				}
		StartButton.SetActive(true);
		OptionButton.SetActive(true);
		ExitButton.SetActive(true);
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

	public void SetBackButtonEnable(bool set)
	{
		backButtonEnabled = set;
		BackButton.SetActive(set);
		}


	void OnGUI()
	{
		if (optionsClicked) {
			if (nameClicked) {
				MenuHide();
				SetBackButtonEnable(true);
				nameClicked = true;
				optionsClicked = true;
				GUI.SetNextControlName("nameControll");
				playerName = GUI.TextField (new Rect (Screen.width * 0.5f, Screen.height * 0.5f, 150.0f, 30.0f), playerName, 10, nameInputFieldStyle);
				GUI.FocusControl("nameControll");
			}
		}
		if (hostClicked) {

			SetBackButtonEnable(true);
			GUI.Label(new Rect (Screen.width * 0.5f, Screen.height * 0.1f, 150.0f, 30.0f), "Server Name: ", serverNameLabel);

			serverName = GUI.TextField (new Rect (Screen.width * 0.5f, Screen.height * 0.3f, 150.0f, 30.0f), serverName, 20, nameInputFieldStyle);

			if (GUI.Button( new Rect (Screen.width * 0.5f, Screen.height * 0.4f, 150.0f, 30.0f), "OK", serverNameLabel)) {
				LobbyActive ();
				GlobalStorage.GetComponent<NetworkManager>().startServer(serverName);
				hostClicked = false;
				SetBackButtonEnable(false);
			}
			}
		if (serverListIsShown) {

			if (GlobalStorage.GetComponent<NetworkManager>().hostData.Length > 0) {
				for (int i = 0; i < GlobalStorage.GetComponent<NetworkManager>().hostData.Length; i++) {
					if(GUI.Button (new Rect (Screen.width * 0.5f, serverListPosY * i, 300, 50), GlobalStorage.GetComponent<NetworkManager>().hostData [i].gameName, serverNameLabel)) {
						GlobalStorage.GetComponent<NetworkManager>().Connect (i);
						LobbyActive();
						serverListIsShown = false;
					}
				}
			}
			else {
				GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 0, 0), "Waiting for servers...", serverNameLabel);
			}

		}
	}
}
