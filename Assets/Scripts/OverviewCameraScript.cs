using UnityEngine;
using System.Collections;

public class OverviewCameraScript : MonoBehaviour {
	private Vector3 startPosition;
	private Quaternion startRotation;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void GoBackToStart(){
		transform.position = startPosition;
		transform.rotation = startRotation;
	}
}
