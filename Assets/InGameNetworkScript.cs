using UnityEngine;
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
		GameObject.FindGameObjectWithTag ("GameManager").GetComponent<CountdownScript> ().StartCountDown ();
	}

	[RPC] 
	void HasLoadedLevel(){
		numOfPlayersLoaded++;
	}
}
