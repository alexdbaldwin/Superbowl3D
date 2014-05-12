using UnityEngine;
using System.Collections;

public class ObstaclePlacementScript : MonoBehaviour {

	enum InputType {MouseControl, TouchControl}

	public string prefabName = "Bumper";

	bool rotating = false;
	bool dragging = false;
	Vector2 initialScreenPos;
	Vector2 centerScreenPos;
	Quaternion startRotation;
	InputType inputType;
//	Vector3 startPos;
	GameObject overviewCamera;
//	Vector3 offset;
//	Vector3 screenPoint;
//	float maxDrag;

	public delegate void RotateFinishedCallback();
	RotateFinishedCallback rotateCallback;

	bool OnLeftHandSide(Vector2 start, Vector2 end, Vector2 p){
		
		return Mathf.Sign ((end.x - start.x) * (p.y - start.y) - (end.y - start.y) * (p.x - start.x)) == 1; 
		
	}

	void Start () {
		overviewCamera = GameObject.FindGameObjectWithTag ("OverviewCamera");
	}

	void Update () {
	
		if (rotating) {
				
			if(dragging){
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
					Place();
					return;
				} else if (Input.GetMouseButtonUp (0)) {
					Place();
					return;
				}

				Vector2 currentScreenPos;

				switch(inputType){
				case InputType.MouseControl:
					currentScreenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					break;
				case InputType.TouchControl:
					currentScreenPos = Input.GetTouch(0).position;
					break;
				default:
					currentScreenPos = Vector2.zero;
					break;
				}

				//rotate the object!
				transform.rotation = startRotation;
				float angle = Mathf.Rad2Deg*(Mathf.Acos(Vector2.Dot ((currentScreenPos-centerScreenPos).normalized,(initialScreenPos-centerScreenPos).normalized)));
				if(OnLeftHandSide(centerScreenPos,initialScreenPos,currentScreenPos)){
					angle = 360 - angle;
				}
				if(float.IsNaN(angle)){
					angle = 0;
				}
				transform.RotateAround(transform.position, transform.up,angle);

			} else {

				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
					if(TouchDown (Input.GetTouch(0).position)){
						inputType = InputType.TouchControl;
					}
				} else if (Input.GetMouseButtonDown (0)) {
					if(TouchDown (Input.mousePosition)){
						inputType = InputType.MouseControl;
					}
				}

			}
		}

	}

	bool TouchDown(Vector2 position){
//		Ray ray = overviewCamera.camera.ScreenPointToRay (position);
//		RaycastHit hit;
//		if (Physics.Raycast (ray, out hit, 1000, 1 << 0)) {
//			if (hit.collider.gameObject == gameObject) {
				dragging = true;
				initialScreenPos = position;
				centerScreenPos = overviewCamera.camera.WorldToScreenPoint(transform.position);
				return true;
//			}
//		}
//		return false;
	
	}
	

	public void StartRotate(RotateFinishedCallback callback){
		rotateCallback = callback;
		dragging = false;
		rotating = true;
		startRotation = transform.rotation;
	}
	

	void Place(){
		dragging = false;
		rotating = false;
		if(rotateCallback != null) rotateCallback ();
	}

}
