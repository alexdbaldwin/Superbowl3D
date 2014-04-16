using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Sprite tiltOff, tiltOn;
	bool tilt = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			Click(Input.GetTouch(0).position);
		} else if (Input.GetMouseButtonUp (0)) {
			Click(Input.mousePosition);
		}
		PlayerPrefs.
	}

	void Click(Vector2 position)
	{
		Ray ray = Camera.main.ScreenPointToRay (position);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject.name == "StartButton") {
				SavePreferences();
				Application.LoadLevel ("Test course two");
			} else if (hit.collider.gameObject.name == "TiltButton") {
				if (tilt) {
					hit.collider.gameObject.GetComponent<SpriteRenderer> ().sprite = tiltOff;
				}
				else {
					hit.collider.gameObject.GetComponent<SpriteRenderer> ().sprite = tiltOn;
				}
				tilt = !tilt;
			} else if (hit.collider.gameObject.name == "ExitButton") {
				Application.Quit ();
			}
		}
	}

	void SavePreferences(){
		PlayerPrefs.SetInt ("Tilt", (int)tilt);
	}
}
