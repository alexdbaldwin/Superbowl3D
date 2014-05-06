using UnityEngine;
using System.Collections;

public class SpectatorCamScript : MonoBehaviour {
	
	
	public GameObject ball;
	public GameObject targetNode;
	private float camSpeed = 2f;
	
	// Use this for initialization
	
	public void SetTargetNode(GameObject newTargetNode)
	{
		targetNode = newTargetNode;
	}
	
	void Start () {
		transform.position = targetNode.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, targetNode.transform.position, camSpeed * Time.deltaTime);
		transform.LookAt(ball.transform.position);
	}
}
