using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	public string gameName = "SuperBowl3D";
	public string serverName;
	public GameObject Lobby;
	public HostData[] hostData;

	public bool startServer(string serverName)
	{
		this.serverName = serverName;
		hostData = MasterServer.PollHostList ();
		foreach(HostData hd in hostData){
			if(hd.gameName == serverName){
				Debug.Log("Duplicate name");
				return false;
				}
		}
		
		Network.InitializeServer (32, 25001, !Network.HavePublicAddress());
		MasterServer.RegisterHost (gameName, serverName, "Open");
		Debug.Log ("Started server");
		return true;
	}
	

	public void Connect(int id)
	{
		Network.Connect(hostData[id]);
	}


	public IEnumerator refreshHostList()
	{
		MasterServer.RequestHostList (gameName);
		yield return new WaitForSeconds (1.5f);
		hostData = MasterServer.PollHostList ();

	}

}
