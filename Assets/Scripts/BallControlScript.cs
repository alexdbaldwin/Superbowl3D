using UnityEngine;
using System.Collections;

public class BallControlScript : MonoBehaviour {
	public GameObject gameCamera;
	private float turnSpeed = 50f;
	float trust = 30.0f;
	//Kontrollprylar
	private float tiltThreshold = 0.5f;
	private float tiltThresholdMax = 0.5f;
	private bool jump, isOnSurface = false;
	private float horizontalMovement;
    bool isTouched = false;
	private Vector2 touchStartPosition;
	private bool slowDown;
	
	private float maxTurnSpeed = 25f; 
	private float velX = 0.0f;
	private Vector3 currentCollisionNormal;
	// Use this for initialization
	void Start () {
	
	}
	
	void Update()
	{
		AccelerometerControls();
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

		rigidbody.AddForce (forceDir * horizontal * turnSpeed);
		//rigidbody.AddForce (-currentCollisionNormal * 5f);
//		rigidbody.AddForce (gameCamera.transform.forward * 5);
		


//		if (rigidbody.velocity.x < maxTurnSpeed && rigidbody.velocity.x > -maxTurnSpeed) {
//				rigidbody.AddForce (new Vector3 (horizontal * turnSpeed, 0, -vertical * trust));
//		}
//			Test



	}

	public void SlowDown()
	{
		rigidbody.drag = 100;
		StartCoroutine ("speedUp");
	}

	IEnumerator speedUp()
	{
		yield return new WaitForSeconds (5.0f);
		rigidbody.drag = 0.5f;
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 100, 100), Input.acceleration.y.ToString());
		GUI.Label (new Rect (0, 20, 100, 100), "Fan fdsafesfesig");
	}

	void OnCollisionStay(Collision collisionInfo) {

		foreach (ContactPoint contact in collisionInfo.contacts) {
			currentCollisionNormal = contact.normal;
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
		
}
