﻿using UnityEngine;
using System.Collections;

public class InGameNetworkScript : MonoBehaviour {

	int numOfPlayersLoaded = 0;
	bool firstRoundStarted = false;

	// Use this for initialization
	void Start () {
		networkView.RPC ("HasLoadedLevel", RPCMode.Server, null);
	}
	
	// Update is called once per frame
	void Update () {
		if (!firstRoundStarted && Network.isServer && numOfPlayersLoaded == Network.connections.Length) {
			firstRoundStarted = true;
			networkView.RPC ("StartFirstRound", RPCMode.All, null);	
		}
	}

	[RPC]
	void StartFirstRound(){
		Time.timeScale = 1;
		GetComponent<CountdownScript> ().StartCountDown (GetComponent<LapTimerScript>().Unpause);
		GetComponent<LapTimerScript>().ResetTimer();
		GetComponent<LapTimerScript>().Pause();
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		//Network.Disconnect ();
		Application.LoadLevel (0);
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		if ((player.guid == PlayerPrefs.GetString ("Player1Guid")) || (player.guid == PlayerPrefs.GetString ("Player2Guid"))) {
			if (Network.isServer) {
				MasterServer.UnregisterHost();
				
			}
			
			Network.Disconnect ();
			Application.LoadLevel (0);

		}

	}

	[RPC] 
	void HasLoadedLevel(){
		numOfPlayersLoaded++;
	}
}
