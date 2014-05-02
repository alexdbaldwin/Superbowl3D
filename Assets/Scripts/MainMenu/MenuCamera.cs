using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {
	Vector3 dir;
	float maxBounce = 0.2f;
	Vector3 startPos;
	Quaternion startQuaternion, goalQuaternion;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		dir = new Vector3 (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), 0);
		dir.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(dir * 0.002f);
		if(Vector3.Distance(startPos, transform.position) >= maxBounce){
			dir = startPos - transform.position;
			dir = Vector3.RotateTowards(dir, -dir, Random.Range (0, Mathf.PI/2.0f),0.0f); 
			//dir = Vector3.RotateTowards(dir, -dir, Random.Range (Mathf.PI/2.0f, Mathf.PI),0.0f);
			dir.Normalize();
		}
	}
}


//public class MenuCamera : MonoBehaviour {
//	bool up = true;
//	float maxBounce = 0.2f;
//	Vector3 startPos;
//	
//	// Use this for initialization
//	void Start () {
//		startPos = transform.position;
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		if (up) {
//			transform.Translate(Vector3.up * 0.002f);
//		} else {
//			transform.Translate(-Vector3.up * 0.002f);	
//		}
//		if(Vector3.Distance(startPos, transform.position) >= maxBounce){
//			up = !up;
//		}
//	}
//}
