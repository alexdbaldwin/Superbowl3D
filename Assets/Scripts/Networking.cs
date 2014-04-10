using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {
	private string connectionIP = "10.5.5.41";
	public int connectionPort = 25001;
	
	public GameObject Player;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected) 
		{
			GUI.Label (new Rect (10, 10, 200, 20), "Status: Disconnected");
		}
		if (GUI.Button (new Rect (10, 30, 120, 20), "Client Connect")) 
		{
			Network.Connect (connectionIP, connectionPort);
		}
			if (GUI.Button(new Rect(10, 50, 120, 20), "Initialize Server"))
		{
			Network.InitializeServer(32, connectionPort, false);
		}
		else if (Network.peerType == NetworkPeerType.Client)
		{
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Client" + connectionIP);
			if (GUI.Button(new Rect(400, 30, 120, 20), "Disconnect"))
			{
				Network.Disconnect(200);
			}
		}
	}
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			rigidbody.position = syncPosition;
		}
	}
}