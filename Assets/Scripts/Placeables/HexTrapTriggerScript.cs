using UnityEngine;
using System.Collections;

public class HexTrapTriggerScript : MonoBehaviour {

	GameObject oldTarget;
	bool alreadyTrackingBall = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "TheBall") {
			if(!alreadyTrackingBall){
				alreadyTrackingBall = true;
				collider.gameObject.GetComponent<AndroidControlScript>().SlowDown();

				//NullReferenceException: Object reference not set to an instance of an object
				//HexTrapTriggerScript.OnTriggerEnter (UnityEngine.Collider collider) (at Assets/Scripts/Placeables/HexTrapTriggerScript.cs:26)

				foreach(Lightning l in GetComponentsInChildren<Lightning>())
				{
					oldTarget = l.targetObject;
					l.targetObject = collider.gameObject;

				}
				StartCoroutine("resetTarget");
			}
			GetComponentInChildren<AudioSource>().Play();
		}
	}


	IEnumerator resetTarget()
	{
		yield return new WaitForSeconds (2.0f);
		GetComponentInChildren<AudioSource>().loop = false;
		foreach(Lightning l in GetComponentsInChildren<Lightning>())
			l.targetObject = oldTarget;
		alreadyTrackingBall = false;

	}
}
