using UnityEngine;
using System.Collections;

public class BumperScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnCollisionEnter(Collision cO)
	{
		foreach (ContactPoint contact in cO.contacts){
			Vector3 direction = Vector3.Reflect(cO.gameObject.rigidbody.velocity, contact.normal);
			direction -= Vector3.Dot(direction, transform.up)* transform.up;
			cO.gameObject.rigidbody.AddForce(direction * 3.0f,ForceMode.VelocityChange);
		}

	}
}