using UnityEngine;
using System.Collections;

public class SpectatorCamTriggerScript : MonoBehaviour {
	public GameObject newTargetNode;
	public GameObject SpectatorCamera;
	// Use this for initialization
	
	void OnTriggerEnter(Collider CollInfo)
	{
		if(CollInfo.gameObject.tag == "TheBall")
			SpectatorCamera.GetComponent<SpectatorCamScript>().SetTargetNode(newTargetNode);
	}
}
