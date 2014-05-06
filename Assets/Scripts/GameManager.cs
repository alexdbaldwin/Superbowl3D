﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject GUICamera;
	public GameObject ballCamera;
	public GameObject overviewCamera;
	public GameObject spectatorCamera;
	public GameObject motherNode;
	public GameObject ball;

	public GameObject overviewGUICamera;

	public bool player2Mode = false;
	private bool inPlacementArea = false;
	private bool cancelZoom = false;

	private bool isSwapped = false;
	private bool isPlaying = false;


	// Use this for initialization
	void Start () {
//		if (player2Mode) {
//			ballCamera.SetActive (false);
//			GUICamera.SetActive (false);
//			overviewCamera.SetActive (true);
//		} else {
//			ballCamera.SetActive (true);
//			GUICamera.SetActive (true);
//			overviewCamera.SetActive (false);		
//		}

//		if (Network.player.ToString () == "1") {
//						ballCamera.SetActive (false);
//						GUICamera.SetActive (false);
//						overviewCamera.SetActive (true);
//				} else if (Network.player.ToString() == "0") {
//						ballCamera.SetActive (true);
//						GUICamera.SetActive (true);
//						overviewCamera.SetActive (false);
//				} else {
//			//Make spectator
//				}

		SetPlayerMode ();

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

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			Click(Input.GetTouch(0).position);
		} else if (Input.GetMouseButtonDown (0)) {
			Click(Input.mousePosition);
		}

	}

	void Click(Vector2 position)
	{

		if (!inPlacementArea) {
			Ray ray = overviewGUICamera.camera.ScreenPointToRay (position);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("OverviewGUIAreas"))) {
				if (hit.collider.gameObject.name == "OverviewArea") {
					overviewCamera.GetComponent<OverviewCameraScript>().LerpTo(hit.collider.gameObject.GetComponent<OverviewAreaRenderer>().alignmentBox.position + hit.collider.gameObject.GetComponent<OverviewAreaRenderer>().alignmentBox.up * 30 ,Quaternion.LookRotation(-hit.collider.gameObject.GetComponent<OverviewAreaRenderer>().alignmentBox.up, hit.collider.gameObject.GetComponent<OverviewAreaRenderer>().alignmentBox.forward), null);
					inPlacementArea = true;
					//Show back arrow
					overviewGUICamera.GetComponentInChildren<SpriteRenderer>().enabled = true;
					overviewGUICamera.camera.cullingMask = 1 << LayerMask.NameToLayer("OverviewGUI");
				}
			}

//			RaycastHit2D hit = Physics2D.Raycast(overviewGUICamera.camera.

		}

		else {
			Ray ray = overviewCamera.camera.ScreenPointToRay (position);
			RaycastHit hit;
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
						overviewCamera.GetComponent<OverviewCameraScript>().GoBackToStart(ShowOverviewAreas);	
						inPlacementArea = false;
						//Hide back arrow
						overviewGUICamera.GetComponentInChildren<SpriteRenderer>().enabled = false;
						//overviewGUICamera.camera.cullingMask = (1 << LayerMask.NameToLayer("OverviewGUI")) | (1 << LayerMask.NameToLayer("OverviewGUIAreas"));

					}
				}

			}
		}

	}

	void SetPlayerMode ()
	{
		if (GameObject.Find ("GlobalStorage").GetComponent<NetworkManager> ().GetId () == 0) {
			ballCamera.SetActive (false);
			GUICamera.SetActive (false);
			spectatorCamera.SetActive(false);
			overviewCamera.SetActive (true);
			isSwapped = true;
			isPlaying = true;
		}
		else
			if (GameObject.Find ("GlobalStorage").GetComponent<NetworkManager> ().GetId () == 1) {
				ballCamera.SetActive (true);
				GUICamera.SetActive (true);
				overviewCamera.SetActive (false);
				spectatorCamera.SetActive(false);
				ball.GetComponent<KulanNetworkScript>().SetAsOwner();
				isPlaying = true;
			}
			else {
			ballCamera.SetActive(false);
			overviewCamera.SetActive(false);
			GUICamera.SetActive(false);
			overviewGUICamera.SetActive(false);
			spectatorCamera.SetActive(true);

			}
	}

	[RPC]
	public void SwapPlayers()
	{
		if (isPlaying) {
			if (isSwapped) {
				ballCamera.SetActive (true);
				GUICamera.SetActive (true);
				overviewCamera.SetActive (false);
				ball.GetComponent<KulanNetworkScript> ().SetAsOwner ();
				isSwapped = false;
			} else {
				ballCamera.SetActive (false);
				GUICamera.SetActive (false);
				overviewCamera.SetActive (true);
				isSwapped = true;
			}
		}

	}

	void ShowOverviewAreas(){
		overviewGUICamera.camera.cullingMask = (1 << LayerMask.NameToLayer("OverviewGUI")) | (1 << LayerMask.NameToLayer("OverviewGUIAreas"));


	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (0, 0, 150, 150), "HEHU")) {
			
			//						NetworkViewID newID = Network.AllocateViewID ();
			//						networkView.RPC ("ChangeOwner", RPCMode.All, newID);
			networkView.RPC("SwapPlayers", RPCMode.All, null);
		}
		
	}


}
