using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
	public GameObject GUICamera;
	public GameObject steeringCircle = null;
	public GameObject steeringArrows = null;
	
	bool steering = false;
	bool jumpDown = false;
	bool jump = false;
	
	int steeringTouch = 0;
	int jumpTouch = 0;
	float maxSteer = 0.825f;
	float maxScreenSteer;
	float currentScreenSteer;
	
	Vector2 steeringTouchStart;
	Vector3 steeringCircleStart;
	
	// Use this for initialization
	void Start () {
		maxScreenSteer = Screen.width / 20.0f;
		steeringCircleStart = steeringCircle.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!steering) {
			for (int i = 0; i < Input.touchCount; i++) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					TouchDown (Input.GetTouch (i).position);
					if (steering) {
						steeringTouch = i;
						steeringArrows.GetComponent<SpriteRenderer>().enabled = false;
					}
				}
			}		
		} else {
			if(Input.GetTouch(steeringTouch).phase == TouchPhase.Ended || Input.GetTouch(steeringTouch).phase == TouchPhase.Canceled){
				steering = false;
				steeringCircle.transform.position = steeringCircleStart;
				steeringArrows.GetComponent<SpriteRenderer>().enabled = true;
			} else {
				currentScreenSteer = Mathf.Clamp(Input.GetTouch(steeringTouch).position.x - steeringTouchStart.x, -maxScreenSteer, maxScreenSteer);
				if(steeringCircle != null){
					steeringCircle.transform.position = steeringCircleStart + steeringCircle.transform.right * currentScreenSteer / maxScreenSteer * maxSteer;
				}
			}
		}
		
		if (!jumpDown) {
			for (int i = 0; i < Input.touchCount; i++) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					TouchDown (Input.GetTouch (i).position);
					if (jumpDown) {
						jumpTouch = i;
					}
				}
			}		
		} else {
			if (Input.GetTouch (jumpTouch).phase == TouchPhase.Ended || Input.GetTouch(jumpTouch).phase == TouchPhase.Canceled) {
				jump = true;
			}
		}
		
		//		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
		//			TouchDown(Input.GetTouch(0).position);
		//		} 
	}
	bool GetJump()
	{
		if (jump) {
			jump = false;
			return true;
		} else {
			return false;
		}
	}
	float GetSteering()
	{
		if (steering) {
			return currentScreenSteer/ maxScreenSteer;
		} else {
			return 0;
		}
	}
	void TouchDown(Vector2 position)
	{
		Ray ray = GUICamera.GetComponent<Camera>().ScreenPointToRay (position);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject.name == "SteeringCircle") {
				steering = true;
				steeringTouchStart = position;
			} else if (hit.collider.gameObject.name == "JumpArrow") {
				jumpDown = true;
				
			}
		}
	}
}
