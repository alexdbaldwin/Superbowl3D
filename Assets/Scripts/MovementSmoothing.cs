using UnityEngine;
using System.Collections;

public class MovementSmoothing : MonoBehaviour {

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	
	bool gotHere = false;
	Vector3 syncPosition = Vector3.zero;


	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		gotHere = false;
		syncPosition = Vector3.zero;
		Vector3 syncAngularVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			gotHere = true;
			syncPosition = rigidbody.position;
			syncAngularVelocity = rigidbody.angularVelocity;
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncAngularVelocity);
		}
		else if (stream.isReading)
		{

		
			stream.Serialize(ref syncPosition);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncStartPosition = rigidbody.position;
			syncEndPosition = syncPosition;
			
			stream.Serialize(ref syncAngularVelocity);
			rigidbody.angularVelocity = syncAngularVelocity;
			
			
				
			//rigidbody.position = syncPosition;
		}

	}
	
	void Update(){
	
		if(!GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IsBall()){
		
			syncTime += Time.deltaTime;
			rigidbody.position = Vector3.Lerp (syncStartPosition, syncEndPosition, syncTime/syncDelay);
			
		}
	
	}


}
