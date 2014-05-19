using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceableMenu : MonoBehaviour {
	
	public List<GameObject> placeables = new List<GameObject> ();
	public GameObject overviewGUICamera;
	private bool menuActive = false;
	private Vector2 centerScreenPos;
	private float menuButtonOffset = 2.0f;
	private List<GameObject> menuButtons = new List<GameObject> ();
	Vector3 spawnPosition;
	Vector3 spawnNormal;
	bool blocked = false;
	public Material disabledButtonMaterial;
	private GameObject gameManager;
	bool clickable = false;

	void Start(){
		gameManager = GameObject.FindGameObjectWithTag ("GameManager");
	}
	
	void Update () {
		if (menuActive) {
			//Check if a menu item has been clicked
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
				MenuClick(Input.GetTouch(0).position);
			} else if (Input.GetMouseButtonUp (0)) {
				MenuClick(Input.mousePosition);
			}
		}
	}

	private void MenuClick(Vector2 position){
		if (clickable) {
				Ray ray = overviewGUICamera.camera.ScreenPointToRay (position);
				RaycastHit[] hits = Physics.RaycastAll (ray, 1000, 1 << 11 /*Layer mask 11*/);
				foreach (RaycastHit hit in hits) {
						if (hit.collider.gameObject.tag == "RadialMenuButton") {

								if (hit.collider.gameObject.GetComponent<PlaceableParameters> ().cost > gameManager.GetComponent<GameManager> ().GetPoints ()) {
										break;
								}


								GameObject newObstacle;
								if (Network.isClient || Network.isServer) {
										newObstacle = (GameObject)Network.Instantiate (Resources.Load ("Prefabs/Placeables/" + hit.collider.gameObject.GetComponent<ObstaclePlacementScript> ().prefabName), transform.position, transform.rotation, 0);
								} else {
										newObstacle = (GameObject)Instantiate (Resources.Load ("Prefabs/Placeables/" + hit.collider.gameObject.GetComponent<ObstaclePlacementScript> ().prefabName));
								}

								gameManager.GetComponent<GameManager> ().RemovePoints (newObstacle.GetComponent<PlaceableParameters> ().cost);

								newObstacle.transform.position = spawnPosition;
								newObstacle.transform.rotation = Quaternion.FromToRotation (Vector3.up, spawnNormal);
								if (newObstacle.GetComponent<PlaceableParameters> ().rotateAfterPlacement) {
										newObstacle.GetComponent<ObstaclePlacementScript> ().StartRotate (Unblock);
										blocked = true;
								}
								//newObstacle.transform.localScale = Vector3.one;
								newObstacle.layer = 0;
								newObstacle.tag = "Obstacle";
						}
				}
		}
		StopRadialMenu ();
	}

	public void Unblock(){
		blocked = false;
	}
	
	public void StartRadialMenu(Vector2 position, Vector3 spawnPosition, Vector3 spawnNormal){

		Vector2 clickPos = position;

		if (blocked)
			return;

		menuActive = true;
		centerScreenPos = position;
		this.spawnPosition = spawnPosition;
		this.spawnNormal = spawnNormal;

		Vector2 offset = new Vector2(0.0f,menuButtonOffset);

		float scaleFactor = Vector3.Magnitude(overviewGUICamera.camera.WorldToScreenPoint (Vector3.right) - overviewGUICamera.camera.WorldToScreenPoint (Vector3.zero));
		


		if (clickPos.x + menuButtonOffset * scaleFactor > Screen.width) {
			clickPos -= new Vector2(clickPos.x + menuButtonOffset * scaleFactor - Screen.width,0);
		}
		if (clickPos.x - menuButtonOffset * scaleFactor < 0) {
			clickPos += new Vector2(-clickPos.x + menuButtonOffset * scaleFactor,0);
		}
		if (clickPos.y + menuButtonOffset * scaleFactor > Screen.height) {
			clickPos -= new Vector2(0,clickPos.y + menuButtonOffset * scaleFactor - Screen.height);
		}
		if (clickPos.y - menuButtonOffset * scaleFactor < 0) {
			clickPos += new Vector2(0,-clickPos.y + menuButtonOffset * scaleFactor);
		}


		Vector3 centerPos = overviewGUICamera.camera.ScreenToWorldPoint (new Vector3 (clickPos.x, clickPos.y, overviewGUICamera.camera.nearClipPlane)) + overviewGUICamera.transform.forward * 5;
		Vector3 trueCenterPos = overviewGUICamera.camera.ScreenToWorldPoint (new Vector3 (position.x, position.y, overviewGUICamera.camera.nearClipPlane)) + overviewGUICamera.transform.forward * 5;
		for (int i = 0; i < placeables.Count; i++) {
			
			float angle = i* Mathf.PI * 2.0f / placeables.Count;
			
			float x = ((offset.x) * Mathf.Cos(angle)) - (( - offset.y) * Mathf.Sin(angle));
			float y = ((-offset.y) * Mathf.Cos(angle)) - ((offset.x) * Mathf.Sin(angle));
			Vector3 newOffset = new Vector3(x,y,0);
			newOffset = overviewGUICamera.transform.InverseTransformDirection(newOffset);
			
			
			GameObject go = (GameObject)Instantiate (placeables[i]);

			if(go.GetComponent<PlaceableParameters>().cost > gameManager.GetComponent<GameManager>().GetPoints()){
				MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
				foreach(MeshRenderer mr in renderers){
					mr.material = disabledButtonMaterial;
				}
			}
			
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
			
			//go.transform.position = centerPos - newColl.center * go.GetComponent<PlaceableParameters>().scale.x + newOffset;
			
			//go.transform.localScale = go.GetComponent<PlaceableParameters>().scale;
			go.transform.rotation = Quaternion.Euler(go.GetComponent<PlaceableParameters>().rotation);
			StartCoroutine(ExpandMenu (go, trueCenterPos, centerPos - newColl.center * go.GetComponent<PlaceableParameters>().scale.x + newOffset, go.GetComponent<PlaceableParameters>().scale));

			go.layer = 11;
			go.tag = "RadialMenuButton";

			//Makes sure all the children have the right tag/layer so they'll show up on the camera
			Transform[] children = go.GetComponentsInChildren<Transform>();
			foreach(Transform child in children){
				child.gameObject.layer = 11;
				//child.gameObject.tag = "RadialMenuButton";
			}

			go.GetComponent<PlaceableParameters>().lockUpdate = true;
	
			menuButtons.Add(go);
			clickable = false;
			Invoke ("MakeClickable", 0.1f);

			
		}
		
	}

	void MakeClickable(){
		clickable = true;
	
	}

	
	public void StopRadialMenu(){
		if (menuActive) {

			CancelInvoke("MakeClickable");

			menuActive = false;
			foreach(GameObject go in menuButtons){
				Destroy (go);
			}
			menuButtons.Clear();
		}
		
	}

	IEnumerator ExpandMenu(GameObject go, Vector3 start, Vector3 destination, Vector3 scale){
		float time = 0;
		float maxTime = 0.15f;
		while (true) {
			if(go == null)
				yield break;
			go.transform.position = Vector3.Lerp(start, destination, time/maxTime);
			go.transform.localScale = Vector3.Lerp(Vector3.zero, scale, time/maxTime);
			if(Vector3.Distance(go.transform.position, destination) < 0.01f)
				yield break;
			yield return null;
			time += Time.deltaTime;
		}
	}
	
}
