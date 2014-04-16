using UnityEngine;
using System.Collections;

public class AndroidControlScript : MonoBehaviour {
	public GameObject gameCamera;
	//Debug output
	//Kontrollprylar
	private float tiltThreshold = 0.1f;
	private float tiltThresholdMax = 0.6f;
	private bool isJumping, isBoosting = false, isOnSurface = false;
	private Vector2 touchStartPosition;

	private Vector3 ballStartPos;
	private Vector3 currentCollisionNormal;
	private float horizontalMovement;
	private float turnSpeed = 5.0f;
	private float powerGauge = 100f;
	private int maxPower = 100;
	private int minPower = 0;
	
	private Rect jumpBtn;
	private Rect boostBtn;
	
	private float jumpVelocity = 2.3f;
	private float boostVelocity = 20;

	private float trust = 30.0f;



	// Use this for initialization
	void Start () {
		ballStartPos = transform.position;
		jumpBtn = new Rect (Screen.width - 150, Screen.height - 150, 100, 100);
		boostBtn = new Rect(Screen.width - 150, Screen.height - 300, 100, 100);


	}

	void Update()
	{
		AccelerometerControls();
		if (Input.GetKey(KeyCode.Escape)) {
			Application.LoadLevel(Application.loadedLevelName);
		}

		if (IsTouching()) {
			if (jumpBtn.Contains(ConvertToTopLeftOrigin(Input.GetTouch(0).position))) {
				isJumping = true;
			}
			if (boostBtn.Contains(ConvertToTopLeftOrigin(Input.GetTouch(0).position))) {
				isBoosting = true;
			}
		}
		//		TouchStick();
	}
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		
		Vector3 right = gameCamera.transform.right;
		right.y = 0;
		right.Normalize ();
		Vector3 cross = Vector3.Cross (currentCollisionNormal, right);
		Vector3 forceDir = Vector3.Cross (cross, currentCollisionNormal);
		rigidbody.AddForce (forceDir * horizontalMovement * turnSpeed);
		
		if(powerGauge < minPower)
			powerGauge = minPower;
		else if(powerGauge > maxPower)
			powerGauge = maxPower;
		
		if(isBoosting && powerGauge > minPower){
			Vector3 boostDir = transform.position - gameCamera.transform.position;
			boostDir.Normalize();
			rigidbody.AddForce(boostDir * boostVelocity);
			powerGauge -= 2f;
		}
		else
			powerGauge += 0.5f;
		isBoosting = false;
		//rigidbody.AddForce (-currentCollisionNormal * 5f);
		//		rigidbody.AddForce (gameCamera.transform.forward * 5);
		
		
		
		//		if (rigidbody.velocity.x < maxTurnSpeed && rigidbody.velocity.x > -maxTurnSpeed) {
		//				rigidbody.AddForce (new Vector3 (horizontal * turnSpeed, 0, -vertical * trust));
		//		}
		//			Test




		if (isJumping && isOnSurface) {
			rigidbody.AddForce(new Vector3(0, jumpVelocity, 0), ForceMode.Impulse);
			isJumping = false;
				}
		
//		if(!isOnSurface)
//			rigidbody.AddForce(0, -1, 0);

	}
	
	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 200, 100), "OnSurface: " + isOnSurface.ToString() + " Jump: " + isJumping.ToString());
		GUI.Label (new Rect (0, 20, 200, 100), "Power Gauge: " + powerGauge.ToString());
		GUI.Label (new Rect (0, 40, 200, 100), Input.acceleration.y.ToString());
		GUI.Button (jumpBtn, "Jumpuru");
		GUI.Button (boostBtn, "Boosturu");
		if (IsTouching()) {
			GUI.Label (new Rect (0, 60, 200, 100), ConvertToTopLeftOrigin(Input.GetTouch(0).position).ToString());
				}


	}
	
	void OnCollisionStay(Collision collisionInfo) {

		if (collisionInfo.contacts.Length > 0) {
			foreach (ContactPoint contact in collisionInfo.contacts) {
				currentCollisionNormal = contact.normal;
				isOnSurface = true;
			}
		}

		currentCollisionNormal.Normalize ();
	}

	void OnCollisionExit(Collision collisionInfo)
	{
		isOnSurface = false;
	}


	void AccelerometerControls()
	{
		float accAmountY = Mathf.Abs(Input.acceleration.x);
		
		if (accAmountY > tiltThreshold) {


			if (accAmountY > tiltThresholdMax) {
				if (Input.acceleration.x < 0) {
					//horizontalMovement = -tiltThresholdMax;
					horizontalMovement = -1.0f;
				}
				else {
					//horizontalMovement = tiltThresholdMax;
					horizontalMovement = 1.0f;
				}
				
				return;
			}
			horizontalMovement = Input.acceleration.x / tiltThresholdMax;
			
		}
	}
	
	void TouchStick ()
	{	



//		if (Input.touchCount > 0) {
//			if (Input.GetTouch (0).phase == TouchPhase.Began) {
//				touchStartPosition = Input.GetTouch (0).position;
//				isTouched = true;
//			}
//			if (Input.GetTouch (0).phase == TouchPhase.Ended) {
//				isTouched = false;
//			}
//			if (isTouched) {
//				float posX = Input.GetTouch (0).position.x;
//				if (posX != touchStartPosition.x) {
//					horizontalMovement = (posX - touchStartPosition.x) / 100;
//				}
//			}
//		}
	}

	bool IsTouching()
	{
		return Input.touchCount > 0;

	}

	Vector2 ConvertToTopLeftOrigin(Vector2 andPos)
	{
		return new Vector2(andPos.x, Screen.height - andPos.y);
	}
	
}
