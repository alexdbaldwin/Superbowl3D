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
		GameObject gm = GameObject.FindGameObjectWithTag ("GameManager");
		if (collInfo.gameObject.tag == "TheBall" && gm.GetComponent<GameManager>().IsBall() ) {
			collInfo.gameObject.transform.position = collInfo.gameObject.GetComponent<AndroidControlScript>().GetLastActiveNodePos();
			gm.GetComponent<CountdownScript> ().StartCountDown (null);
			gm.GetComponent<GameManager>().BroadcastCountdown();
			collInfo.gameObject.rigidbody.velocity = Vector3.zero;
		}
	}
}

