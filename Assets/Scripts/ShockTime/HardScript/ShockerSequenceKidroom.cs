using UnityEngine;
using System.Collections;

public class ShockerSequenceKidroom : ShockerSequence {


    public bool isDone = false;

	public GameObject doorBlock;


	public AudioClip melody;

	public Interaction theDoor;
	public AudioClip newDoorSound;
	public float newAnimationSpeed = 0.5f;

	public float creepyCounterDoor = 11f;

	public float creepyCounterDarkness = 2f;

	public float creepyCounterWaitInDark = 4f;

	public float creepyCounterWaitAfterDark = 4f;

	public float creepyCounterFinish = 1f;

	public AudioClip afterDark;

	public GameObject projector;

	public GameObject slender;

	public GameObject number;

    public GameObject activateAfterAll;

	public override void StartTheShocker(){

		if(doorBlock)
			doorBlock.SetActive (true);

        if (!isDone)
        {
            StartCoroutine("WaitTillAnimDone");
            isDone = true;
        }
	}

	private IEnumerator WaitTillAnimDone(){

		if (this.GetComponent<Animation> ())
			while (this.GetComponent<Animation>().isPlaying)
				yield return null;

		StartCoroutine ("StartIT");

	}


	private IEnumerator StartIT(){

		if (melody) {
			this.GetComponent<AudioSource>().clip = melody;
			this.GetComponent<AudioSource>().Play();
			this.GetComponent<Animation>().Play ("play");
		}
        Game.player.GetComponent<PlayerController>().canMove = false;

		yield return new WaitForSeconds (creepyCounterDoor);
        
		//tür Langsam zu
		if (theDoor) {
			theDoor.animation.anim[theDoor.animation.anim.clip.name].speed = - newAnimationSpeed;

			if(newDoorSound)
				theDoor.specialRemoteAudio = newDoorSound;

			theDoor.ResetInteraction(0f);
            theDoor.interactDone = true;
		}

		//warte bis musik fertig
		while(this.GetComponent<AudioSource>().isPlaying){

			yield return null;
		}

		yield return new WaitForSeconds (creepyCounterDarkness);

		Game.handler.darknessHandler.DarknessEnabledEffect = true;

		//warte bis licht angeht

        Game.handler.musicManager.PlayShockingClip(afterDark);
        yield return new WaitForSeconds (creepyCounterWaitInDark);

		

		

		if (projector)
			projector.SetActive (true);

		if(slender)
			slender.SetActive(true);

		Game.handler.darknessHandler.DarknessEnabled = false;

		yield return new WaitForSeconds (creepyCounterWaitAfterDark);

		Game.handler.darknessHandler.DarknessEnabled = true;

		yield return new WaitForSeconds (creepyCounterFinish);

		Finished ();
	}


	private void Finished(){

		if (number)
			number.SetActive (true);
		if (doorBlock)
			doorBlock.GetComponent<Interaction> ().RemoteUse (0f);

		Game.player.GetComponent<PlayerController> ().canMove = true;

		if (projector)
			projector.SetActive (false);
		if(slender)
			slender.SetActive(false);

        theDoor.interactDone = false;
		Game.handler.darknessHandler.DarknessEnabled = false;

		myInteraction.GetComponent<Animation> ().Stop ();

        if (activateAfterAll)
        {

            activateAfterAll.SetActive(true);
        }
	}
}
