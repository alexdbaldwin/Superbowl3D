using UnityEngine;
using System.Collections;

public class KulanNetworkScript : MonoBehaviour 
{

	public void SetAsOwner()
	{
		NetworkViewID newID = Network.AllocateViewID ();
		GameObject oldBall = this.gameObject;//GameObject.FindGameObjectWithTag("TheBall");
		oldBall.GetComponent<BallUtilityScript>().ResetPosition();
		Network.Instantiate(Resources.Load("Prefabs/Kulan"), oldBall.transform.position, Quaternion.identity, 1);
		Network.Destroy(oldBall);
	}
}
