using UnityEngine;
using System.Collections;

public class JumpPadScript : MonoBehaviour {
	private Vector3 targetPosition;
	private Vector3 oldPosition;
	public GameObject pad;
	bool goingUp = false;



	void OnTriggerEnter(Collider coll){

		if (coll.tag == "TheBall") {
			StartCoroutine(Bounce ());
			coll.attachedRigidbody.AddForce(new Vector3(0.0f, 8.0f, 0.0f), ForceMode.VelocityChange);
			GetComponent<AudioSource>().Play();
//			networkView.RPC("BroadcastJumpSound", RPCMode.All, null);
		}
	}
	
	[RPC]
	public void BroadcastJumpSound()
	{
		GetComponent<AudioSource>().Play();
	}



	IEnumerator Bounce(){
		oldPosition = pad.transform.position;
		targetPosition = pad.transform.position + transform.up * 0.3f;
		goingUp = true;
		while (true) {

			if(goingUp){
				pad.transform.position = Vector3.Lerp(pad.transform.position, targetPosition, Time.deltaTime *10.0f);
				if(Vector3.Distance(pad.transform.position, targetPosition) < 0.05f){
					goingUp = false;
				}
			} else {
				pad.transform.position = Vector3.Lerp(pad.transform.position, oldPosition, Time.deltaTime *10.0f);
				if(Vector3.Distance(pad.transform.position, oldPosition ) < 0.05f){
					yield break;
				}
			}
			yield return null;		
		
		}


	}
}
