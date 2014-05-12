using UnityEngine;
using System.Collections;

public class CollectableScript : MonoBehaviour {

	GameObject gameManager;
	float rotSpeed = 100.2f;
	int points = 1;

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager");

		//Position the object so that it is upright with respect to the track's normal direction
		RaycastHit[] hits = Physics.RaycastAll (transform.position, -transform.up, 50.0f);
		foreach (RaycastHit hit in hits) {
			if (hit.collider.tag == "TheLevel"){
				transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
				transform.position = hit.point;
				break;
			}		
		}
	}

	void Update () {
		transform.RotateAround (transform.position, transform.forward, rotSpeed * Time.deltaTime); 
	}

	public void ResetCollectable(){
		//Reenable the collectible for a new play round
		GetComponent<MeshRenderer> ().enabled = true;
		GetComponent<SphereCollider> ().enabled = true;
	}


	//Disables the collectible and gives the ball-controlling player some points on collision
	void OnTriggerEnter(Collider coll){

		//Give the ball-controlling player 'points' points and play a jolly sound effect
		if (gameManager.GetComponent<GameManager>().IsBall()) {
			gameManager.GetComponent<GameManager> ().AddPoints (points);
			GetComponent<AudioSource> ().Play ();
		}

		//Disble the collectible until the next round
		GetComponent<MeshRenderer> ().enabled = false;
		GetComponent<SphereCollider> ().enabled = false;

		//Spawn a one-shot particle system at the collectible's center
		GameObject particles = (GameObject)Instantiate (Resources.Load ("Prefabs/Collectables/Pickup"), transform.position + transform.forward * 0.6f , Quaternion.identity);

	}
}
