using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum InteractionType
{
    None,
    Drawing,
    Mechanic,
    MechanicalActivation,
    Animation,
    Obstacles

}


public class Interaction : MonoBehaviour {

    public InteractionType interactType;

	public Sprite requireUI;
    //public interactEffect interactEffect;
    public float interactRadius = 4f;
    public float interactDelay = 0.5f;
    public StoryStates activeAtStoryState;

    public bool disappearAfterUse = false;
	public bool destroyColAfterUse = false;
    
    
    public bool withCooldownInteract = false;
    public bool interactDone = false;

    private bool interDefaultDone = false;

    public DrawingScript drawing;
    public InterAnimScript animation;
    public MechanicScript mechanic;
    public ActivatedByMechanic mechanicActiv;

    public Animation AnimationOfOther;
    [HideInInspector]
    public Animation targetAnim;


	public AudioClip audioDefault;
	public string animNameDefault;
	public AudioClip audioInteract;
	public AudioClip specialRemoteAudio;

	public bool playHorror = false;

	public Transform activateObject;


    public float lockTimer = 4f;
    public float exeCD = 8f;

    public KeyScript keyValue;

    public bool goToNextScene = false;

    public Transform activateProjector;

    public DeactivateTrigger dactTrigUse;

    delegate bool MultiInteractDelegate();
    MultiInteractDelegate interactMultiDelegate;


	public bool isDoor = false;

    void Start()
    {
		if (this.GetComponent<ShockerSequence> ())
			this.GetComponent<ShockerSequence> ().myInteraction = this;
        if (this.transform.tag != "Interactable")
        {
            this.transform.tag = "Interactable";
        }

        if (AnimationOfOther) {
			targetAnim = AnimationOfOther;

		}
        else
            targetAnim = GetComponent<Animation>();

        switch (interactType)
        {
            case InteractionType.Drawing:

                drawing.targetAnimator = Game.player.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(5).GetComponent<Animator>();
                drawing.myGOName = gameObject.name;
                interactMultiDelegate = drawing.ShowDrawing;

                break;

            case InteractionType.Animation:


                animation.anim = targetAnim;

                interactMultiDelegate = animation.MakeMyAnimation;

                break;

            case InteractionType.Mechanic:

                mechanic.audio = this.GetComponent<AudioSource>();
                mechanic.anim = targetAnim;
                mechanic.MechStart();
                interactMultiDelegate = mechanic.MyMechanic;

                break;


            case InteractionType.MechanicalActivation:

                mechanicActiv.anim = targetAnim;
                mechanicActiv.ActiveMechStart();
                mechanicActiv.myInteractDad = this;
                interactMultiDelegate = mechanicActiv.MechValid;

                break;

            case InteractionType.None:


                interactMultiDelegate = NoneUse;

                break;

			case InteractionType.Obstacles:
			
			
				interactMultiDelegate = NoneUse;
				
				break;

            default:
                //Debug.LogWarning("There is noch interact like " + interactType.ToString());
                break;
        }
    }

    private bool NoneUse()
    {
        return true;
    }

    public void MyInteraction()
    {
		if(interactType == InteractionType.Obstacles)
			return;
       
        

        if (!LegitActivation())
        {

            if (audioDefault && !interactDone && !interDefaultDone)
            {
                StartCoroutine("DefaultInter");
                this.GetComponent<AudioSource>().clip = audioDefault;
                PlayAudio();

				if (animNameDefault != "") { targetAnim.Play(animNameDefault); }
            }
            
            return;

        }
        
		if (Game.handler.interactState.inspectingObj != this.transform)
		{
			
			Game.handler.interactState.inspectingObj = this.transform;
			
		}else
			return;

        if (interactMultiDelegate != null)
        {

            
            StartCoroutine("Interact");

            if (this.GetComponent<TriggerScript>())
                this.GetComponent<TriggerScript>().DoInteractTrigger();
        }

    }


    private IEnumerator Interact()
    {
        ////set to used
        //interactDone = true;     

		Game.handler.NewGameState(Game.handler.interactState);
        

        // delay
        yield return new WaitForSeconds(interactDelay);

        
        // show if available

        interactDone = interactMultiDelegate();

        if (interactType == InteractionType.Animation && animation.playAllAnimsFromList)
            StartCoroutine(PlayAllAnims(animation.anim));

		if (this.GetComponent<ShockerSequence> ())
			this.GetComponent<ShockerSequence> ().StartTheShocker();

        if (interactDone) {
			if(requireUI)
				requireUI = null;

            if (keyValue.keyState == KeyState.GivesKey)
            {
                Game.handler.playerInventory.ChangeKey(true,keyValue.delayedKey);

                keyValue.keyState = KeyState.None;
            }

			if(audioInteract){
				this.GetComponent<AudioSource>().clip = audioInteract;
				PlayAudio();
			}

			if(activateObject){
				activateObject.gameObject.SetActive(true);
			}

			if(playHorror){
				Game.handler.musicManager.PlayHorrorAmbientClip();
			}

            if (activateProjector)
                activateProjector.gameObject.SetActive(true);

            if (dactTrigUse)
            {

                this.GetComponent<AudioSource>().Stop();
                dactTrigUse.playerIsInMe = true;

            }

			if(isDoor){
				if(this.GetComponent<NavMeshObstacle>() && this.GetComponent<NavMeshObstacle>().enabled){
					this.GetComponent<BoxCollider>().size  = new Vector3(1.0f, this.GetComponent<BoxCollider>().size.y,this.GetComponent<BoxCollider>().size.z);
					this.GetComponent<NavMeshObstacle>().enabled = false;
				}

			}

		} else {

			if(audioDefault){
				this.GetComponent<AudioSource>().clip = audioDefault;
				PlayAudio();
			}
		}

        if (interactType == InteractionType.Mechanic && !interactDone)
        {
            StopCoroutine("Interact");
            StartCoroutine("MechInteract");
            yield return null;
        }
        else
        {

            Executed();

            

            
        }
		if(Game.handler.iActLib.Contains(this))
			GameMasterScript.control.AddToSave(this);
    }

    private bool LegitActivation()
    {
        
        if (activeAtStoryState != StoryStates.None && (int)Game.handler.storyState < (int)activeAtStoryState)
            return false;
        
        if (interactDone)
            return false;

       
        if (!keyValue.OpenMe())
        {
            return false;
        }
        

        return true;
    }

	public bool LegitControlStop()
	{


		if (activeAtStoryState != StoryStates.None && (int)Game.handler.storyState < (int)activeAtStoryState)
			return false;

		if(interactType == InteractionType.Mechanic && !interactDone)
			return true;

		if(interactType == InteractionType.MechanicalActivation && !mechanicActiv.mDone)
			return false;

		if (interactDone)
			return false;

		if(interactType == InteractionType.Drawing)
			return true;
//
//		if(this.GetComponent<TriggerScript>())
//			return true;

		return true;

	}

    private void Executed()
    {
        
        if (goToNextScene)
        {
            Game.handler.NewGameState(Game.handler.endState);
            
        }

        if (disappearAfterUse)
        {

            StartCoroutine("DestroyThis");
            return;
        }


        if (!goToNextScene)
        {
            StartCoroutine(LockPlayer(lockTimer));
        }
        //set cooldown for reuse


        

        
        if (withCooldownInteract)
        {
            //StarteCD
            StartCoroutine(CoolDown(exeCD));
        }
        
    }


    public IEnumerator DestroyThis()
    {

		if(this.GetComponent<Animation>())
	        while(targetAnim.isPlaying)
	            yield return null;

		if(this.GetComponent<AudioSource>())
			while(this.GetComponent<AudioSource>().isPlaying)
				yield return null;

        if (Game.handler.currentState == Game.handler.interactState)
                Game.handler.NewGameState(Game.handler.normalState);
            
        this.gameObject.SetActive(false);
            
    }

    private IEnumerator LockPlayer(float _time)
    {

        
        //Debug.Log("Locking Player");
        yield return new WaitForSeconds(_time);

        if(Game.handler.currentState == Game.handler.interactState)
            Game.handler.NewGameState(Game.handler.normalState);

		
		if (destroyColAfterUse)
		{
			if(this.GetComponent<NavMeshObstacle>())
				this.GetComponent<NavMeshObstacle>().enabled = false;

			this.GetComponent<BoxCollider>().enabled = false;
			
		}
        //Debug.Log("Unlocking Player");
    }

    private IEnumerator CoolDown(float _time)
    {
        yield return new WaitForSeconds(_time);
        interactDone = false;
    }

    private IEnumerator DefaultInter()
    {

        interDefaultDone = true;

        yield return new WaitForSeconds(exeCD);

        interDefaultDone = false;

    }


    private IEnumerator PlayAllAnims(Animation _anim)
    {
        foreach (AnimationState state in _anim)
        {
            _anim.Play(state.clip.name);

            yield return new WaitForSeconds(state.length);

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator MechInteract()
    {
        InteractState myInteract = Game.handler.interactState;
        
//		Debug.Log ("NONONO");
        while (!interactDone)
        {
            
            //if blabla else None, if blabla else None ~ vllt switch
            switch (myInteract.MyControll())
            {
                case MyActions.RollLeft:
                    if (!mechanic.countEnigma)
                        mechanic.DoMechanic(myInteract.myAction);
                    else
                    {
                        mechanic.CurrAnim = +1;

					mechanic.anim.clip = mechanic.anim[mechanic.myAnimations[mechanic.CurrAnim]].clip;
                    }

                    if (mechanic.audioClip)
                    {
						if(mechanic.anim.clip.name == "Reset"){

							mechanic.audio.clip = mechanic.resetAudio;
							mechanic.audio.Play();

						}else{

	                        mechanic.audio.clip = mechanic.audioClip;
	                        mechanic.audio.Play();
						}

                    }
                    
                    yield return new WaitForSeconds(mechanic.anim.clip.length + 0.5f);
                    break;

                case MyActions.RollRight:
                    mechanic.DoMechanic(myInteract.myAction);

                    if (mechanic.audioClip)
                    {
						if(mechanic.anim.clip.name == "Reset"){
							
							mechanic.audio.clip = mechanic.resetAudio;
							mechanic.audio.Play();
							
						}else{
							
							mechanic.audio.clip = mechanic.audioClip;
							mechanic.audio.Play();
						}

                    }
                    
                    yield return new WaitForSeconds(mechanic.anim.clip.length + 0.5f);
                    break;

                case MyActions.Abort:
                    interactDone = true;
                    break;

                default:

                    break;
            }
             

            yield return null;
        }

        if (!withCooldownInteract)
        {

            withCooldownInteract = true;
            lockTimer = 0.5f;
            exeCD = 0.5f;
        }

        if (mechanic.countEnigma && !mechanic.mechanicSolved)
            mechanic.ResetMech();

        Executed();
    }

    public void ResetInteraction(float _delay)
    {
        if (_delay > 0f)
        {
            StartCoroutine(WaitForKey(_delay,true));
            return;
        }

		if(isDoor){
			if(this.GetComponent<NavMeshObstacle>() && !this.GetComponent<NavMeshObstacle>().enabled){
				this.GetComponent<BoxCollider>().size  = new Vector3(1.4f, this.GetComponent<BoxCollider>().size.y,this.GetComponent<BoxCollider>().size.z);
				this.GetComponent<NavMeshObstacle>().enabled = true;
			}
			
		}
        //Reset
        if (interactType == InteractionType.Animation && !keyValue.resetOnlyWithKey)
        {

            animation.anim[animation.anim.clip.name].time = animation.anim.clip.length;
       
			if(animation.anim[animation.anim.clip.name].speed > 0)
            	animation.anim[animation.anim.clip.name].speed = -1f;

            animation.anim.Play(animation.anim.clip.name);

			if(specialRemoteAudio){
				this.GetComponent<AudioSource>().clip = specialRemoteAudio;
				PlayAudio();
				specialRemoteAudio = null;
			}
            StartCoroutine(ResetAnimSpeedAfterFin(animation.anim, animation.anim.clip.name));

        }
        keyValue.keyState = KeyState.None;
        activeAtStoryState = StoryStates.None;
        interactDone = false;
    }


    public void RemoteUse(float _delay)
    {
        if (_delay > 0f)
        {
            StartCoroutine(WaitForKey(_delay, false));
            return;
        }

		if (interactMultiDelegate != null)
			interactDone = interactMultiDelegate ();
		else
			interactDone = true;

       
		if(isDoor){
			if(this.GetComponent<NavMeshObstacle>() && this.GetComponent<NavMeshObstacle>().enabled){
				this.GetComponent<BoxCollider>().size  = new Vector3(1.0f, this.GetComponent<BoxCollider>().size.y,this.GetComponent<BoxCollider>().size.z);
				this.GetComponent<NavMeshObstacle>().enabled = false;
			}
			
		}

		if (specialRemoteAudio) {
			this.GetComponent<AudioSource>().clip = specialRemoteAudio;
			PlayAudio();
			specialRemoteAudio = null;
		}else if (audioInteract && keyValue.remoteOnlyWithKey)
        {
            this.GetComponent<AudioSource>().clip = audioInteract;
            PlayAudio();
        }
        
        if (this.GetComponent<TriggerScript>())
            this.GetComponent<TriggerScript>().DoInteractTrigger();
        
        if (activateObject)
        {
            activateObject.gameObject.SetActive(true);
        }

        if (activateProjector)
            activateProjector.gameObject.SetActive(true);

        if (destroyColAfterUse)
        {
			if(this.GetComponent<NavMeshObstacle>())
				this.GetComponent<NavMeshObstacle>().enabled = false;

           this.GetComponent<BoxCollider>().enabled = false;
            
        }
        
		if(Game.handler.iActLib.Contains(this))
			GameMasterScript.control.AddToSave(this);

		if (this.GetComponent<OnRemoteUse> ())
			this.GetComponent<OnRemoteUse> ().OnRemote ();

        if (disappearAfterUse)
        {

            StartCoroutine("DestroyThis");
            return;
        }

        if (withCooldownInteract && interactType != InteractionType.Mechanic)
        {
            //StarteCD
            StartCoroutine(CoolDown(exeCD));
        }



    }

    private IEnumerator WaitForKey(float _wait, bool _reset)
    {

        yield return new WaitForSeconds(_wait);

        if (_reset)
        {

            ResetInteraction(0f);

        }
        else
        {
            RemoteUse(0f);

        }
    }

    private IEnumerator ResetAnimSpeedAfterFin(Animation _anim, string _animName)
    {
        while (_anim.isPlaying)
        {
            yield return null;
        }

        _anim[_animName].speed = 1f;
    }

	public void PlayAudio(){

		this.GetComponent<AudioSource>().Play();
		if (this.GetComponentsInChildren<AudioSource> ().Length > 0)
			foreach(AudioSource _audioS in this.GetComponentsInChildren<AudioSource>())
				_audioS.Play();
	}

	public void PlayHorrorAudio(){
		
		Game.handler.musicManager.PlayHorrorAmbientClip ();
	}

	public void OnLoadCall() {
		
		specialRemoteAudio = null;

		keyValue.remoteInteraction.Clear();
		keyValue.resetInteraction.Clear();
		keyValue.keyState = KeyState.None;

		keyValue.activatesNewStateAdd = false;

		if(interactType == InteractionType.Mechanic){
		   
		   
			interactType = InteractionType.None;
		}

		if(activateObject){

			activateObject.gameObject.SetActive(true);

			
		}

		if(activateProjector)
			activateProjector.gameObject.SetActive(true);

		if(requireUI)
			requireUI = null;

		if (destroyColAfterUse)
		{

			this.GetComponent<BoxCollider>().enabled = false;
			
			if(this.GetComponent<NavMeshObstacle>())
				this.GetComponent<NavMeshObstacle>().enabled = false;
		}


		if(!interactDone)
			return;


		if(isDoor){
			if(this.GetComponent<NavMeshObstacle>() && this.GetComponent<NavMeshObstacle>().enabled){
				this.GetComponent<BoxCollider>().size  = new Vector3(1.0f, this.GetComponent<BoxCollider>().size.y,this.GetComponent<BoxCollider>().size.z);
				this.GetComponent<NavMeshObstacle>().enabled = false;
			}
			
		}

		switch (interactType)
		{	

		case InteractionType.Drawing:


			
			break;

		case InteractionType.Animation:
			
			AnimationDueToLoad(true);
			
			break;
			
		case InteractionType.Mechanic:

			//targetAnim.play (ifSpecificAnim _>)
			AnimationDueToLoad(false);

			withCooldownInteract = false;

			mechanic. mechanicSolved = true;
			interactDone = true;

			interactType = InteractionType.None;

			break;
			
			
		case InteractionType.MechanicalActivation:
			
			AnimationDueToLoad(false);

			
			break;
		
			
		default:
			//Debug.LogWarning("There is noch interact like " + interactType.ToString());
			break;
		}


	}

	private void AnimationDueToLoad(bool _specAnim) {

		Animation _tarAnim;

		if(AnimationOfOther)
			_tarAnim = AnimationOfOther;
		else
			_tarAnim = this.GetComponent<Animation>();

		if(_specAnim){


			if (animation.animationFromList != "") {
				
				_tarAnim.clip = _tarAnim[animation.animationFromList].clip;

			}


		}else{
		
//			if(!_tarAnim.clip)
//				return;

		}
		_tarAnim[_tarAnim.clip.name].time = _tarAnim.clip.length-0.2f;
		_tarAnim.Play();
	}
}
