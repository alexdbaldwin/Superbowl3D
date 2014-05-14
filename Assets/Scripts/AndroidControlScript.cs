using UnityEngine;
using System.Collections;

public class AndroidControlScript : MonoBehaviour {
	public GameObject gameCamera;
	public GameObject GUIManager;
	public AudioSource boostEffect;
	public AudioSource jumpEffect;
	private GameObject gameManager;
	private GameObject boostTrail;

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
	private float maxPower = 100;
	private float minPower = 0;
	
	private Rect jumpBtn;
	private Rect boostBtn;
	
	private float jumpVelocity = 3.8f;
	private float boostVelocity = 20;
	
	private float boostModifier = 0;
	private float boostModifierMax = 1f;

	private float trust = 30.0f;

	private float boostEffectModifier = 0.05f;

	private bool tiltControls = false;
	private bool lockedControls = false;



	// Use this for initialization
	void Start () {
		gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
		GUIManager = GameObject.FindGameObjectWithTag("GUIManager");
		gameManager = GameObject.FindGameObjectWithTag("GameManager");
		boostTrail = GameObject.FindGameObjectWithTag("BoostTrail");

		ballStartPos = transform.position;
		jumpBtn = new Rect (Screen.width - 150, Screen.height - 150, 100, 100);
		boostBtn = new Rect(Screen.width - 150, Screen.height - 300, 100, 100);
		defaultBodyDrag = rigidbody.drag;
		tiltControls = PlayerPrefs.GetInt ("Tilt") == 1 ? true : false;
		LockControls ();
		rigidbody.Sleep ();
	}

	void Update()
	{
		if(gameManager == null)
			gameManager = GameObject.FindGameObjectWithTag("GameManager");
	
	
		if (lockedControls || !gameManager.GetComponent<GameManager>().IsBall())
			return;

		if (tiltControls) {
			AccelerometerControls ();
		} else {
			TouchControls();	
		}


		isBoosting = GUIManager.GetComponent<GUIScript> ().GetBoost () > 0 ? true : false;
		if (Application.isEditor || Application.isWebPlayer) {
						isBoosting = Input.GetKey (KeyCode.UpArrow);
				}
		isJumping = GUIManager.GetComponent<GUIScript> ().GetJump ();

		if (isBoosting && powerGauge > minPower) {
			boostTrail.GetComponent<BoostTrailScript> ().Show ();	
			GetComponent<MeshRenderer>().materials[1].color = Color.Lerp(new Color(89.0f / 256.0f, 30.0f / 256.0f, 150.0f / 256.0f), Color.white, powerGauge/maxPower);
		} else {
			boostTrail.GetComponent<BoostTrailScript> ().Hide();
			GetComponent<MeshRenderer>().materials[1].color = new Color(89.0f / 256.0f, 30.0f / 256.0f, 150.0f / 256.0f);
		}
	}


	void FixedUpdate () 
	{
		if(gameManager == null)
			gameManager = GameObject.FindGameObjectWithTag("GameManager");
	
	
		if (lockedControls || !gameManager.GetComponent<GameManager>().IsBall())
			return;
			
		

		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsWebPlayer) {
			horizontalMovement = Input.GetAxis ("Horizontal");
				}

		if(gameCamera == null)
			gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
		
		Vector3 right = gameCamera.transform.right;
		right.y = 0;
		right.Normalize ();
		Vector3 cross = Vector3.Cross (currentCollisionNormal, right);
		Vector3 forceDir = Vector3.Cross (cross, currentCollisionNormal);
		rigidbody.AddForce (forceDir * horizontalMovement * turnSpeed * (isOnSurface ? 1.0f : 0.4f), ForceMode.VelocityChange);
		

		
		if(isBoosting && powerGauge > minPower){
			if(!audio.isPlaying)
			{
				boostEffect.Play();
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
			boostEffect.Stop();
			boostEffect.pitch = 1;
			powerGauge += 0.5f;
			boostModifier = 0;
			if(powerGauge > maxPower)
				powerGauge = maxPower;
		}
		//isBoosting = false;


		if ((isJumping || Input.GetKey(KeyCode.Space)) && isOnSurface) {
			rigidbody.AddForce(new Vector3(0, jumpVelocity, 0), ForceMode.Impulse);
			isJumping = false;
			jumpEffect.Play();
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
		if (collisionInfo.gameObject.tag == "TheLevel" && gameManager.GetComponent<GameManager>().IsBall())
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

	public void LockControls(){
		lockedControls = true;
	}

	public void UnlockControls(){
		lockedControls = false;
	}
}
