using UnityEngine;
using System.Collections;

public class CameraPositioningScript : MonoBehaviour {
	public GameObject target;
	public GameObject motherNode;
	public GameObject testCube;
	public Vector3 nodePos;
	public float cameraHeight = 2.0f;
	Transform[] directionNodes;

	// Use this for initialization
	void Start () {
		MeshRenderer[] derp;
		derp = motherNode.GetComponentsInChildren<MeshRenderer>();
		CapsuleCollider[] colliders;
		colliders = motherNode.GetComponentsInChildren<CapsuleCollider> ();
		foreach (MeshRenderer mr in derp)
		{
			mr.enabled = false;
		}
		foreach (CapsuleCollider cl in colliders) 
		{
			cl.enabled = false;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		

		directionNodes = motherNode.GetComponentsInChildren<Transform>();
		float minDistance = 10000.0f;
		Transform closest = null;
		foreach (Transform node in directionNodes) {
			float dist = Vector3.Distance(target.transform.position, node.position);
			if(dist < minDistance){
				closest = node;
				minDistance = dist;
			}
		}

		nodePos = closest.position;

		

		testCube.transform.position = closest.position;
		Vector3 cameraOffset = -closest.up;
		cameraOffset.y = 0;
		cameraOffset.Normalize ();
		cameraOffset *= 0.5f;
		cameraOffset.y += cameraHeight;

		transform.position = Vector3.Lerp (transform.position, target.transform.position + cameraOffset, Time.deltaTime);
		transform.LookAt (target.transform);



//		Vector3 targetVelocity = -target.rigidbody.velocity;
//		targetVelocity.Normalize ();
//		transform.position = Vector3.Lerp(transform.position, target.transform.position + targetVelocity + new Vector3(0, cameraHeight, 0), Time.deltaTime * 2.0f);
//		transform.LookAt (target.transform);
	}
}
