using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyScript : MonoBehaviour {
	public GameObject Menu;
	List<string> observerList = new List<string>();
	string serverName = "";
	string player1 = "Player 1";
	string player2 = "Player 2";
	bool player1Locked = false;
	bool player2Locked = false;

	public GameObject globalStorage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (Menu.GetComponent<MainMenu>().lobbyActive) {
			if (globalStorage.GetComponent<NetworkManager>().hostData.Length !=0) {
				for (int i = 0; i < Network.connections.Length; i++) {
					observerList.Add(PlayerPrefs.GetString("PlayerName"));
				}
			}
		}
	}

	[RPC]
	void SetPlayer1(string name)
	{
		player1 = name;
	}

	[RPC]
	void SetPlayer2(string name)
	{
		player2 = name;
	}

	void OnGUI()
	{
		if (Menu.GetComponent<MainMenu>().lobbyActive) {
			if (GUI.Button (new Rect (100, 100, 200, 40), player1)) {
				networkView.RPC("SetPlayer1", RPCMode.AllBuffered, PlayerPrefs.GetString("PlayerName"));
				if (player1 == player2) {
					player2 = "Player 2";
				}
				else {
					for (int i = 0; i < observerList.Count; i++) {
						if (observerList.Contains(player1)) {
							observerList.Remove(player1);
						}
					}
				}
			}
			if (GUI.Button (new Rect (350, 100, 200, 40), player2)) {
				networkView.RPC("SetPlayer2", RPCMode.AllBuffered, PlayerPrefs.GetString("PlayerName"));
				if (player2 == player1) {
					player1 = "Player 1";
				}
				else {
					for (int i = 0; i < observerList.Count; i++) {
						if (observerList.Contains(player2)) {
							observerList.Remove(player2);
						}
					}
				}
			}
			for (int i = 0; i < Network.connections.Length + 1; i++) {
				GUI.Button (new Rect (600, 100 + i * 50, 200, 40), "Observer: " + PlayerPrefs.GetString ("PlayerName"));
			}
			if (GUI.Button (new Rect (100, 500, 200, 40), "Back")) {
				Menu.GetComponent<MainMenu>().initMenu = true;
				player1 = "";
				player2 = "";
				observerList.Clear();
				Network.Disconnect();
			}
			if(Network.isServer){
					if (GUI.Button (new Rect (350, 500, 200, 40), "Launch")) {
					//Startar spelet.

				}
			}
		}
	}
}


