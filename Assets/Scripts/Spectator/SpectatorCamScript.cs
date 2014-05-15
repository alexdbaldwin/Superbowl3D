using UnityEngine;
using System.Collections;

public class SpectatorCamScript : MonoBehaviour {
	
	private GameObject firstNode;
	public GameObject ball;
	public GameObject targetNode;
	private float camSpeed = 0.5f;
	public GUIStyle uiOverlayStyle;
	private int player1Score = 0, player2Score = 0;
	public string scoreUnit = "SpacePoints", speedUnit = "ly/s";
	private string player1Name = "Player1", player2Name = "Player2";
	private float ballSpeed = 0;
	
	// Use this for initialization
	
	public void SetTargetNode(GameObject newTargetNode)
	{
		targetNode = newTargetNode;
		
	}

	public void SetSpeed(float speed){
		camSpeed = speed;
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
		if(ball == null)
			ball = GameObject.FindGameObjectWithTag("TheBall");
	
		transform.position = Vector3.Lerp(transform.position, targetNode.transform.position, camSpeed * Time.deltaTime);
		transform.LookAt(ball.transform.position);
		
		ballSpeed = GameObject.FindGameObjectWithTag ("TheBall").rigidbody.velocity.magnitude;
	}
	
	void OnGUI()
	{
		GUI.Label (new Rect (Screen.width * 0.045f, 0, 0, 0), player1Score.ToString (), uiOverlayStyle);
		GUI.Label (new Rect (Screen.width * 0.01f, Screen.height * 0.05f, 0, 0), player1Name, uiOverlayStyle);
//		string text = player1Name + ": " + player1Score.ToString () + " " + scoreUnit;
//		GUI.Label (new Rect (0, 0, 200, 200), text, uiOverlayStyle);
		string text = player2Score.ToString ();
		Vector2 labelSize = uiOverlayStyle.CalcSize(new GUIContent(text));
		GUI.Label (new Rect (Screen.width - Screen.width * 0.045f - labelSize.x, 0, 0, 0), player2Score.ToString (), uiOverlayStyle);
		
		labelSize = uiOverlayStyle.CalcSize(new GUIContent(player2Name));
		GUI.Label (new Rect (Screen.width - Screen.width * 0.01f - labelSize.x, Screen.height * 0.05f, 0, 0), player2Name, uiOverlayStyle);
//		GUI.Label (new Rect (Screen.width - labelSize.x, 0, 200, 200), text,uiOverlayStyle);
		text = "Speed: " + ballSpeed.ToString () + speedUnit;
		GUI.Label (new Rect (0, Screen.height - labelSize.y, 200, 200), text, uiOverlayStyle);
		
	}
	
	public void SetPlayerName(string name, int playerType)
	{
		if (playerType == 0) {
			player1Name = name;
		}
		else {
			player2Name = name;
		}
	}
	
	public void AddPoints(int points, int playerType)
	{
		if (playerType == 0) {
			player1Score = points;
		}
		else if (playerType == 1) {
			player2Score = points;
		}
	
	
	}
	
}

