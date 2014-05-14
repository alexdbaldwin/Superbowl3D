using UnityEngine;
using System.Collections;

public class SpectatorCamTriggerScript : MonoBehaviour {
	public GameObject newTargetNode;
	public GameObject SpectatorCamera;
	public float lerpSpeed = 0.5f;
	// Use this for initialization
	
	void OnTriggerEnter(Collider CollInfo)
	{
		if (CollInfo.gameObject.tag == "TheBall") {
			if(newTargetNode != null){
				SpectatorCamera.GetComponent<SpectatorCamScript> ().SetTargetNode (newTargetNode);
				SpectatorCamera.GetComponent<SpectatorCamScript> ().SetSpeed(lerpSpeed);
			} else {
				SpectatorCamera.GetComponent<SpectatorCamScript> ().SetTargetNode (CollInfo.gameObject);
				SpectatorCamera.GetComponent<SpectatorCamScript> ().SetSpeed(2.0f);
			}
		}
	}
}
