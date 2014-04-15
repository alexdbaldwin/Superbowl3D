using UnityEngine;
using System.Collections;

public class AndroidControlScript : MonoBehaviour {

	// Use this for initialization
	Vector2 fingerPosition = Vector2.zero;
	float speed = 3.0f;
	public GameObject gameCamera;
	public float turnSpeed = 20f;
	float trust = 30.0f;
	public float maxTurnSpeed = 25f; 
	public float velX = 0.0f;
	public Vector3 currentCollisionNormal;

	public bool jump;
	private float horizontalMovement;

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
/*
	void Update () {
		Vector3 direction = Vector3.zero;

		direction.x = -Input.acceleration.y;
		direction.z = Input.acceleration.x;

		if (direction.sqrMagnitude > 1) {
			direction.Normalize();
				}
		direction *= Time.deltaTime;

		transform.Translate(direction * speed);

	}*/

	void Update(){

		float width = Screen.width / 10.0f;

		Rect jumpButton = new Rect (Screen.width - width, 0, width, width);

		if (jump 
		    || (Input.GetMouseButtonUp (0) && jumpButton.Contains (Input.mousePosition)) 
		    || Input.touchCount > 0 && jumpButton.Contains(Input.GetTouch(0).position)) {
			jump = true;
		} else {
			jump = false;		
		}

		//horizontalMovement = Input.acceleration.x;
		horizontalMovement = Input.GetAxis ("Horizontal");
	}
	

	// Update is called once per frame
	void FixedUpdate () 
	{
	


		Vector3 right = gameCamera.transform.right;
		right.y = 0;
		right.Normalize ();
		
		Vector3 cross = Vector3.Cross (currentCollisionNormal, right);
		Vector3 forceDir = Vector3.Cross (cross, currentCollisionNormal);
		
		rigidbody.AddForce (forceDir * horizontalMovement * turnSpeed);
		if (jump) {
			rigidbody.AddForce(new Vector3(0,150,0));
			jump = false;
		}
		//rigidbody.AddForce (-currentCollisionNormal * 5f);
		//		rigidbody.AddForce (gameCamera.transform.forward * 5);
		
		
		
		//		if (rigidbody.velocity.x < maxTurnSpeed && rigidbody.velocity.x > -maxTurnSpeed) {
		//				rigidbody.AddForce (new Vector3 (horizontal * turnSpeed, 0, -vertical * trust));
		//		}
		//			Test
		
		
		
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 100, 100), rigidbody.velocity.x.ToString());
		GUI.Label (new Rect (0, 20, 100, 100), "Hello");
		//GUI.Label (new Rect (0, 40, 100, 100), "mouse pos: " + Input.mousePosition.ToString ());
//		float width = Screen.width / 10.0f;
//		if (jump || GUI.Button (new Rect (Screen.width - width, Screen.height - width, width, width), "jump")) {
//						jump = true;		
//				} else {
//			jump  = false;		
//		}
	}

	void OnCollisionStay(Collision collisionInfo) {
		
		foreach (ContactPoint contact in collisionInfo.contacts) {
			currentCollisionNormal = contact.normal;
		}
		currentCollisionNormal.Normalize ();
	}

}
