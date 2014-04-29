using UnityEngine;
using System.Collections;

public class MakeStaticScript : MonoBehaviour {
	
	void Start () {
		DontDestroyOnLoad (gameObject);
	}

}
