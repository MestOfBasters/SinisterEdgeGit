using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class DragMe : MonoBehaviour {

    public float waiter = 0.5f;

    public bool finish = false;

    private AudioSource audio;


    public AudioClip dragSound;
    public AudioClip hitSound;
    public AudioClip fallSound;
    public AudioClip finishSound;

    public AudioClip wastedSound;
    public Interaction lastDoor;

    public GameObject credits;

    private GameObject currText;

    private int currTextInt = 0;

	public DarkScript theDark;
	public Animation endOfGame;
	public Text endOfGameText, endOfGameTextC;
	
	public AudioSource musicS;
	public AudioClip theEndMusic;

    void Start()
    {
        audio = this.GetComponent<AudioSource>();

        
    }

    public void HitDown()
    {

        
        audio.clip = hitSound;
        audio.Play();


    }

    public void FallDown()
    {

        audio.clip = fallSound;
        audio.Play();


    }


    public void PlayDragSound()
    {
        if (credits)
            if (!credits.activeSelf)
                credits.SetActive(true);

//        Debug.Log(currTextInt);

        if (currText)
        {
            
            currText.SetActive(false);

            currTextInt++;

            currText = null;

            
        }

        audio.clip = dragSound;
        audio.Play();

        if (!currText)
            currText = credits.transform.GetChild(currTextInt).gameObject;

        if(currTextInt == 8)
            Game.handler.GetComponent<AudioSource>().Pause();
    }


    public void ImFinished()
    {
        finish = true;

        audio.clip = finishSound;
        audio.Play();

        
    }


    public void Wasted()
    {
        
        lastDoor.RemoteUse(0.2f);
        audio.clip = wastedSound;
        audio.Play();

    }


    public void EndOfGame()
    {

		musicS.clip = theEndMusic;
		musicS.Play();

		theDark.DarknessEnabled = true;

//		endOfGame.gameObject.SetActive(true);

		endOfGame.Play("GameEnd");


		StartCoroutine("LastIteration");
		
    }


	IEnumerator LastIteration (){

		yield return new WaitForSeconds(1.5f);

		while(endOfGameText.color.a<1f){
			endOfGameText.color += new Color(1f,1f,1f,(0.05f));
//			Debug.Log ("EndPlus");
			yield return null;
		}

		yield return new WaitForSeconds(1f);

		while(endOfGameTextC.color.a<1f){

			endOfGameTextC.color += new Color(1f,1f,1f,(0.075f));
//			Debug.Log ("QuestPlus");
			yield return null;
		}

		yield return new WaitForSeconds(7f);

		Application.LoadLevel(0);
		

	}
}
