using UnityEngine;
using System.Collections;

public class OverviewGUI : MonoBehaviour {
	public GameObject overviewCamera;
	public GameObject backArrow;
	public GameObject ballOverviewSprite;
	private Rect bounds;
	GameObject theBall;
	Vector3 ballPos;

	void Start () {
		bounds = gameObject.camera.pixelRect;
		theBall = GameObject.FindWithTag ("TheBall");
		RefreshGUILayout ();

	}
	

	void Update () {
		if (!bounds.Equals (gameObject.camera.pixelRect)) {
			RefreshGUILayout();
			bounds = gameObject.camera.pixelRect;
		}

		ballPos = overviewCamera.camera.WorldToScreenPoint (theBall.transform.position);

		ballOverviewSprite.transform.position = gameObject.camera.ScreenToWorldPoint (ballPos);
		ballOverviewSprite.transform.position = new Vector3 (ballOverviewSprite.transform.position.x, ballOverviewSprite.transform.position.y, 5);

	}

	void RefreshGUILayout(){

		backArrow.transform.position = gameObject.camera.ScreenToWorldPoint (new Vector3 (camera.pixelWidth/20.0f, camera.pixelWidth/20.0f, 8.0f))
			+ new Vector3(backArrow.GetComponent<BoxCollider>().bounds.size.x/2.0f, backArrow.GetComponent<BoxCollider>().bounds.size.y/2.0f, 0.0f);

	}
}
