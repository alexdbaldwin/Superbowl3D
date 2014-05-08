using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {

	GameObject ball;
	public GameObject barrel;
	
	void Start () {
		ball = GameObject.FindGameObjectWithTag ("TheBall");
	}

	void Update () {

		Vector3 targetLookDirection = (ball.transform.position - transform.position).normalized;
		targetLookDirection -= Vector2.Dot (targetLookDirection, transform.up) * transform.up;

		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.LookRotation (targetLookDirection), Time.deltaTime * 10.0f);


		Vector3 barrelLookDirection = (ball.transform.position - barrel.transform.position).normalized;
		//barrelLookDirection -= Vector2.Dot (barrelLookDirection, barrel.transform.forward) * barrel.transform.forward;
		barrelLookDirection -= Vector2.Dot (barrelLookDirection, barrel.transform.right) * barrel.transform.right;
		barrel.transform.localRotation = Quaternion.Lerp (barrel.transform.localRotation, Quaternion.LookRotation (barrel.transform.InverseTransformDirection(barrelLookDirection)), Time.deltaTime * 10.0f);

	}
}
