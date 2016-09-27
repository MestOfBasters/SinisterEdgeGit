using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MechanicScript {

    //animation
    public bool countEnigma = false;
    public int solvedAtAnim = 0;

    public List<bool> countAnims = new List<bool>();

    [HideInInspector]
    public int countAnimInt = 0;

    private int currAnim = 0;

    [HideInInspector]
    public Animation anim;

    [HideInInspector]
    public AudioSource audio;

    
    public AudioClip audioClip;

    [HideInInspector]
    public bool mechanicSolved = false;

    [HideInInspector]
    public List<string> myAnimations;

	[System.NonSerialized]
	public ActivatedByMechanic mechanicToSolved;

    public void MechStart()
    {
        foreach (AnimationState _anim in anim)
            myAnimations.Add(_anim.name);


    }
    

    public bool MyMechanic()
    {
        //Debug.Log("Catched by: " + anim.transform.name);
        return false;
    }

    private void PlayNextClip (bool _reverse){

        if (_reverse)
        {
            CurrAnim = -1;

            anim[myAnimations[CurrAnim]].time = anim[myAnimations[CurrAnim]].length;
            anim[myAnimations[CurrAnim]].speed = -1f;
            
            anim.Play(myAnimations[CurrAnim]);
            
        }
        else
        {
            if (countEnigma)
                if (countAnims[currAnim])
                {
                   
                    countAnimInt++;
                    
                }
                else
                {
                    countAnimInt--;

                }

			anim.clip = anim[myAnimations[CurrAnim]].clip;
            anim[myAnimations[CurrAnim]].speed = 1f;
            anim.Play(myAnimations[CurrAnim]);
            CurrAnim = +1;
        }

        
        
    }

    public int CurrAnim{
        get
        {
            return currAnim;
        }

        set
        {
            currAnim += value;


            if (currAnim >= myAnimations.Count)
            {
				if(countEnigma)
					ResetMech();
                currAnim = 0;

            }

            


            

            if (currAnim < 0)
                currAnim = myAnimations.Count-1;

            if (!countEnigma)
            {
                if (currAnim == solvedAtAnim)
                {
//                    Debug.Log("Mechanic solved (with " + myAnimations[currAnim] + " )");
                    mechanicSolved = true;

                    mechanicToSolved.TestIfMechanicIsSolved();

                }
                else
                {
                    mechanicSolved = false;
                }
            }
            else
            {

                if (countAnimInt == solvedAtAnim)
                {
//                    Debug.Log("CountMechanic solved!");
                    mechanicSolved = true;

                    mechanicToSolved.TestIfMechanicIsSolved();

                }
                else
                {
                    mechanicSolved = false;
                }

            }
            
        }

    }

    public void DoMechanic(MyActions _action)
    {
        
        if (anim.isPlaying)
            return;
        
        
        switch (_action)
        {
            case MyActions.RollLeft:
                PlayNextClip(true);
                break;

            case MyActions.RollRight:

                

                PlayNextClip(false);

                
                
                break;

            case MyActions.Abort:
                Debug.Log("action shouldnt happen");
                break;

            default:
                Debug.Log("action who shouldnt happen");

                break;
        }

        
        

    }



	public AudioClip resetAudio;

    public void ResetMech()
    {
        currAnim = 0;
		anim.clip = anim ["Reset"].clip;
        anim["Reset"].speed = 1f;
        anim.Play("Reset");

		if (resetAudio) {
			audio.clip = resetAudio;
			audio.Play();
		}
        countAnimInt = 0;
        
    }
    
}
