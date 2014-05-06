using UnityEngine;
using System.Collections;

public class FreePlacementScript : MonoBehaviour {

	GameManager gm;
	PlaceableMenu menu;
	public GameObject overviewCamera;


	// Use this for initialization
	void Start () {
	
		//Get a reference to the GameManager script so we can find out which placement area we're currently looking at (if any)
		gm = GetComponent<GameManager> ();
		//Get a reference to the obstacle menu so we can start a new radial menu when the player clicks on a suitable area
		menu = GetComponent<PlaceableMenu> ();

	}
	
	// Update is called once per frame
	void Update () {
	

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			Click(Input.GetTouch(0).position);
		} else if (Input.GetMouseButtonDown (0)) {
			Click(Input.mousePosition);
		}

	}

	void Click(Vector2 position){

		//Check that we're zoomed in on a placement area
		GameObject placementBox = gm.GetPlacementBox ();
		if (placementBox != null) {
			
			Ray ray = overviewCamera.camera.ScreenPointToRay (position);
			RaycastHit[] hits = Physics.RaycastAll (ray, 1000, 1 << 0 /*Layer mask 0, default*/);
			foreach(RaycastHit hit in hits) {
				if (hit.collider.gameObject.tag == "TheLevel") {
					//Check that the played clicked in the right area, not on another part of the track
					if(placementBox.GetComponent<BoxCollider>().bounds.Contains(hit.point)){

						//Check that we're not too close to an existing object
						bool spaceFree = true;
						Collider[] colliders = Physics.OverlapSphere(hit.point, 0.9f); //Totally arbitrary size
						foreach(Collider coll in colliders){
							if (coll.gameObject.tag == "Obstacle" && !coll.isTrigger){
								spaceFree = false;
								break;
							}
						}
						if(spaceFree){
							menu.StartRadialMenu(position, hit.point, hit.normal);
						}
					}
					break;
				}
			} 
			
		}

	}

}
