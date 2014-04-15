using UnityEngine;
using System.Collections;

public class AndroidControlScript : MonoBehaviour {

	// Use this for initialization
	Vector2 fingerPosition = Vector2.zero;
	float speed = 3.0f;
	public GameObject gameCamera;
	public float turnSpeed = 20f;
	float trust = 30.0f;
	public float tiltThreshold = 1.1f;
	public float maxTurnSpeed = 25f; 
	public float velX = 0.0f;
	public Vector3 currentCollisionNormal;

	public bool jump, isOnSurface = false;
	private float horizontalMovement;

	void Start () {


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

	void Update(){

		float width = Screen.width / 7.0f;



		Rect jumpButton = new Rect (Screen.width - width, 0, width, width);

		if (jump 
		    || (Input.GetMouseButtonUp (0) && jumpButton.Contains (Input.mousePosition)) 
		    || Input.touchCount > 0 && jumpButton.Contains(Input.GetTouch(0).position)) {
			if (isOnSurface) {
				jump = true;
			}

		} else {
			jump = false;		
		}

		if (Input.acceleration.magnitude > tiltThreshold) {
			float accAmount = Input.acceleration.y - tiltThreshold;
			horizontalMovement = Input.acceleration.y;
				}

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
			rigidbody.AddForce(new Vector3(0,50,0));
			jump = false;
		}

		
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 100, 100), rigidbody.velocity.x.ToString());
		GUI.Label (new Rect (0, 20, 100, 100), "" + Input.acceleration.magnitude.ToString() + " " + rigidbody.velocity.ToString());

//		}
	}

	void OnCollisionStay(Collision collisionInfo) {


		foreach (ContactPoint contact in collisionInfo.contacts) {
			currentCollisionNormal = contact.normal;
			isOnSurface = true;
		}
		currentCollisionNormal.Normalize ();
	}

	void OnCollisionExit(Collision collisionInfo)
	{
		isOnSurface = false;
		}

}
