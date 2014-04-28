using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	private string gameName = "SuperBowl3D";
	private HostData[] hostData;
	private int serverListPosY = 40;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void startServer()
	{
		Network.InitializeServer (32, 25001, !Network.HavePublicAddress());
		MasterServer.RegisterHost (gameName, "Tja! Fet server pa g!", "Vi testar");
		Debug.Log ("Started server");
	}

	void refreshHostList()
	{
		MasterServer.RequestHostList (gameName);
		System.Threading.Thread.Sleep (1500);
		Debug.Log(MasterServer.PollHostList ().Length);
		hostData = MasterServer.PollHostList ();
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer) {
						if (GUI.Button (new Rect (800, 200, 200, 200), "Start Server"))
								startServer ();
		
						if (GUI.Button (new Rect (800, 450, 200, 200), "Refresh"))
								refreshHostList ();
						for (int i = 0; i < hostData.Length; i++) {
								if(GUI.Button (new Rect (600, serverListPosY * i, 300, 30), hostData [i].gameName + hostData [i].gameType))
										Network.Connect(hostData[i]);
						}
				}
	}
}
