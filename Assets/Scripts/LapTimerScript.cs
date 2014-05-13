using UnityEngine;
using System.Collections;
using Timing;

public class LapTimerScript : MonoBehaviour {
	public GUIStyle style;
	float counterInSec;
	int counterInMin;
	bool paused = false;
	// Use this for initialization
	public LapTime GetLapTime(){
		return new LapTime(counterInMin, counterInSec);
	}
	
	public void ResetTimer()
	{
		counterInSec = 0;
		counterInMin = 0;
	}
	
	public void Pause(){
		paused = true;	
	}
	
	public void Unpause(){
		paused = false;
	}
	
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!paused){
			counterInSec += Time.deltaTime;
			if(counterInSec > 59)
			{
				counterInMin += 1;
				counterInSec = 0;
			}
		}
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width * 0.5f, 0, 0,0), counterInMin.ToString()+"."+counterInSec.ToString("F2"), style); 
	}
}
