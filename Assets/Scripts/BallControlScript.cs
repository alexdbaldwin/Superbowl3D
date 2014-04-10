using UnityEngine;
using System.Collections;

public class BallControlScript : MonoBehaviour {
	float turnSpeed = 10.2f;
	float trust = 30.0f;
	float maxTurnSpeed = 2.5f; 
	public float velX = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

				float horizontal = Input.GetAxis ("Horizontal");
				float vertical = Input.GetAxis ("Vertical");
				if (rigidbody.velocity.x < maxTurnSpeed && rigidbody.velocity.x > -maxTurnSpeed) {
						rigidbody.AddForce (new Vector3 (-horizontal * turnSpeed, 0, -vertical * trust));
				}
//			Test

	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 100, 100), rigidbody.velocity.x.ToString());
		GUI.Label (new Rect (0, 20, 100, 100), "Fan va jag e störig");
	}
}
