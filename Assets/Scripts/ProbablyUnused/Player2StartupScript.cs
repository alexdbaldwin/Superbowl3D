using UnityEngine;
using System.Collections;

public class Player2StartupScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Network.isClient) {
						NetworkViewID newViewID = Network.AllocateViewID ();
						networkView.viewID = newViewID;
				}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
