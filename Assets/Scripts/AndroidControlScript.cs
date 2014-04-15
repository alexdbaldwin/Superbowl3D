using UnityEngine;
using System.Collections;

public class AndroidControlScript : MonoBehaviour {

	// Use this for initialization
	Vector2 fingerPosition = Vector2.zero;
	public GameObject gameCamera;
	public float turnSpeed = 20f;
	public float tiltThreshold = 0.5f;
	public float tiltThresholdMax = 0.5f;
	public float maxTurnSpeed = 25f; 
	public float velX = 0.0f;
	public Vector3 currentCollisionNormal;

	public bool jump, isOnSurface = false;
	private float horizontalMovement;

	private float counter = 0.0f;
	private bool isTouched = false;
	private Vector2 touchStartPosition;

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

		TouchStick ();




		if (jump 
		    || (Input.GetMouseButtonUp (0) && jumpButton.Contains (Input.mousePosition)) 
		    || Input.touchCount > 0 && jumpButton.Contains(Input.GetTouch(0).position)) {
			if (isOnSurface) {
				jump = true;
			}

		} else {
			jump = false;		
		}

		AccelerometerControls ();

	}
	

	// Update is called once per frame
	void FixedUpdate () 
	{
	


		Vector3 right = gameCamera.transform.right;
		right.y = 0;
		right.Normalize ();
		
		Vector3 cross = Vector3.Cross (currentCollisionNormal, right);
		Vector3 forceDir = Vector3.Cross (cross, currentCollisionNormal);

//		float maxVelocity = Mathf.Abs (rigidbody.velocity.x);
//		if (maxVelocity < 10.0f) {
			rigidbody.AddForce (forceDir * horizontalMovement * turnSpeed);
//				}


		if (jump) {
			rigidbody.AddForce(new Vector3(0,50,0));
			jump = false;
		}

		
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 20, 100, 100), "y:" + Input.acceleration.y.ToString() + " :" + Screen.width.ToString());

//		}
	}

	void OnCollisionStay(Collision collisionInfo) {


		foreach (ContactPoint contact in collisionInfo.contacts) {
			currentCollisionNormal = contact.normal;
			isOnSurface = true;
		}
		currentCollisionNormal.Normalize ();
	}

	void AccelerometerControls()
	{
		float accAmountY = Mathf.Abs(Input.acceleration.y);

		if (accAmountY > tiltThreshold) {
			if (accAmountY > tiltThresholdMax) {
				if (Input.acceleration.y < 0) {
					horizontalMovement = -tiltThresholdMax;
				}
				else {
					horizontalMovement = tiltThresholdMax;
				}

				return;
			}
			horizontalMovement = Input.acceleration.y;

		}
	}

	void TouchStick ()
	{
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				touchStartPosition = Input.GetTouch (0).position;
				isTouched = true;
			}
			if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				isTouched = false;
			}
			if (isTouched) {
				float posX = Input.GetTouch (0).position.x;
				if (posX != touchStartPosition.x) {
					horizontalMovement = (posX - touchStartPosition.x) / 100;
				}
			}
		}
	}

	void OnCollisionExit(Collision collisionInfo)
	{
		isOnSurface = false;
		}

}
