using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	private string gameName = "SuperBowl3D";
	public GameObject Lobby;
	public HostData[] hostData;

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


	public IEnumerator refreshHostList()
	{
		MasterServer.RequestHostList (gameName);
		yield return new WaitForSeconds (1.5f);
		Debug.Log(MasterServer.PollHostList ().Length);
		hostData = MasterServer.PollHostList ();

	}

}
