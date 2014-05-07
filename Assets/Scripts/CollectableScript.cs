using UnityEngine;
using System.Collections;

public class CollectableScript : MonoBehaviour {

	GameObject gameManager;
	float rotSpeed = 100.2f;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager");

		RaycastHit[] hits = Physics.RaycastAll (transform.position, -transform.up, 50.0f);
		foreach (RaycastHit hit in hits) {
			if (hit.collider.tag == "TheLevel"){
				transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
				transform.position = hit.point;
				break;
			}		
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (transform.position, transform.forward, rotSpeed * Time.deltaTime); 
	}

	public void ResetCollectable(){
		GetComponent<MeshRenderer> ().enabled = true;
		GetComponent<SphereCollider> ().enabled = true;
		}

	void OnTriggerEnter(Collider coll){

		if (gameManager.GetComponent<GameManager>().IsBall()) {
			gameManager.GetComponent<GameManager> ().AddPoints (1);
			GetComponent<AudioSource> ().Play ();
		}
		GetComponent<MeshRenderer> ().enabled = false;
		GetComponent<SphereCollider> ().enabled = false;

	}
}
