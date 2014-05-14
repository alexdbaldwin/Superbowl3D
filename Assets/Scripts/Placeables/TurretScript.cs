using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {

	GameObject ball;
	public GameObject barrel;
	public GameObject burst;
	float shootDelay = 0.5f;
	float shootTimer = 0.3f;
	float range = 10.0f;

	void Start () {
		ball = GameObject.FindGameObjectWithTag ("TheBall");
		burst.transform.parent = barrel.transform;
	}

	void Update () {
		if(ball == null)
		{
			ball = GameObject.FindGameObjectWithTag ("TheBall");
		}
		if (GetComponent<PlaceableParameters> ().lockUpdate)
			return;

		if (Vector3.Distance (transform.position, ball.transform.position) < range) {
			shootTimer += Time.deltaTime;
			if(shootTimer >= shootDelay){
				Shoot();
				shootTimer = 0.0f;
			}
		}

		Vector3 targetLookDirection = (ball.transform.position - transform.position).normalized;
		targetLookDirection -= Vector3.Dot (targetLookDirection, transform.up) * transform.up;

		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.LookRotation (targetLookDirection.normalized, transform.up), Time.deltaTime * 10.0f);


		Vector3 barrelLookDirection = (ball.transform.position - barrel.transform.position).normalized;
		//barrelLookDirection -= Vector2.Dot (barrelLookDirection, barrel.transform.forward) * barrel.transform.forward;
		barrelLookDirection -= Vector3.Dot (barrelLookDirection, barrel.transform.right) * barrel.transform.right;
		barrel.transform.localRotation = Quaternion.Lerp (barrel.transform.localRotation, Quaternion.LookRotation (barrel.transform.InverseTransformDirection(barrelLookDirection)), Time.deltaTime * 10.0f);



	}

	void Shoot(){

		//burst.GetComponent<ParticleSystem> ().Play ();
		GameObject bullet = (GameObject)Instantiate (Resources.Load ("Prefabs/Placeables/Bullet"));
		bullet.transform.position = burst.transform.position;
		bullet.transform.rotation = barrel.transform.rotation;
		//bullet.GetComponent<ParticleSystem> ().Play ();
	}
}
