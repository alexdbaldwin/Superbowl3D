using UnityEngine;
using System.Collections;

public class CameraPositioningScript : MonoBehaviour {
	public GameObject target;
	public float cameraHeight = 2.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetVelocity = -target.rigidbody.velocity;
		targetVelocity.Normalize ();
		transform.position = Vector3.Lerp(transform.position, target.transform.position + targetVelocity + new Vector3(0, cameraHeight, 0), Time.deltaTime);
		transform.LookAt (target.transform);
	}
}
