using UnityEngine;
using System.Collections;

public class JumpPadScript : MonoBehaviour {
	private Vector3 targetPosition;
	private Vector3 oldPosition;
	// Use this for initialization
	void Start () {
		oldPosition = transform.position;
		targetPosition = transform.position + transform.forward * 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Vector3.Lerp(transform.position, oldPosition, Time.deltaTime / 2);
	}

	void OnTriggerStay(Collider other)
	{
		if (other.name == "Kulan") {
			this.transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 2);
			other.attachedRigidbody.AddForce(0.0f, 25.0f, 0.0f);
			}
	}
}
