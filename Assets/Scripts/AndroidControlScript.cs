using UnityEngine;
using System.Collections;

public class AndroidControlScript : MonoBehaviour {
	public GameObject gameCamera;
	public GameObject GUIManager;
	public AudioSource boostEffect;

	public float defaultBodyDrag;
	private float rigidbodyDrag;
	public bool slowed = false;
	//Debug output
	//Kontrollprylar
	private float tiltThreshold = 0.1f;
	private float tiltThresholdMax = 0.6f;
	private bool isJumping, isBoosting = false, isOnSurface = false;
	private Vector2 touchStartPosition;

	private Vector3 ballStartPos;
	private Vector3 lastActiveNodePos;
	private Vector3 currentCollisionNormal;
	private float horizontalMovement;
	private float turnSpeed = 0.25f;
	private float powerGauge = 100f;
	private int maxPower = 100;
	private int minPower = 0;
	
	private Rect jumpBtn;
	private Rect boostBtn;
	
	private float jumpVelocity = 3.8f;
	private float boostVelocity = 20;
	
	private float boostModifier = 0;
	private float boostModifierMax = 1f;

	private float trust = 30.0f;

	private float boostEffectModifier = 0.05f;

	private bool tiltControls = false;



	// Use this for initialization
	void Start () {
		ballStartPos = transform.position;
		jumpBtn = new Rect (Screen.width - 150, Screen.height - 150, 100, 100);
		boostBtn = new Rect(Screen.width - 150, Screen.height - 300, 100, 100);
		defaultBodyDrag = rigidbody.drag;
		tiltControls = PlayerPrefs.GetInt ("Tilt") == 1 ? true : false;

	}

	void Update()
	{

		if (tiltControls) {
			AccelerometerControls ();
		} else {
			TouchControls();	
		}
		if (Input.GetKey(KeyCode.Escape)) {
			Application.LoadLevel(Application.loadedLevelName);
		}

//		if (IsTouching()) {
////			if (jumpBtn.Contains(ConvertToTopLeftOrigin(Input.GetTouch(0).position))) {
////				isJumping = true;
////			}
////			if (boostBtn.Contains(ConvertToTopLeftOrigin(Input.GetTouch(0).position))) {
////				isBoosting = true;
////			}
//		}
		isBoosting = GUIManager.GetComponent<GUIScript> ().GetBoost () > 0 ? true : false;
		isJumping = GUIManager.GetComponent<GUIScript> ().GetJump ();
		//		TouchStick();
	}
	// Update is called once per frame
	void FixedUpdate () 
	{
		//comment this out for android
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			horizontalMovement = Input.GetAxis ("Horizontal");
				}
		//float horizontalMovement = Input.GetAxis ("Horizontal");

		
		Vector3 right = gameCamera.transform.right;
		right.y = 0;
		right.Normalize ();
		Vector3 cross = Vector3.Cross (currentCollisionNormal, right);
		Vector3 forceDir = Vector3.Cross (cross, currentCollisionNormal);
		rigidbody.AddForce (forceDir * horizontalMovement * turnSpeed, ForceMode.VelocityChange);
		

		
		if(isBoosting && powerGauge > minPower){
			if(!audio.isPlaying)
			{
				audio.Play();
			}
			Vector3 boostDir = transform.position - gameCamera.transform.position;
			boostDir.Normalize();
			boostEffect.pitch += boostEffectModifier;
			rigidbody.AddForce(boostDir * (boostVelocity * boostModifier));
			powerGauge -= 1f;
			boostModifier = Mathf.Min(boostModifier + 0.02f, boostModifierMax);
			if(powerGauge < minPower)
				powerGauge = minPower;
		}
		else
		{
			audio.Stop();
			audio.pitch = 1;
			powerGauge += 0.5f;
			boostModifier = 0;
			if(powerGauge > maxPower)
				powerGauge = maxPower;
		}
		//isBoosting = false;


		if (isJumping && isOnSurface) {
			rigidbody.AddForce(new Vector3(0, jumpVelocity, 0), ForceMode.Impulse);
			isJumping = false;
		}


	}

	public void SlowDown()
	{
		if (!slowed) {
			rigidbodyDrag = rigidbody.drag;
						rigidbody.drag = 2;
						StartCoroutine ("speedUp");
				}
	}
	
	IEnumerator speedUp()
	{
		yield return new WaitForSeconds (2.0f);
		rigidbody.drag = defaultBodyDrag;

		slowed = false;
	}

	void OnGUI()
	{
//		GUI.Label (new Rect (0, 0, 200, 100), "OnSurface: " + isOnSurface.ToString() + " Jump: " + isJumping.ToString());
//		GUI.Label (new Rect (0, 20, 200, 100), "Power Gauge: " + powerGauge.ToString());
//		GUI.Label (new Rect (0, 40, 200, 100), "Boost modifier : " + boostModifier.ToString());
//		GUI.Label (new Rect (0, 60, 200, 100), "Touch count : " + Input.touchCount);
//		GUI.Button (jumpBtn, "Jumpuru");
//		GUI.Button (boostBtn, "Boosturu");
//		if (IsTouching()) {
//			GUI.Label (new Rect (0, 60, 200, 100), ConvertToTopLeftOrigin(Input.GetTouch(0).position).ToString());
//		}


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
		if (collisionInfo.gameObject.tag == "TheLevel")
						lastActiveNodePos = gameCamera.GetComponent<CameraPositioningScript> ().GetCurrentNodePosition();
	}

	public Vector3 GetLastActiveNodePos()
	{
		return lastActiveNodePos;
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

	void TouchControls(){

		horizontalMovement = GUIManager.GetComponent<GUIScript> ().GetSteering ();

	}


//	bool IsTouching()
//	{
//		return Input.touchCount > 0;
//
//	}

	Vector2 ConvertToTopLeftOrigin(Vector2 andPos)
	{
		return new Vector2(andPos.x, Screen.height - andPos.y);
	}
	public float GetPowerGauge()
	{
		return (powerGauge / 100);
	}

	public GameObject GetGameCamera()
	{
		return gameCamera;
	}
}
