using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
	public GameObject GUICamera;
	public GameObject Ball;
	public GameObject boostBar = null;
	public GameObject steeringCircle = null;
	public GameObject steeringArrows = null;
	public GameObject jumpArrow = null;
	
	bool steering = false;
	bool jumpDown = false;
	bool jump = false;
	bool boosting = false;
	
	int steeringTouch = 0;
	int jumpTouch = 0;
	float maxSteer = 0.8f;
	float maxScreenSteer;
	float currentScreenSteer;

	float boostAmount;
	float boostMin;
	float boostMax;

	Vector2 steeringTouchStart;
	Vector2 jumpTouchStart;
	Vector3 steeringCircleStart;
	Vector3 jumpArrowStart;
	
	// Use this for initialization
	void Start () {
		maxScreenSteer = Screen.width / 20.0f;
		boostMax = Screen.height / 4.0f;
		boostMin = Screen.height / 40.0f;
		boostAmount = 0.0f;
		steeringCircleStart = steeringCircle.transform.position;
		jumpArrowStart = jumpArrow.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		//Steering controls
		if (!steering) {
			for (int i = 0; i < Input.touchCount; i++) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					Ray ray = GUICamera.GetComponent<Camera>().ScreenPointToRay (Input.GetTouch (i).position);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit)) {
						if (hit.collider.gameObject.name == "SteeringCircle") {
							steering = true;
							steeringTouchStart = Input.GetTouch (i).position;
							steeringTouch = Input.GetTouch (i).fingerId;
							steeringArrows.GetComponent<SpriteRenderer>().enabled = false;
						} 
					}
				}
			}		
		} else {
			//Find the touch with the right fingerId
			int touchIndex = -1;
			for (int i = 0; i < Input.touchCount; i++) {
				if(Input.GetTouch(i).fingerId == steeringTouch){
					touchIndex = i;
					break;
				}
			}
			if(touchIndex != -1 && (Input.GetTouch(touchIndex).phase == TouchPhase.Ended || Input.GetTouch(touchIndex).phase == TouchPhase.Canceled)){
				steering = false;
				steeringCircle.transform.position = steeringCircleStart;
				steeringArrows.GetComponent<SpriteRenderer>().enabled = true;
			} else {
				currentScreenSteer = Mathf.Clamp(Input.GetTouch(touchIndex).position.x - steeringTouchStart.x, -maxScreenSteer, maxScreenSteer);
				if(steeringCircle != null){
					steeringCircle.transform.position = steeringCircleStart + steeringCircle.transform.right * currentScreenSteer / maxScreenSteer * maxSteer;
				}
			}
		}

		//Jumping/Boosting controls
		if (!jumpDown) {
			for (int i = 0; i < Input.touchCount; i++) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					Ray ray = GUICamera.GetComponent<Camera>().ScreenPointToRay (Input.GetTouch (i).position);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit)) {
						if (hit.collider.gameObject.name == "JumpArrow") {
							jumpDown = true;
							jumpTouchStart = Input.GetTouch (i).position;
							jumpTouch = Input.GetTouch(i).fingerId;
						}
					}
				}
			}		
		} else {
			//Find the touch with the right FingerId
			int touchIndex = -1;
			for (int i = 0; i < Input.touchCount; i++) {
				if(Input.GetTouch(i).fingerId == jumpTouch){
					touchIndex = i;
					//break;
				}
			}
			if (touchIndex != -1){
				if(Input.GetTouch (touchIndex).phase == TouchPhase.Ended || Input.GetTouch(touchIndex).phase == TouchPhase.Canceled) {
					if(!boosting){
						jump = true;
					}
					jumpDown = false;
					jumpArrow.transform.position = jumpArrowStart;
					boosting = false;
				} else {
					boostAmount = Mathf.Clamp(Input.GetTouch(touchIndex).position.y - jumpTouchStart.y - boostMin, 0, boostMax);
					if (boostAmount > 0){
						boosting = true;
					}
					if(boosting){
						jumpArrow.transform.position = jumpArrowStart + jumpArrow.transform.up * boostAmount / boostMax * 2.0f;

					}
				}
			}

		}

		//ControlScript.GetComponent<AndroidControlScript> ().GetPowerGauge (); 
		//restet 
		if (Input.touchCount == 0) {
			steering = false;
			jumpDown = false;
			boosting = false;
			jumpTouch = 0;
			steeringTouch = 0;
		}

		//		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
		//			TouchDown(Input.GetTouch(0).position);
		//		} 

		//Power Gauge
		boostBar.transform.localScale = new Vector3(1, 1 * Ball.GetComponent<AndroidControlScript> ().GetPowerGauge (), 1);
	}
	public bool GetJump()
	{
		if (jump) {
			jump = false;
			return true;
		} else {
			return false;
		}
	}
	public float GetSteering()
	{
		if (steering) {
			return currentScreenSteer/ maxScreenSteer;
		} else {
			return 0;
		}
	}
	public float GetBoost(){
		if (boosting) {
			return boostAmount / boostMax;
		} else {
			return 0.0f;		
		}
	}



	void OnGUI()
	{
		GUI.Label (new Rect (0, 80, 200, 100), "Boosting : " + boosting.ToString());
		GUI.Label (new Rect (0, 100, 200, 100), "Jump Down : " + jumpDown.ToString());
		GUI.Label (new Rect (0, 120, 200, 100), "Jump touch (FingerId) : " + jumpTouch.ToString());

		//		GUI.Button (jumpBtn, "Jumpuru");
		//		GUI.Button (boostBtn, "Boosturu");
		//		if (IsTouching()) {
		//			GUI.Label (new Rect (0, 60, 200, 100), ConvertToTopLeftOrigin(Input.GetTouch(0).position).ToString());
		//		}
		
		
	}
}
