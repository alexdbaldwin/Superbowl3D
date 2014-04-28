using UnityEngine;
using System.Collections;

public class ObstaclePlacementScript : MonoBehaviour {

	enum InputType {MouseControl, TouchControl}

	bool placementMode = false;
	bool dragging = false;
	Vector2 initialScreenPos;
	InputType inputType;
	Vector3 startPos;
	GameObject overviewCamera;
	Vector3 offset;
	Vector3 screenPoint;
	float maxDrag;


	void Start () {
		overviewCamera = GameObject.FindGameObjectWithTag ("OverviewCamera");
	}

	void Update () {
	
		if (placementMode) {
				
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

				if(Vector2.Distance(currentScreenPos, initialScreenPos) > 1000){
//					dragging = false;
//					transform.position = startPos;
//					return;
				} else {

//					float scaleFactor = 1.0f;
//					Vector3 screenDir = -(overviewCamera.camera.WorldToScreenPoint(Vector3.zero) - overviewCamera.camera.WorldToScreenPoint(transform.forward));
//					scaleFactor = screenDir.magnitude;
//					screenDir.Normalize();
//					Debug.Log(screenDir.ToString());
//					float movement = Vector2.Dot(currentScreenPos - initialScreenPos, new Vector2(screenDir.x,screenDir.y));
//					Debug.Log ("movement " + movement.ToString());
//					float worldMovement = movement / scaleFactor;
//					transform.position = startPos + transform.forward * worldMovement;

					Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
					
					Vector3 curPosition = overviewCamera.camera.ScreenToWorldPoint(curScreenPoint) + offset;
					transform.position = startPos + Mathf.Clamp(Vector3.Dot (curPosition - startPos, transform.forward),-maxDrag, maxDrag) * transform.forward;
				}

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
		Ray ray = overviewCamera.camera.ScreenPointToRay (position);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 1000, 1 << 0)) {
			if (hit.collider.gameObject == gameObject) {
				dragging = true;
				initialScreenPos = position;
				startPos = transform.position;

				screenPoint = overviewCamera.camera.WorldToScreenPoint(transform.position);
				offset = gameObject.transform.position - overviewCamera.camera.ScreenToWorldPoint(new Vector3(position.x, position.y, screenPoint.z));
				return true;
			}
		}
		return false;
	
	}

	public void SetPlacementMode(bool b){
		placementMode = b;
	}

	public void SetMaxDrag(float drag){
		maxDrag = drag;
		Debug.Log (maxDrag);
	}

	void Place(){
		dragging = false;
		placementMode = false;
	}
}
