using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	
	float speed = 15.0f;

	void Start () {
		Invoke ("DestroyMe", 5.0f);
	}
	

	void Update () {
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.name != "TurretBarrel") {
			Destroy (gameObject);
		}
	}

	void DestroyMe(){
		Destroy (gameObject);
	}
}
