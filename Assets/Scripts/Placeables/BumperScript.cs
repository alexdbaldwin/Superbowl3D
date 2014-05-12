using UnityEngine;
using System.Collections;

public class BumperScript : MonoBehaviour {

	public GameObject pointLight;
	bool fade = false;
	float pulseRate = 1.0f;
	float colorRate = 0.2f;
	float minIntensity = 0.2f;
	float maxIntensity = 0.8f;
	float colorModifier = 0.0f;
	bool bumped = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if (!bumped) {
//			if (fade) {
//				//pointLight.GetComponent<Light> ().intensity -= Time.deltaTime * pulseRate;
//				GetComponent<MeshRenderer> ().materials [1].color = new Color (89.0f / 256.0f + colorModifier, 30.0f / 256.0f + colorModifier, 150.0f / 256.0f + colorModifier);
//				colorModifier -= Time.deltaTime * colorRate;
//				if (pointLight.GetComponent<Light> ().intensity < minIntensity) {
//					fade = false;		
//				}
//			} else {
//				pointLight.GetComponent<Light> ().intensity += Time.deltaTime * pulseRate;
//				GetComponent<MeshRenderer> ().materials [1].color = new Color (89.0f / 256.0f + colorModifier, 30.0f / 256.0f + colorModifier, 150.0f / 256.0f + colorModifier);
//				colorModifier += Time.deltaTime * colorRate;
//				if (pointLight.GetComponent<Light> ().intensity > maxIntensity) {
//					fade = true;		
//				}
//			}
//		}
	}


	void OnCollisionEnter(Collision cO)
	{
		foreach (ContactPoint contact in cO.contacts){
			Vector3 direction = Vector3.Reflect(cO.gameObject.rigidbody.velocity, contact.normal);
			direction -= Vector3.Dot(direction, transform.up)* transform.up;
			cO.gameObject.rigidbody.AddForce(direction * 3.0f,ForceMode.VelocityChange);
		}
		GetComponent<AudioSource>().Play();

		//StartCoroutine (Bump ());

	}


	IEnumerator Bump(){
		bumped = true;
		pointLight.GetComponent<Light> ().intensity = 1.0f;
		GetComponent<MeshRenderer> ().materials [1].color = Color.white;
		yield return new WaitForSeconds (0.2f);
		float glowTimer = 0.0f;
		while (true) {

			glowTimer += Time.deltaTime;
			GetComponent<MeshRenderer> ().materials [1].color = Color.Lerp(Color.white, new Color(89.0f / 256.0f, 30.0f / 256.0f, 150.0f / 256.0f), glowTimer/0.5f);
			pointLight.GetComponent<Light> ().intensity = 1.0f - glowTimer/0.5f;
			if(glowTimer >= 0.5f){

				bumped = false;
				colorModifier = 0.0f;
				fade = false;
				pointLight.GetComponent<Light> ().intensity = 0.2f;
				GetComponent<MeshRenderer> ().materials [1].color = new Color (89.0f / 256.0f + colorModifier, 30.0f / 256.0f + colorModifier, 150.0f / 256.0f + colorModifier);
				yield break;
			}
			yield return null;
		}


	}
}