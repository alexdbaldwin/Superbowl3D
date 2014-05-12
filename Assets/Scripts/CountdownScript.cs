using UnityEngine;
using System.Collections;

public class CountdownScript : MonoBehaviour {
	public GameObject ball;
	public GUIStyle countdownStyle;
	float textScale;
	float countdownTime = 3;
	public bool countingDown = false;
	// Use this for initialization
	
	public void StartCountDown()
	{
		if(ball == null)
			ball = GameObject.FindGameObjectWithTag("TheBall");
	
		countingDown = true;
		ball.rigidbody.Sleep();
		ball.GetComponent<AndroidControlScript> ().LockControls ();
	}
	void Start () {
		textScale = (countdownStyle.fontSize * (Screen.width * 0.001f));
		countdownStyle.fontSize = (int)textScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(ball == null)
			ball = GameObject.FindGameObjectWithTag("TheBall");
	
		if (countingDown) {
			countdownTime = countdownTime -Time.deltaTime;

			}
		if (countdownTime <= 0) {
			countingDown = false;
			countdownTime = 3;
			ball.rigidbody.WakeUp();
			ball.GetComponent<AndroidControlScript> ().UnlockControls();
		}
	}
	void OnGUI()
	{
		if (countingDown) {
			GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.4f, 0.0f,0.0f),((int)countdownTime).ToString(), countdownStyle);
		}
	}
}
