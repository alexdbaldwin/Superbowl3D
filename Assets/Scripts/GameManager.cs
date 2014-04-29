﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject GUICamera;
	public GameObject ballCamera;
	public GameObject overviewCamera;
	public GameObject motherNode;

	public GameObject overviewGUICamera;

	public bool player2Mode = false;
	private bool inPlacementArea = false;

	// Use this for initialization
	void Start () {


		if (player2Mode) {
			ballCamera.SetActive (false);
			GUICamera.SetActive (false);
			overviewCamera.SetActive (true);
		} else {
			ballCamera.SetActive (true);
			GUICamera.SetActive (true);
			overviewCamera.SetActive (false);		
		}

		MeshRenderer[] meshRenderers;
		meshRenderers = motherNode.GetComponentsInChildren<MeshRenderer>();
		CapsuleCollider[] colliders;
		colliders = motherNode.GetComponentsInChildren<CapsuleCollider> ();
		foreach (MeshRenderer mr in meshRenderers)
		{
			mr.enabled = false;
		}
		foreach (CapsuleCollider cl in colliders) 
		{
			cl.enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			Click(Input.GetTouch(0).position);
		} else if (Input.GetMouseButtonUp (0)) {
			Click(Input.mousePosition);
		}

	}

	void Click(Vector2 position)
	{
		Ray ray = overviewCamera.camera.ScreenPointToRay (position);
		RaycastHit hit;
		if (!inPlacementArea) {
			if (Physics.Raycast(ray, out hit)) {
				if (hit.collider.gameObject.name == "OverviewArea") {
					overviewCamera.transform.rotation = Quaternion.LookRotation(-hit.collider.gameObject.transform.up, hit.collider.gameObject.transform.forward);
					overviewCamera.transform.position = hit.collider.gameObject.transform.position + hit.collider.gameObject.transform.up * 30;
					inPlacementArea = true;
					//Show back arrow
					overviewGUICamera.GetComponentInChildren<SpriteRenderer>().enabled = true;
				}
			}
		}
		else {

			//Only check for collision with layer 10 (placement objects)
			if (Physics.Raycast(ray, out hit, 1000, 1 << 10)) {
				if (hit.collider.gameObject.name == "Cylinder") {

					hit.collider.gameObject.GetComponent<ObstacleMenu>().StartRadialMenu(position);
//					GameObject obst = (GameObject)Instantiate(Resources.Load("Prefabs/GeosphereTower"));
//					obst.transform.position = hit.collider.gameObject.transform.position;
					//obst.transform.rotation = hit.collider.gameObject.transform.rotation;

					//Destroy(hit.collider.gameObject);


				}		
			} else {
				Ray rayB = overviewGUICamera.camera.ScreenPointToRay (position);
				RaycastHit hitB;
				if (Physics.Raycast(rayB, out hitB, 1000, 1 << 11)) {
					if (hitB.collider.gameObject.name == "BackArrow") {
						overviewCamera.GetComponent<OverviewCameraScript>().GoBackToStart();	
						inPlacementArea = false;
						//Hide back arrow
						overviewGUICamera.GetComponentInChildren<SpriteRenderer>().enabled = false;
					}
				}

			}
		}

	}
}
