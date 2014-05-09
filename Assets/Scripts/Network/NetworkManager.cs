using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	private string gameName = "SuperBowl3D";
	public GameObject Lobby;
	public HostData[] hostData;
	private int serverListPosY = 40;
	private int networkPlayerId = -1;
	private int numOfPlayersLoaded;
	private bool firstRoundStarted = false;
	// Use this for initialization
	void Start () {
		//StartCoroutine("refreshHostList");
	}

	void Update(){
		if (!firstRoundStarted && Network.isServer && numOfPlayersLoaded == Network.connections.Length 
						&& Application.loadedLevel == 1) {
				firstRoundStarted = true;
				networkView.RPC ("StartFirstRound", RPCMode.AllBuffered, null);	
			}
		}

	void OnLevelWasLoaded(int i){
		if(i == 1)
			networkView.RPC ("HasLoadedLevel", RPCMode.Server, null);
		}

	[RPC]
	void StartFirstRound(){
		Time.timeScale = 1;
	}
	[RPC] 
	void HasLoadedLevel(){
		numOfPlayersLoaded++;
	}

	public void startServer(string serverName)
	{
		Network.InitializeServer (32, 25001, !Network.HavePublicAddress());
		MasterServer.RegisterHost (gameName, serverName);
		Debug.Log ("Started server");
	}

	public void Connect(int id)
	{
		Network.Connect(hostData[id]);
	}

	void OnConnectedToServer()
	{
//		networkPlayerId = int.Parse(Network.player.ToString());
//		Network.SetLevelPrefix (10);
//		Application.LoadLevel(1);


	}
	
	void OnPlayerConnected()
	{
	}

	public int GetId()
	{
		return networkPlayerId;
	}
	
	public void SetId(int newId)
	{
		networkPlayerId = newId;
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
//		Network.Disconnect ();
		Application.LoadLevel (0);

	}

	public IEnumerator refreshHostList()
	{
		MasterServer.RequestHostList (gameName);
		yield return new WaitForSeconds (1.5f);
		Debug.Log(MasterServer.PollHostList ().Length);
		hostData = MasterServer.PollHostList ();

	}

	void OnGUI()
	{
		GUI.Label(new Rect(0,0,100,50), "connections.Length = " + Network.connections.Length.ToString());
		GUI.Label(new Rect(0,60,100,50), "numOfPlayersLoaded = " + numOfPlayersLoaded.ToString());
		GUI.Label(new Rect(0,120,100,50), "LoadedLevel = " + Application.loadedLevel.ToString());
		GUI.Label(new Rect(0,180,100,50), "TimeScale = " + Time.timeScale.ToString());
//		if (!Network.isClient && !Network.isServer) {
//			if (GUI.Button (new Rect (Screen.width - 100, Screen.height - 100, 100, 100), "Start Server")){
//				//startServer ();
//			}
//		
//			if (GUI.Button (new Rect (Screen.width - 100, 100, 100, 100), "Refresh")) {
//				StartCoroutine("refreshHostList");
//			}
//			if(hostData != null){
//					for (int i = 0; i < hostData.Length; i++) {
//					if(GUI.Button (new Rect (Screen.width - 300, serverListPosY * i, 300, 50), hostData [i].gameName + hostData [i].gameType)) {
//								
//							Connect (i);
//
//					}
//				}
		//	}
		//}

	}
}
