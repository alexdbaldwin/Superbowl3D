using UnityEngine;
using System.Collections;

public class CameraUtilityScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("Mute") == 1)
						AudioListener.volume = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
