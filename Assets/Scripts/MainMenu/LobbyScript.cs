using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public enum PlayerState {
	PLAYERONE, PLAYERTWO, OBSERVER, NONE
}

public class LobbyScript : MonoBehaviour {
	public GameObject Menu;
	List<string> observerList = new List<string>(5);
	List<ConnectedPlayer> playerList = new List<ConnectedPlayer>();
	bool[] observerLocks = new bool[5];
	string player1 = "Player 1";
	string player2 = "Player 2";
	bool player1Locked = false;
	bool player2Locked = false;
	int currentObserverIndex = -1;
	
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
		globalStorage.GetComponent<NetworkManager>().SetId(2);
		for (int i = 0; i < 5; i++) {
				observerList.Add("Observer");
				observerLocks[i] = false;
		}
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
		player1 = "Player 1";
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
		player2 = "Player 2";
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
		observerList[index] = "Observer";
		observerLocks[index] = false;
	}
	
	[RPC]
	void StartGame()
	{
		Application.LoadLevel(1);
	}

	void OnGUI()
	{
		
		if (Menu.GetComponent<MainMenu>().lobbyActive) {
			if (GUI.Button (new Rect (100, 100, 200, 40), player1) && !player1Locked) {
				if(currentPlayerState == PlayerState.PLAYERTWO)
					networkView.RPC("RemovePlayer2", RPCMode.AllBuffered, null);
				else if(currentPlayerState == PlayerState.OBSERVER)
				{
					networkView.RPC("RemoveObserver", RPCMode.AllBuffered, currentObserverIndex);
				}
				networkView.RPC("SetPlayer1", RPCMode.AllBuffered, PlayerPrefs.GetString("PlayerName"));
				currentPlayerState = PlayerState.PLAYERONE;
				globalStorage.GetComponent<NetworkManager>().SetId(0);
				currentObserverIndex = -1;
			}
			if (GUI.Button (new Rect (350, 100, 200, 40), player2) && !player2Locked) {
				if(currentPlayerState == PlayerState.PLAYERONE)
					networkView.RPC("RemovePlayer1", RPCMode.AllBuffered, null);
				else if(currentPlayerState == PlayerState.OBSERVER)
				{
					networkView.RPC("RemoveObserver", RPCMode.AllBuffered, currentObserverIndex);
				}
				networkView.RPC("SetPlayer2", RPCMode.AllBuffered, PlayerPrefs.GetString("PlayerName"));
				currentPlayerState = PlayerState.PLAYERTWO;
				globalStorage.GetComponent<NetworkManager>().SetId(1);
				currentObserverIndex = -1;
			}
//			for (int i = 0; i < playerList.Count; i++) {
//				GUI.Button (new Rect (600, 100 + i * 50, 200, 40), "Observer: " + playerList[i].GetName());
//			}
			for (int i = 0; i < observerList.Count; i++) {
				
				if(GUI.Button (new Rect (600, 100 + i * 50, 200, 40), observerList[i].ToString()) && !observerLocks[i]){
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
					globalStorage.GetComponent<NetworkManager>().SetId(2);
					currentObserverIndex = i;
				}
			}
			if (GUI.Button (new Rect (100, 500, 200, 40), "Back")) {
				Menu.GetComponent<MainMenu>().initMenu = true;
				player1 = "";
				player2 = "";
				observerList.Clear();
				Network.Disconnect();
			}
			if(Network.isServer){
					if (GUI.Button (new Rect (350, 500, 200, 40), "Launch") && player1Locked && player2Locked) {
						networkView.RPC("StartGame", RPCMode.All, null);
				}
			}
		}
	}
}


