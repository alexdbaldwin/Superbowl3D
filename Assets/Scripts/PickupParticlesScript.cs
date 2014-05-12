using UnityEngine;
using System.Collections;

public class PickupParticlesScript : MonoBehaviour {
	
	void Start () {
		Destroy (gameObject, 1.0f);
	}
}
