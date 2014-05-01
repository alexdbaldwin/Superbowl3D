using UnityEngine;
using System.Collections;

public class OverviewCameraScript : MonoBehaviour {
	private Vector3 startPosition;
	private Quaternion startRotation;
	private bool cancelZoom1 = false;
	private bool cancelZoom2 = false;
	private bool zooming1 = false;
	private bool zooming2 = false;

	private bool whichZoom = true;

	public delegate void ZoomFinishedCallback ();

	// Use this for initialization
	void Start () {

//		GameObject track = GameObject.FindGameObjectWithTag ("Track");
//		//track.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
//		float l = track.GetComponent<MeshFilter> ().mesh.bounds.size.z * track.transform.localScale.z;
//		float theta = Mathf.PI / 6.0f;
//		float dist = l / 2.0f * Mathf.Sqrt (1 / Mathf.Pow (Mathf.Sin (theta), 2) - 1);
//		dist /= camera.aspect;
//
//		Debug.Log(track.GetComponent<MeshFilter> ().mesh.bounds.size.ToString());
//
//		transform.position = track.transform.position + track.GetComponent<MeshFilter> ().mesh.bounds.center + new Vector3 (0, track.GetComponent<MeshFilter> ().mesh.bounds.size.y / 2.0f * track.transform.localScale.y + dist, 0);

		//transform.position = track.transform.position + new Vector3(track.GetComponent<MeshFilter> ().mesh.bounds.center.x, track.GetComponent<MeshFilter> ().mesh.bounds.center.y + track.GetComponent<MeshFilter> ().mesh.bounds.size.y/2.0f * track.transform.localScale.y + dist, track.GetComponent<MeshFilter> ().mesh.bounds.center.z*track.transform.localScale.z);

		startPosition = transform.position;
		startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void GoBackToStart(ZoomFinishedCallback zoomFinished){

		LerpTo (startPosition, startRotation, zoomFinished);
	}

	public void LerpTo(Vector3 targetPosition, Quaternion targetRotation, ZoomFinishedCallback zoomFinished){
		if (whichZoom) {
			if(zooming2){
				cancelZoom2 = true;
			}
			zooming1 = true;
		} else {
			if(zooming1){
				cancelZoom1 = true;
			}
			zooming2 = true;
		}


		StartCoroutine (LerpToStep (targetPosition, targetRotation, whichZoom, zoomFinished));
		whichZoom = !whichZoom;

	}

	IEnumerator LerpToStep(Vector3 targetPosition, Quaternion targetRotation, bool firstZoom, ZoomFinishedCallback zoomFinished){
		
		while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f 
		       && Vector3.Distance(targetPosition, transform.position) > 0.01f) {
			if(firstZoom && cancelZoom1){
				cancelZoom1 = false;
				zooming1 = false;
				if(zoomFinished != null) zoomFinished ();
				yield break;
			} else if (!firstZoom && cancelZoom2){
				cancelZoom2 = false;
				zooming2 = false;
				if(zoomFinished != null) zoomFinished ();
				yield break;
			}
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime*6.0f);
			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime*8.0f);
			yield return null;
		}

		if (firstZoom) {
			zooming1 = false;		
		} else {
			zooming2 = false;
		}
		transform.rotation = targetRotation;
		transform.position = targetPosition;
		if(zoomFinished != null) zoomFinished ();
		
	}
}
