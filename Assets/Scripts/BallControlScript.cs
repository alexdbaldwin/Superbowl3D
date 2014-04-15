using UnityEngine;
using System.Collections;

public class BallControlScript : MonoBehaviour {
	public GameObject gameCamera;
	public float turnSpeed = 20f;
	float trust = 30.0f;
	public float maxTurnSpeed = 25f; 
	public float velX = 0.0f;
	public Vector3 currentCollisionNormal;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		Vector3 right = gameCamera.transform.right;
		right.y = 0;
		right.Normalize ();

		Vector3 cross = Vector3.Cross (currentCollisionNormal, right);
		Vector3 forceDir = Vector3.Cross (cross, currentCollisionNormal);

		rigidbody.AddForce (forceDir * horizontal * turnSpeed);
		//rigidbody.AddForce (-currentCollisionNormal * 5f);
//		rigidbody.AddForce (gameCamera.transform.forward * 5);
		


//		if (rigidbody.velocity.x < maxTurnSpeed && rigidbody.velocity.x > -maxTurnSpeed) {
//				rigidbody.AddForce (new Vector3 (horizontal * turnSpeed, 0, -vertical * trust));
//		}
//			Test



	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 100, 100), rigidbody.velocity.x.ToString());
		GUI.Label (new Rect (0, 20, 100, 100), "Fan HEJSAN");
	}

	void OnCollisionStay(Collision collisionInfo) {

		foreach (ContactPoint contact in collisionInfo.contacts) {
			currentCollisionNormal = contact.normal;
		}
		currentCollisionNormal.Normalize ();
	}
		
}
