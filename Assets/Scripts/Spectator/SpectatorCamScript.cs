using UnityEngine;
using System.Collections;

public class SpectatorCamScript : MonoBehaviour {
	
	private GameObject firstNode;
	public GameObject ball;
	public GameObject targetNode;
	private float camSpeed = 2f;
	
	// Use this for initialization
	
	public void SetTargetNode(GameObject newTargetNode)
	{
		targetNode = newTargetNode;

	}

	public void ResetToStart(){
		targetNode = firstNode;
		transform.position = targetNode.transform.position;
	}
	
	void Start () {
		transform.position = targetNode.transform.position;
		firstNode = targetNode;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, targetNode.transform.position, camSpeed * Time.deltaTime);
		transform.LookAt(ball.transform.position);
	}

}
