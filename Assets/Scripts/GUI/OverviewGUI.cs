using UnityEngine;
using System.Collections;

public class OverviewGUI : MonoBehaviour {
	public GameObject overviewCamera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		
//		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
//			Click(Input.GetTouch(0).position);
//		} else if (Input.GetMouseButtonUp (0)) {
//			Click(Input.mousePosition);
//		}	
//	}
//	
//	void Click(Vector2 position)
//	{
//		Ray ray = gameObject.camera.ScreenPointToRay (position);
//		RaycastHit hit;
//		if (Physics.Raycast(ray, out hit, 1000, 1 << 11)) {
//			if (hit.collider.gameObject.name == "BackArrow") {
//				overviewCamera.GetComponent<OverviewCameraScript>().GoBackToStart();	
//			}
//		}
	}
}
