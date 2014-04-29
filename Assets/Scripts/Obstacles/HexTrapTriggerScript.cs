using UnityEngine;
using System.Collections;

public class HexTrapTriggerScript : MonoBehaviour {

	GameObject oldTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.name == "Kulan") {
			collider.gameObject.GetComponent<AndroidControlScript>().SlowDown();

			foreach(Lightning l in transform.parent.GetComponentsInChildren<Lightning>())
			{
				oldTarget = l.targetObject;
				l.targetObject = collider.gameObject;

			}
			StartCoroutine("resetTarget");
		}
	}

	IEnumerator resetTarget()
	{
		yield return new WaitForSeconds (2.0f);
		foreach(Lightning l in transform.parent.GetComponentsInChildren<Lightning>())
			l.targetObject = oldTarget;

	}
}
