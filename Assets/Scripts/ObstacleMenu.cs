using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleMenu : MonoBehaviour {

	public List<GameObject> obstacles = new List<GameObject> ();
	private GameObject overviewGUICamera;

	// Use this for initialization
	void Start () {
		overviewGUICamera = GameObject.FindGameObjectWithTag ("OverviewGUICamera");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartRadialMenu(Vector2 position){

		//Vector3 centerPos = overviewGUICamera.transform.position + overviewGUICamera.transform.forward * 5;
		//centerPos.x += overviewGUICamera.camera.rect.width / 2.0f;
		Vector2 offset = new Vector2(0.0f,1.0f);

		Vector3 centerPos = overviewGUICamera.camera.ScreenToWorldPoint (new Vector3 (position.x, position.y, overviewGUICamera.camera.nearClipPlane)) + overviewGUICamera.transform.forward * 5;

		for (int i = 0; i < obstacles.Count; i++) {

			float angle = i* Mathf.PI * 2.0f / obstacles.Count;
			float x = ((offset.x) * Mathf.Cos(angle)) - (( - offset.y) * Mathf.Sin(angle));
			float y = ((-offset.y) * Mathf.Cos(angle)) - ((offset.x) * Mathf.Sin(angle));
			Vector3 newOffset = new Vector3(x,y,0);
			newOffset = overviewGUICamera.transform.InverseTransformDirection(newOffset);

			GameObject go = (GameObject)Instantiate (obstacles[i]);
			go.transform.position = centerPos + newOffset;
			go.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
			go.layer = 11;

		}


	}


}
