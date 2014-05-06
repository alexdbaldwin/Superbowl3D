using UnityEngine;
using System.Collections;

public class FanScript : MonoBehaviour {
	// Use this for initialization
	public float pushValue;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (Vector3.forward * Time.deltaTime * 200);
	}

	void OnTriggerStay(Collider other)
	{
		if (other.name == "Kulan") {
					other.attachedRigidbody.AddForce(this.transform.forward * pushValue);
				}
	}
}
