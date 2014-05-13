using UnityEngine;
using System.Collections;

public class OutOfBoundScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collInfo)
	{
		if (collInfo.gameObject.tag == "TheBall") {
			collInfo.gameObject.transform.position = collInfo.gameObject.GetComponent<AndroidControlScript>().GetLastActiveNodePos();
			GameObject.FindGameObjectWithTag ("GameManager").GetComponent<CountdownScript> ().StartCountDown (null);
			collInfo.gameObject.rigidbody.velocity = Vector3.zero;
		}
	}
}

