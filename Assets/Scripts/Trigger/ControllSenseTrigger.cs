using UnityEngine;
using System.Collections;

public class ControllSenseTrigger : MonoBehaviour {

	public StoryStates activeAtStoryState;

	public bool isDone = false;


	public GameObject myC;

	void OnTriggerEnter(Collider other)
	{
		if (isDone)
			return;
		
		if(Game.master.virtualReality)
			return;
		
		if (!LegitActivation())
			return;




		if (other.tag == "Player")
		{
			if (other == Game.player.gameObject.GetComponent<CapsuleCollider>())
			{
				myC.SetActive(true);
				isDone = true;
			}
		}

	}

	private bool LegitActivation()
	{


		if ((int)Game.handler.storyState < (int)activeAtStoryState && activeAtStoryState != StoryStates.None)
			return false;


		return true;
	}
}
