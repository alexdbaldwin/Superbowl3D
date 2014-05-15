using UnityEngine;
using System.Collections;

public class FanScript : MonoBehaviour {
	// Use this for initialization
	public float pushValue;
	public GameObject blades;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		blades.transform.Rotate (Vector3.forward * Time.deltaTime * 200);
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "TheBall") {
					other.attachedRigidbody.AddForce(this.transform.forward * pushValue);
				}
	}
}
