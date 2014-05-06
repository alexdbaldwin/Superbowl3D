using UnityEngine;
using System.Collections;

public class ObstaclePlacementScript : MonoBehaviour {

	enum InputType {MouseControl, TouchControl}

	public string prefabName = "GeosphereTower";
	public Collider placementMesh;

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
					Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
					
					Vector3 curPosition = overviewCamera.camera.ScreenToWorldPoint(curScreenPoint) + offset;
					transform.position = startPos + Mathf.Clamp(Vector3.Dot (curPosition - startPos, transform.forward),-maxDrag, maxDrag) * transform.forward;
					SnapToTrack();
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

	public void SnapToTrack(){
		Ray ray = new Ray (transform.position + transform.up * 3.0f, -transform.up);
		RaycastHit[] hits =	Physics.RaycastAll (ray);
		foreach (RaycastHit hit in hits) {
			if (hit.collider.gameObject.tag == "TheLevel") {
				Vector3 hitPos = hit.point;
				float halfHeight = placementMesh.bounds.size.y / 2;
				float distanceToMove = Vector3.Distance (hitPos, transform.position);
				distanceToMove -= halfHeight;
				transform.Translate (new Vector3 (0, -distanceToMove, 0));

				//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler (hit.normal),10000.0f);
				transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
			}
		}
	}

	[RPC]
	void SendObjectOverNetwork()
	{
//		Object parent = UnityEditor.PrefabUtility.GetPrefabParent (this.gameObject);
//		path = UnityEditor.AssetDatabase.GetAssetPath (parent);

//		GameObject myNewObstace = (GameObject)Network.Instantiate(Resources.Load ("Prefabs/" + this.gameObject.name - "(clone)"), this.gameObject.transform.position, this.gameObject.transform.rotation, 0);
	}

	public void OnGUI()
	{

		GUI.Label (new Rect (0, 100, 100, 100), gameObject.name);
//		GameObject parent = UnityEditor.PrefabUtility.GetPrefabParent (this.gameObject);
//		UnityEditor.AssetDatabase.

	}
}
