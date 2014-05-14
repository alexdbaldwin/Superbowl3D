using UnityEngine;
using System.Collections;

public class BoostTrailScript : MonoBehaviour {

	GameObject ball;

	void LateUpdate () {
		if (ball == null)
			ball = GameObject.FindGameObjectWithTag ("TheBall");

		transform.position = ball.transform.position;
		transform.rotation = Quaternion.FromToRotation (Vector3.forward, -ball.rigidbody.velocity);
	}

	public void Hide(){
		particleSystem.Stop ();
	}

	public void Show(){
		if(!particleSystem.isPlaying) particleSystem.Play ();
	}
}
