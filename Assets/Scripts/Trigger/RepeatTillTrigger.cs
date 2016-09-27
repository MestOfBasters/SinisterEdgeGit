using UnityEngine;
using System.Collections;

[System.Serializable]
public class RepeatTillTrigger : MonoBehaviour  {
		
	public DeactivateTrigger myTrigger;

	public Transform myPreperat;
	private Animation propAnim;

	public AudioClip normalAudio;
	public AudioClip reverseAudio;

	public float soundBoost = 22f;
    public float animSpeed = 2f;

	private float oldSound;

	public bool reverseFirst = true;
    public bool onlyOneWay = false;
	public bool repeatTillInteraction = false;

    public bool needTwoAudioSources = false;
    public bool waitForAudioEnd = false;

    private bool isLoop = false;

    public float waitTillAgain = 0f;

	public bool StartHorrorTrigger() {

        if (!repeatTillInteraction)
        {
            if(!myTrigger)
			    myTrigger = this.transform.GetChild (0).GetComponent<DeactivateTrigger> ();

			myTrigger.iamActive = true;
		
			if(!Game.handler.playerInventory.currEndTrigger.Contains (myTrigger))
				Game.handler.playerInventory.currEndTrigger.Add (myTrigger);
		}
		propAnim = myPreperat.GetComponent<Animation> ();
		oldSound = myPreperat.GetComponent<AudioSource> ().minDistance;

		myPreperat.GetComponent<AudioSource> ().minDistance = soundBoost;

        if (myPreperat.GetComponents<AudioSource>().Length > 1)
        {
            myPreperat.GetComponents<AudioSource>()[1].minDistance = soundBoost;

        }

        if (propAnim.isPlaying)
        {
            isLoop = true;
            

        }

        if (repeatTillInteraction)
			StartCoroutine ("DoTillPlayer");
		else
			StartCoroutine ("WaitForPlayer");

		return true;
	}

	private IEnumerator WaitForPlayer () {

        

		while(!myTrigger.playerIsInMe){

            if (!isLoop)
            {
                while (propAnim.isPlaying)
                    yield return null;
            }

			if(reverseFirst){

				propAnim[propAnim.clip.name].time = propAnim[propAnim.clip.name].length;
                propAnim[propAnim.clip.name].speed = -animSpeed;
				
				propAnim.Play(propAnim.clip.name);
				if(reverseAudio){

                    if (myPreperat.GetComponent<AudioSource>().loop && !myPreperat.GetComponent<AudioSource>().isPlaying)
                    {
                        myPreperat.GetComponent<AudioSource>().clip = reverseAudio;
                        myPreperat.GetComponent<AudioSource>().Play();
                    }

                    if (!myPreperat.GetComponent<AudioSource>().loop)
                    {
                        if (myPreperat.GetComponents<AudioSource>()[1] && needTwoAudioSources)
                        {
                            myPreperat.GetComponents<AudioSource>()[1].clip = reverseAudio;
                            myPreperat.GetComponents<AudioSource>()[1].Play();

                        }
                        else
                        {
                            myPreperat.GetComponent<AudioSource>().clip = reverseAudio;
                            myPreperat.GetComponent<AudioSource>().Play();
                        }
                    }
				}
                //Debug.Log("Reverse");
                if(!onlyOneWay)
				    reverseFirst = false;
			}
			else
			{
                propAnim[propAnim.clip.name].speed = animSpeed;
				
				propAnim.Play(propAnim.clip.name);
				if(normalAudio){
					myPreperat.GetComponent<AudioSource>().clip = normalAudio;
					myPreperat.GetComponent<AudioSource>().Play();
				}
                //Debug.Log("Normal");
                if (!onlyOneWay)
				    reverseFirst = true;
			}
            
            if (waitTillAgain > 0f)
            {
                
                yield return new WaitForSeconds(waitTillAgain);

            }

            if (isLoop)
            {
                yield return new WaitForSeconds(propAnim.clip.length);
            }

            

			yield return null;
		}

        //Debug.Log("Stop IT");
		StartCoroutine ("StopIT");
	}

	private IEnumerator DoTillPlayer () {

        if (myPreperat.GetComponent<Interaction>().interactDone)
            myPreperat.GetComponent<Interaction>().interactDone = false;

        while (!myPreperat.GetComponent<Interaction>().interactDone)
        {


            while (myPreperat.GetComponent<AudioSource>().isPlaying)
				yield return null;
			

			propAnim[propAnim.clip.name].speed = 1f;
			
			propAnim.Play(propAnim.clip.name);
			if(normalAudio){
				myPreperat.GetComponent<AudioSource>().clip = normalAudio;
				myPreperat.GetComponent<AudioSource>().Play();
			}

            if (waitTillAgain > 0f)
                yield return new WaitForSeconds(waitTillAgain);

			yield return null;
		}

        if (waitForAudioEnd)
        {
            while (myPreperat.GetComponent<AudioSource>().isPlaying)
                yield return null;

        }

		ResetToOld ();

	}


	public IEnumerator StopIT(){

        if (!isLoop)
        {
            while (propAnim.isPlaying)
                yield return null;
        }

		if (reverseFirst) {
			
			propAnim [propAnim.clip.name].time = propAnim [propAnim.clip.name].length;
            propAnim[propAnim.clip.name].speed = -animSpeed;
			
			propAnim.Play (propAnim.clip.name);
			if (reverseAudio) {
				myPreperat.GetComponent<AudioSource> ().clip = reverseAudio;
				myPreperat.GetComponent<AudioSource> ().Play ();
			}
			
		}

        if (!isLoop)
        {
            while (propAnim.isPlaying)
                yield return null;
        }

        if (waitForAudioEnd)
        {
            while (myPreperat.GetComponent<AudioSource>().isPlaying)
                yield return null;

            if (needTwoAudioSources)
            {
                while (myPreperat.GetComponents<AudioSource>()[1].isPlaying)
                    yield return null;
                myPreperat.GetComponents<AudioSource>()[1].clip = null;
            }
        }
        
        //Debug.Log("Done");
		ResetToOld ();

	}

	private void ResetToOld(){
        
        //Resetet
//        Debug.Log("Done Reset");
		if(myPreperat.GetComponent<Interaction>()){
	        myPreperat.GetComponent<Interaction>().interactDone = false;
	        
			myPreperat.GetComponent<AudioSource> ().minDistance = oldSound;
		}
		propAnim[propAnim.clip.name].speed = 1f;

        
		if(myPreperat)
			if (myPreperat.GetComponent<AudioSource> ()) {
				myPreperat.GetComponent<AudioSource> ().Pause ();
				myPreperat.GetComponent<AudioSource> ().clip = null;
			}

         
		if (reverseFirst)
		{
			
			
			myPreperat.GetComponent<Interaction>().interactDone = false;
			
		}
        
	}
}
