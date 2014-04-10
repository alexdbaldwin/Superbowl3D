using UnityEngine;
using System.Collections;

public class CameraPositioningScript : MonoBehaviour {
	public GameObject target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + 1, target.transform.position.z + 3);
		transform.LookAt (target.transform);
	}
}
