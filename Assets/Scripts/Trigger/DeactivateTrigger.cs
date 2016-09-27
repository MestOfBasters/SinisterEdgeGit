using UnityEngine;
using System.Collections;

public class DeactivateTrigger : MonoBehaviour {

	public bool playerIsInMe = false;

	public bool iamActive = false;

	void OnTriggerEnter(Collider other)
	{
		if (!iamActive)
			return;

		if (other.tag == "Player")
		{
			if (other == Game.player.gameObject.GetComponent<CapsuleCollider>())
			{
//				Debug.Log ("GotHim");
				playerIsInMe = true;
			}
		}
		
	}
}
