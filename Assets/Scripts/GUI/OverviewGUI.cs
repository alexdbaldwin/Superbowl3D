using UnityEngine;
using System.Collections;

public class OverviewGUI : MonoBehaviour {
	public GameObject overviewCamera;
	public GameObject backArrow;

	private Rect bounds;

	void Start () {
		bounds = gameObject.camera.pixelRect;

		RefreshGUILayout ();

	}
	

	void Update () {
		if (!bounds.Equals (gameObject.camera.pixelRect)) {
			RefreshGUILayout();
			bounds = gameObject.camera.pixelRect;
		}
	}

	void RefreshGUILayout(){

		backArrow.transform.position = gameObject.camera.ScreenToWorldPoint (new Vector3 (camera.pixelWidth/20.0f, camera.pixelWidth/20.0f, 8.0f))
			+ new Vector3(backArrow.GetComponent<BoxCollider>().bounds.size.x/2.0f, backArrow.GetComponent<BoxCollider>().bounds.size.y/2.0f, 0.0f);

	}
}
