using UnityEngine;
using System.Collections;

public class BallUtilityScript : MonoBehaviour {

	//private Vector3 startPos;

	// Use this for initialization
	void Start () {
		//startPos = transform.position;
	}

//	public Vector3 GetStartPosition()
//	{
//		return startPos;
//	}

	public void ResetPosition()
	{
		transform.position = GameObject.FindGameObjectWithTag("BallStartPos").transform.position;
		rigidbody.velocity = Vector3.zero;
	}

}
