using UnityEngine;
using System.Collections;

public class SpectatorCamScript : MonoBehaviour {
	
	private GameObject firstNode;
	public GameObject ball;
	public GameObject targetNode;
	private float camSpeed = 2f;
	public GUIStyle uiOverlayStyle;
	public int player1Score = 0, player2Score = 0;
	public string scoreUnit = "sp", speedUnit = "ly/s";
	private float ballSpeed = 0;
	
	// Use this for initialization
	
	public void SetTargetNode(GameObject newTargetNode)
	{
		targetNode = newTargetNode;
		
	}
	
	public void ResetToStart(){
		targetNode = firstNode;
		transform.position = targetNode.transform.position;
	}
	
	void Start () {
		transform.position = targetNode.transform.position;
		firstNode = targetNode;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, targetNode.transform.position, camSpeed * Time.deltaTime);
		transform.LookAt(ball.transform.position);
		
		ballSpeed = GameObject.FindGameObjectWithTag ("TheBall").rigidbody.velocity.magnitude;
	}
	
	void OnGUI()
	{
		
		string text = "Player1: " + player1Score.ToString () + " " + scoreUnit;
		GUI.Label (new Rect (0, 0, 200, 200), text, uiOverlayStyle);
		text = "Player2: " + player2Score.ToString () + " " + scoreUnit;
		Vector2 labelSize = uiOverlayStyle.CalcSize(new GUIContent(text));
		GUI.Label (new Rect (Screen.width - labelSize.x, 0, 200, 200), text,uiOverlayStyle);
		text = "Speed: " + ballSpeed.ToString () + speedUnit;
		GUI.Label (new Rect (0, Screen.height - labelSize.y, 200, 200), text, uiOverlayStyle);
		
	}
	
}

