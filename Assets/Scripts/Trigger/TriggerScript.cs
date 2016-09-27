using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum TriggerEffect
{
    TheSpawn,
    JumpScareImage,
    Audio,
	Animation,
	RepeatTillNearHorror,
	RemoteUse,
	ResetUse,
    None
}

public enum TriggerType
{
    CollisionTrigger,
    RayTrigger,
    InteractTrigger

}

public class TriggerScript : MonoBehaviour {


    public TriggerType triggerType;
    public TriggerEffect triggerEffect;

    public StoryStates activeAtStoryState;

    [Range(0, 10)]
    public int activateAfter = 0;

    private int activateAfterCounter = 0;

    [Range(0f, 100f)]
    public float chanceToTrigger = 100f;
    public bool oneChanceTotrigger = false;

    [Range(0f, 26f)]
    public float delayTrigger = 0f;

    private bool triggerIsDone = false;

    public bool destroyColider = true;


    public bool triggerDone
    {

        get
        {
            //Some other code
            return triggerIsDone;
        }
        set
        {
            triggerIsDone = value;


        }
    }

    public ForcedSpawn forcedSpawn;
    public AudioSpawn audioSpawn;
    public TriggerAnimation animationTrigger;
    public JumpScareTrigger jumpScareTrigger;
	public KeyScript remoteTrigger;

    delegate bool MultiDelegate();
    MultiDelegate triggerMultiDelegate;

    public List<GameObject> activateObjects = new List<GameObject>();

    void Start()
    {
        if (this.GetComponent<Interaction>())
            destroyColider = false;
        
        if (triggerType == TriggerType.RayTrigger)
        {
            this.transform.tag = "RayTrigger";
        }

        switch (triggerEffect)
        {
            case TriggerEffect.TheSpawn:

                if (this.transform.childCount >= 2)
                {
                    forcedSpawn.SpawnOne = this.transform.GetChild(0).position;
                    forcedSpawn.SpawnTwo = this.transform.GetChild(1).position;
                }
                else
                {
                    triggerEffect = TriggerEffect.None;
                    return;

                }
                triggerMultiDelegate = forcedSpawn.SpawnOnTriggerEnter;

                break;

            case TriggerEffect.Audio:

                triggerMultiDelegate = audioSpawn.DoMyAudio;

                break;

            case TriggerEffect.Animation:

                animationTrigger.AssignAnimation();

                triggerMultiDelegate = animationTrigger.PlayMyAnimation;

                break;

            case TriggerEffect.JumpScareImage:

                jumpScareTrigger.AssignAnimation();

                triggerMultiDelegate = jumpScareTrigger.PlayMyJumpScare;

                break;

		case TriggerEffect.RepeatTillNearHorror:

			if(this.GetComponent<RepeatTillTrigger>())
				triggerMultiDelegate = this.GetComponent<RepeatTillTrigger>().StartHorrorTrigger;
			
			break;

		case TriggerEffect.RemoteUse:
			
			triggerMultiDelegate = remoteTrigger.RemoteFromTrigger;
			
			break;

		case TriggerEffect.ResetUse:
			
			triggerMultiDelegate = remoteTrigger.ResetFromTrigger;
			
			break;

            default:
                Debug.LogWarning("There is noch Trigger like " + triggerEffect.ToString());
                break;
        }
    }


    public void MyRayEvent()
    {
        
        if(!LegitActivation(TriggerType.RayTrigger))
            return;


        DoIt();
            
    }

    public void DoInteractTrigger()
    {

        if (!LegitActivation(TriggerType.InteractTrigger))
            return;


        DoIt();

    }

    void OnTriggerEnter(Collider other)
    {

        if (!LegitActivation(TriggerType.CollisionTrigger))
            return;

        

        if (other.tag == "Player")
        {
            if (other == Game.player.gameObject.GetComponent<CapsuleCollider>())
            {
                DoIt();
            }
        }

    }

    private void DoIt()
    {
        if (this.GetComponent<ShockerSequence>())
            this.GetComponent<ShockerSequence>().StartTheShocker();

        if (triggerMultiDelegate != null)
        {
            if (delayTrigger <= 0f)
            {
                triggerDone = triggerMultiDelegate();
                if (activateObjects.Count > 0)
                    ActivateAll();

            }
            else
                StartCoroutine("DelayCo");

            if(destroyColider)
			    Destroy (this.GetComponent<Collider> ());

			if(Game.handler.trigLib.Contains(this))
				GameMasterScript.control.AddToSave(this);
        }


    }

     private IEnumerator DelayCo()
    {
        
         yield return new WaitForSeconds(delayTrigger);
         triggerDone = triggerMultiDelegate();
		if (activateObjects.Count > 0)
			ActivateAll();
         

    }

     private void ActivateAll()
     {

         foreach (GameObject _go in activateObjects)
            _go.SetActive(true);


     }

    private bool LegitActivation(TriggerType _type)
    {


		if ((int)Game.handler.storyState < (int)activeAtStoryState && activeAtStoryState != StoryStates.None)
            return false;
        
        
        if (triggerType != _type)
            return false;
        
        if (triggerDone)
            return false;
        
        if (activateAfterCounter < activateAfter)
        {
//			Debug.Log (activateAfter);
            activateAfterCounter++;
            return false;
        }
        
        if (Random.Range(0f, 100f) > chanceToTrigger)
        {
            triggerDone = oneChanceTotrigger;
            return false;
        }

        
        return true;
    }

    public void OnLoadCall() {

		triggerDone = true;

		foreach (GameObject _go in activateObjects){


				_go.SetActive(true);

		}

	}
}
