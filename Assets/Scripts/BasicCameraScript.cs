using UnityEngine;
using System.Collections;

public class BasicCameraScript : MonoBehaviour {
	public GameObject target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt (target.transform);
		transform.position = target.transform.position + new Vector3(0, 0.5f, -2);
	}
}
