using UnityEngine;
using System.Collections;

public class TowerScript : MonoBehaviour {

	public GameObject target;
	
	private Vector3 targetPosition;
	public Vector3 oldDirection;


	void Start () {
		oldDirection = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerStay(Collider collider)
	{
			targetPosition = new Vector3 (target.transform.position.x,
			                              this.transform.position.y,
			                              target.transform.position.z);
			
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.LookRotation (targetPosition - transform.position), Time.deltaTime * 10.0f);
		

	}

	
}
