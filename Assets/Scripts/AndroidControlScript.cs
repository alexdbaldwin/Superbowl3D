using UnityEngine;
using System.Collections;

public class AndroidControlScript : MonoBehaviour {

	// Use this for initialization
	Vector2 fingerPosition = Vector2.zero;
	float speed = 3.0f;
	void Start () {
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.orientation = ScreenOrientation.AutoRotation;
	}
	
	// Update is called once per frame
	/*
	void Update () {
		float horizontal = Input.GetAxis ("Horizontal");

		fingerPosition = Input.GetTouch(0).position;

		if (Input.touchCount > 0) {
			if (fingerPosition.y <= Screen.height/2 ) {
				//gå åt höger
				rigidbody.AddForce (new Vector3 (horizontal, 0, 0));
			}
			else {
			//gå åt vänster
				rigidbody.AddForce (new Vector3 (-horizontal, 0, 0));
			}

		}
	}*/
	//Tilt controlls

	void Update () {
		Vector3 direction = Vector3.zero;

		direction.x = -Input.acceleration.y;
		direction.z = Input.acceleration.x;

		if (direction.sqrMagnitude > 1) {
			direction.Normalize();
				}
		direction *= Time.deltaTime;

		transform.Translate(direction * speed);

	}

}
