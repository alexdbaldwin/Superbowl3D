using UnityEngine;
using System.Collections;

public class KulanNetworkScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NetworkView newNetView = gameObject.AddComponent<NetworkView>();
		newNetView.stateSynchronization = NetworkStateSynchronization.ReliableDeltaCompressed;
		newNetView.observed = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnLevelWasLoaded(int id){
		if (id == 3) {
			NetworkView newNetView = gameObject.AddComponent<NetworkView>();
			newNetView.stateSynchronization = NetworkStateSynchronization.ReliableDeltaCompressed;
			newNetView.observed = gameObject.GetComponent<Rigidbody>();
		}
	}

	[RPC]
	void CreateBall(int i)
	{
		GameObject go = (GameObject)Network.Instantiate (Resources.Load("Prefabs/Kulan"), new Vector3 (0.0f, 22.0f, -4.0f), Quaternion.identity, 0);
		go.GetComponent<AndroidControlScript> ().gameCamera = gameObject.GetComponent<AndroidControlScript> ().gameCamera;
		go.GetComponent<AndroidControlScript> ().GUIManager = gameObject.GetComponent<AndroidControlScript> ().GUIManager;
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraPositioningScript> ().target = go;
		
	}

	[RPC]
	void ChangeOwner(NetworkViewID newID)
	{
		networkView.viewID = newID;
	}
	

	void OnGUI()
	{
		if (GUI.Button (new Rect (0, 0, 150, 150), "HEHU")) {
			networkView.RPC ("CreateBall", RPCMode.All, 1);
//						NetworkViewID newID = Network.AllocateViewID ();
//						networkView.RPC ("ChangeOwner", RPCMode.All, newID);

				}

	}
}
