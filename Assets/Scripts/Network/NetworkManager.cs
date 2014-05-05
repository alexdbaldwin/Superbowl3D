using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	private string gameName = "SuperBowl3D";
	public HostData[] hostData;
	private int serverListPosY = 40;
	private int networkPlayerId = -1;
	// Use this for initialization
	void Start () {
		StartCoroutine("refreshHostList");
	}

	void Update(){
		}

	public void startServer()
	{
		Network.InitializeServer (32, 25001, !Network.HavePublicAddress());
		MasterServer.RegisterHost (gameName, "Server up. Press to join", "Vi testar");
		Debug.Log ("Started server");
	}

	public void Connect(int id)
	{
		Network.Connect(hostData[id]);
	}

	void OnConnectedToServer()
	{
		networkPlayerId = int.Parse(Network.player.ToString());
		Network.SetLevelPrefix (10);
		Application.LoadLevel(3);
	}
	
	void OnPlayerConnected()
	{
		networkPlayerId = int.Parse(Network.player.ToString());
		Network.SetLevelPrefix (10);
		Application.LoadLevel(3);
	}

	public int GetId()
	{
		return networkPlayerId;
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
		//Debug output on screen
		GUI.Label (new Rect (0, 0, 100, 100), "Player NetID: " + Network.player.ToString());

		if (!Network.isClient && !Network.isServer) {
			if (GUI.Button (new Rect (Screen.width - 100, Screen.height - 100, 100, 100), "Start Server")){
				startServer ();
			}
		
			if (GUI.Button (new Rect (Screen.width - 100, 100, 100, 100), "Refresh")) {
				StartCoroutine("refreshHostList");
			}
			if(hostData != null){
					for (int i = 0; i < hostData.Length; i++) {
					if(GUI.Button (new Rect (Screen.width - 300, serverListPosY * i, 300, 50), hostData [i].gameName + hostData [i].gameType)) {
								
							Connect (i);

					}
				}
			}
		}

	}
}
