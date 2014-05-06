using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleMenu : MonoBehaviour {

	public List<GameObject> obstacles = new List<GameObject> ();
	private GameObject overviewGUICamera;
	private bool menuActive = false;
	private Vector2 centerScreenPos;
	private float menuButtonOffset = 2.0f;
	private List<GameObject> menuButtons = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		overviewGUICamera = GameObject.FindGameObjectWithTag ("OverviewGUICamera");
	}
	
	// Update is called once per frame
	void Update () {
		if (menuActive) {
			//Check if a menu item has been clicked
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				MenuClick(Input.GetTouch(0).position);
			} else if (Input.GetMouseButtonDown (0)) {
				MenuClick(Input.mousePosition);
			}
		}
	}

	private void MenuClick(Vector2 position){
		Ray ray = overviewGUICamera.camera.ScreenPointToRay (position);
	    RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 1000, 1 << 11 /*Layer mask 11*/)) {
			if (hit.collider.gameObject.tag == "RadialMenuButton") {
				//Debug.Log (hit.collider.gameObject.ToString());
				GameObject newObstacle;
				if (Network.isClient || Network.isServer) {
					newObstacle = (GameObject)Network.Instantiate (Resources.Load("Prefabs/Placeables/"+hit.collider.gameObject.GetComponent<ObstaclePlacementScript>().prefabName), transform.position, transform.rotation, 0);
				} else {
					newObstacle = (GameObject)Instantiate(Resources.Load("Prefabs/Placeables/"+hit.collider.gameObject.GetComponent<ObstaclePlacementScript>().prefabName));
				}

				newObstacle.transform.position = transform.position;
				newObstacle.transform.rotation = transform.rotation;
				//newObstacle.transform.localScale = Vector3.one;
				newObstacle.layer = 0;
				newObstacle.tag = "Obstacle";
				newObstacle.GetComponent<ObstaclePlacementScript>().SetPlacementMode(true);
				newObstacle.GetComponent<ObstaclePlacementScript>().SnapToTrack();
				newObstacle.GetComponent<ObstaclePlacementScript>().SetMaxDrag(GetComponentsInChildren<Transform>()[1].localPosition.z);
				StopRadialMenu ();
				Destroy(gameObject);
			}
		} 
		//else {
//			//Find out how far the menu buttons are from the center of the menu in screen coordinates
//			float screenOffset = Vector3.Distance(overviewGUICamera.camera.WorldToScreenPoint(Vector3.zero) - overviewGUICamera.camera.WorldToScreenPoint(Vector3.up));
//			Debug.Log (screenOffset.ToString());
//			if(Vector2.Distance(position, centerScreenPos) > screenOffset*1.2f)
//		}
			StopRadialMenu ();
	}

	public void StartRadialMenu(Vector2 position){
		menuActive = true;
		centerScreenPos = position;
		//Vector3 centerPos = overviewGUICamera.transform.position + overviewGUICamera.transform.forward * 5;
		//centerPos.x += overviewGUICamera.camera.rect.width / 2.0f;
		Vector2 offset = new Vector2(0.0f,menuButtonOffset);

		Vector3 centerPos = overviewGUICamera.camera.ScreenToWorldPoint (new Vector3 (position.x, position.y, overviewGUICamera.camera.nearClipPlane)) + overviewGUICamera.transform.forward * 5;

		for (int i = 0; i < obstacles.Count; i++) {

			float angle = i* Mathf.PI * 2.0f / obstacles.Count;

			float x = ((offset.x) * Mathf.Cos(angle)) - (( - offset.y) * Mathf.Sin(angle));
			float y = ((-offset.y) * Mathf.Cos(angle)) - ((offset.x) * Mathf.Sin(angle));
			Vector3 newOffset = new Vector3(x,y,0);
			newOffset = overviewGUICamera.transform.InverseTransformDirection(newOffset);


			GameObject go = (GameObject)Instantiate (obstacles[i]);


			//Disable all colliders so we can add a new collider for clicking on the menu button
			Collider coll = go.GetComponent<CapsuleCollider>();
			if(coll != null)
			{
				coll.enabled = false;
			}
			coll = go.GetComponent<SphereCollider>();
			if(coll != null)
			{
				coll.enabled = false;
			}
			coll = go.GetComponent<BoxCollider>();
			if(coll != null)
			{
				coll.enabled = false;
			}
			coll = go.GetComponent<MeshCollider>();
			if(coll != null)
			{
				coll.enabled = false;
			}



			SphereCollider newColl = go.AddComponent<SphereCollider>();

			go.transform.position = centerPos - newColl.center * go.GetComponent<PlaceableParameters>().scale.x + newOffset;

			go.transform.localScale = go.GetComponent<PlaceableParameters>().scale;
			go.transform.rotation = Quaternion.Euler(go.GetComponent<PlaceableParameters>().rotation);
			go.layer = 11;
			go.tag = "RadialMenuButton";

			Transform[] children = go.GetComponentsInChildren<Transform>();
			foreach(Transform child in children){
				child.gameObject.layer = 11;
				child.gameObject.tag = "RadialMenuButton";
			}



			menuButtons.Add(go);

		}

	}

	public void StopRadialMenu(){
		if (menuActive) {
				
			menuActive = false;
			foreach(GameObject go in menuButtons){
				Destroy (go);
			}
			menuButtons.Clear();
		}

	}


}
