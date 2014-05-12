using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : MonoBehaviour {
	// Use this for initialization
	public GameObject targetObject;
	public GameObject[] targetObjects;
	public bool multipleTargets = false;
	public GameObject oldTarget;
	// Kom ihåg att byta på Line Renderer -> Positions -> Size till samma värde som numberOfSegments i
	// Line Renderers properties
	public int numberOfSegments = 0;
	
	private LineRenderer lineRenderer;
	private int randomTarget;
	
	Vector3 pos;
	
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (multipleTargets == false) {
		lineRenderer.SetPosition (0, this.transform.position);
		

			for (int i = 1; i < numberOfSegments; i++) {
				pos = Vector3.Lerp (this.transform.position,
				                    targetObject.transform.position,
				                    i / (float)(numberOfSegments - 1));
				
				pos.x += Random.Range (-0.2f, 0.2f);
				pos.y += Random.Range (-0.2f, 0.2f);
				
				lineRenderer.SetPosition (i, pos);
			}
			
			
			lineRenderer.SetPosition ((numberOfSegments - 1), targetObject.transform.position);
			
					} 
							else {
			
							lineRenderer.SetPosition (0, this.transform.localPosition);
			
			
							randomTarget = (int)Random.Range (0.0f, (float)targetObjects.Length);
			
							for (int i = 1; i <= (numberOfSegments - 1); i++) {
									pos = Vector3.Lerp (this.transform.localPosition,
			                   targetObjects [randomTarget].transform.localPosition,
			                   i / (float)(numberOfSegments - 1));
			
									pos.x += Random.Range (-0.2f, 0.01f);
									pos.y += Random.Range (-0.2f, 0.01f);
			
									lineRenderer.SetPosition (i, pos);
							}
			
							lineRenderer.SetPosition ((numberOfSegments - 1), targetObjects [randomTarget].transform.localPosition);
					}
			
		}
	}

