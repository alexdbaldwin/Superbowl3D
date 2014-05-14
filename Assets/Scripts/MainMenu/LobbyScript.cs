using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public enum PlayerState {
	PLAYERONE, PLAYERTWO, OBSERVER, NONE
}

public class LobbyScript : MonoBehaviour {
	public GameObject Menu;
	public GUIStyle guiStyle, launchStyle, labelStyle;
	private GUIStyle playerStyle;
	List<string> observerList = new List<string>(5);
	List<ConnectedPlayer> playerList = new List<ConnectedPlayer>();
	bool[] observerLocks = new bool[5];
	const string observerText = "-Free Slot-";
	const string playerJoinText = "-Free Slot-";
	string player1 = playerJoinText;
	string player2 = playerJoinText;
	const string launchText = "Let's Roll!";
	const string ballLabel = "Team Ball";
	const string trackLabel = "Team Track";
	const string observerLabel = "Join as Observer";
	bool player1Locked = false;
	bool player2Locked = false;
	bool showDisconnectedFromServerText = false;
	float timer = 0;
	int currentObserverIndex = -1;
	float offsetX = Screen.width * 0.05f;
	float offsetY = Screen.height * 0.05f;
	float textScale;
	Color textColorGray = new Color(64.0f, 64.0f, 64.0f);
	
	PlayerState currentPlayerState = PlayerState.NONE;

	public GameObject globalStorage;
	
	public void AddPlayer(string name)
	{
		ConnectedPlayer newPlayer = new ConnectedPlayer(name);
		playerList.Add(newPlayer);
	}
	
	public void ClearPlayerList()
	{
		playerList.Clear();
	}
	// Use this for initialization
	void Start () {
		//globalStorage.GetComponent<NetworkManager>().SetId(2);
		for (int i = 0; i < 5; i++) {
			observerList.Add(observerText);
				observerLocks[i] = false;
		}

		textScale = (guiStyle.fontSize * (Screen.width * 0.001f));
		guiStyle.fontSize = (int)textScale;
		textScale = (launchStyle.fontSize * (Screen.width * 0.001f));
		launchStyle.fontSize = (int)(textScale);
		textScale = (labelStyle.fontSize * (Screen.width * 0.001f));
		labelStyle.fontSize = (int)(textScale);

		playerStyle = labelStyle;

	}
	
	// Update is called once per frame
	void Update () {
//	if (Menu.GetComponent<MainMenu>().lobbyActive) {
//			if (globalStorage.GetComponent<NetworkManager>().hostData.Length !=0) {
//				for (int i = 0; i < Network.connections.Length; i++) {
//					observerList.Add(PlayerPrefs.GetString("PlayerName"));
//				}
//			}
//		}
		if(Menu.GetComponent<MainMenu>().lobbyActive)
		WaitingForLobby ();
		
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			Click(Input.GetTouch(0).position);
		} else if (Input.GetMouseButtonUp (0)) {
			Click(Input.mousePosition);
			}
			
		if(showDisconnectedFromServerText)
		{
			timer += Time.deltaTime;
			if (timer > 2.0f) {
				timer = 0;
				showDisconnectedFromServerText = false;
				Menu.GetComponent<MainMenu>().MenuReset();
			}
		
		}
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info) {
	
		RemovePlayer1();
		RemovePlayer2();
		for (int i = 0; i < observerList.Count; i++) {
			RemoveObserver(i);
		}
		
		
		showDisconnectedFromServerText = true;
		Menu.GetComponent<MainMenu>().MenuHide();
		
	}

	void WaitingForLobby ()
	{
		if (Network.connections.Length == 0) {
			Menu.GetComponent<SplashScreen> ().Show ();
			Menu.GetComponent<SplashScreen> ().SetText("Waiting for other players to connect...");
			guiStyle.normal.textColor = textColorGray;
			labelStyle.normal.textColor = textColorGray;
		}
		else {
			Menu.GetComponent<SplashScreen> ().Hide ();
		}
	}


	[RPC]
	void SetPlayer1(string name)
	{
		player1 = name;
		player1Locked = true;
	}
	
	[RPC]
	void RemovePlayer1()
	{
		player1 = playerJoinText;
		player1Locked = false;
	}

	[RPC]
	void SetPlayer2(string name)
	{
		player2 = name;
		player2Locked = true;
	}
	
	[RPC]
	void RemovePlayer2()
	{
		player2 = playerJoinText;
		player2Locked = false;
	}
	
	[RPC]
	void SetObserver(int index, string name)
	{
		observerList[index] = name;
		observerLocks[index] = true;
	}
	
	[RPC] 
	void RemoveObserver(int index)
	{
		observerList[index] = observerText;
		observerLocks[index] = false;
	}
	
	[RPC]
	void StartGame()
	{
		Time.timeScale = 0;
		Application.LoadLevel(1);

	}

	void Click(Vector2 position)
	{
		Ray ray = Camera.main.ScreenPointToRay (position);
	
		if (Menu.GetComponent<MainMenu>().BackButtonClicked(position)) {
			
			if(currentPlayerState == PlayerState.PLAYERONE)
				networkView.RPC("RemovePlayer1", RPCMode.AllBuffered, null);
			else if(currentPlayerState == PlayerState.PLAYERTWO)
				networkView.RPC("RemovePlayer2", RPCMode.AllBuffered, null);
			else if(currentPlayerState == PlayerState.OBSERVER)
				networkView.RPC("RemoveObserver", RPCMode.AllBuffered, currentObserverIndex);
			Network.Disconnect();
			if(Network.isServer)
			{
				MasterServer.UnregisterHost();
				
			}
			RemovePlayer1();
			RemovePlayer2();
			Menu.GetComponent<MainMenu>().MenuReset();
		}

	}

	void OnGUI()
	{
		if(showDisconnectedFromServerText)
		{
			string text = "Disconnected from server";
			GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 0, 0), text, labelStyle);
		}
		
		if (Menu.GetComponent<MainMenu>().lobbyActive && !Menu.GetComponent<SplashScreen> ().IsActive()) {
			
			//Mode labels
			GUI.Label(new Rect(offsetX, offsetY + Screen.height * 0.0f, 200, 40), ballLabel, labelStyle);
			GUI.Label(new Rect(offsetX + Screen.width * 0.3f, offsetY + Screen.height * 0.0f, 200, 40), trackLabel, labelStyle);
			GUI.Label(new Rect(offsetX + Screen.width * 0.6f, offsetY + Screen.height * 0.0f, 200, 40), observerLabel, labelStyle);

			//Player1 button
			if (GUI.Button (new Rect (offsetX, offsetY + Screen.height * 0.1f, 200, 40), player1, player1Locked ? playerStyle : guiStyle) && !player1Locked && Network.connections.Length > 0) {
				
				if(currentPlayerState == PlayerState.PLAYERTWO)
					networkView.RPC("RemovePlayer2", RPCMode.AllBuffered, null);
				else if(currentPlayerState == PlayerState.OBSERVER)
				{
					networkView.RPC("RemoveObserver", RPCMode.AllBuffered, currentObserverIndex);
				}
				networkView.RPC("SetPlayer1", RPCMode.AllBuffered, PlayerPrefs.GetString("PlayerName"));
				currentPlayerState = PlayerState.PLAYERONE;
//				globalStorage.GetComponent<NetworkManager>().SetId(0);
				PlayerPrefs.SetInt("PlayerType", 0);
				currentObserverIndex = -1;
			}
			//Player2 button
			if (GUI.Button (new Rect (offsetX + Screen.width * 0.3f, offsetY + Screen.height * 0.1f, 200, 40), player2, player2Locked ? playerStyle : guiStyle) && !player2Locked && Network.connections.Length > 0) {
				if(currentPlayerState == PlayerState.PLAYERONE)
					networkView.RPC("RemovePlayer1", RPCMode.AllBuffered, null);
				else if(currentPlayerState == PlayerState.OBSERVER)
				{
					networkView.RPC("RemoveObserver", RPCMode.AllBuffered, currentObserverIndex);
				}
				networkView.RPC("SetPlayer2", RPCMode.AllBuffered, PlayerPrefs.GetString("PlayerName"));
				currentPlayerState = PlayerState.PLAYERTWO;
//				globalStorage.GetComponent<NetworkManager>().SetId(1);
				PlayerPrefs.SetInt("PlayerType", 1);
				currentObserverIndex = -1;
			}
//			for (int i = 0; i < playerList.Count; i++) {
//				GUI.Button (new Rect (600, 100 + i * 50, 200, 40), "Observer: " + playerList[i].GetName());
//			}

			for (int i = 0; i < observerList.Count; i++) {
				
				if(GUI.Button (new Rect (offsetX + Screen.width * 0.6f, offsetY + Screen.height * 0.1f + i * Screen.height * 0.1f, 200, 40), observerList[i].ToString(), guiStyle) && !observerLocks[i] && Network.connections.Length > 0){
					if(currentPlayerState == PlayerState.PLAYERONE)
						networkView.RPC("RemovePlayer1", RPCMode.AllBuffered, null);
					else if(currentPlayerState == PlayerState.PLAYERTWO)
						networkView.RPC("RemovePlayer2", RPCMode.AllBuffered, null);
					else if(currentPlayerState == PlayerState.OBSERVER && currentObserverIndex != i)
					{
						networkView.RPC("RemoveObserver", RPCMode.AllBuffered, currentObserverIndex);
					}
					networkView.RPC("SetObserver", RPCMode.AllBuffered, i, PlayerPrefs.GetString("PlayerName"));
					currentPlayerState = PlayerState.OBSERVER;
//					globalStorage.GetComponent<NetworkManager>().SetId(2);
					PlayerPrefs.SetInt("PlayerType", 2);
					currentObserverIndex = i;
				}
			}
			if(Network.isServer && player1Locked && player2Locked){
				if (GUI.Button (new Rect (Screen.width * 0.6f, Screen.height * 0.7f , launchStyle.fontSize * launchText.Length * 0.5f, launchStyle.fontSize * 2), launchText, launchStyle) && player1Locked && player2Locked) {
						networkView.RPC("StartGame", RPCMode.All, null);
				}
			}
			else {
				string text = "";
				if(player1Locked && player2Locked)
				{
					text = "Waiting for host to launch...";
				}
				else {
					text = "Waiting for players...";
				}
				GUI.Label(new Rect (Screen.width * 0.6f, Screen.height * 0.7f , launchStyle.fontSize * launchText.Length * 0.5f, launchStyle.fontSize * 2), text, guiStyle);
			}
		}
	}
}


