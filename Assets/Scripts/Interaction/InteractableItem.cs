using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class InteractableItem : MonoBehaviour
{

    public float interactionRadius;
    public InteractionType type;

    //drawing
    public Sprite drawingToShow;
    public Animator targetAnimator;

    //animation
    private Animation anim;
    public string animName = "";


    public bool wasUsed;

    
    //eigene FlickerKlasse
    public bool isFlicker;
    
	
	void Start ()
	{
        if(animName != "")
	        anim = GetComponent<Animation>();

        
	}

    public IEnumerator Interact()
    {
        //WIEBITTE WAAAAAAS?!
        //return if already used
        //if (wasUsed)
        //    yield return null;

        
        //set to used
        wasUsed = true;

        // delay
        yield return new WaitForSeconds(.5f);

        //play default Anim if available
        if (animName != "") {anim.Play(animName);}
        
        // show drawing if available
        if (type == InteractionType.Drawing) { Drawing(); }

        //debug
        Debug.Log("interacting with" + gameObject.name);
    
    }

    public void Drawing()
    {
        //show Drawing
        targetAnimator.gameObject.GetComponent<Image>().sprite = drawingToShow;
        targetAnimator.gameObject.GetComponent<DrawingSoundHandler>().PlayDrawingUnfoldSound();

        Game.handler.playerInventory.AddPicture((gameObject.name));

        if (isFlicker){StartCoroutine(GameObject.FindWithTag("Player").GetComponent<PlayerController>().Flicker());}
        GameObject.FindWithTag("MusicManager").GetComponent<MusicManager>().PlayDrawingFoundClip();
        targetAnimator.SetTrigger(gameObject.name);
        StartCoroutine(LockPlayer(4f));
        //set cooldown for reuse
        StartCoroutine(CoolDown(8f));
    }

    public IEnumerator LockPlayer(float _time)
    {
        PlayerController pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        pc.canMove = false;
        Debug.Log("Locking Player");
        yield return new WaitForSeconds(_time);
        pc.canMove = true;
        Debug.Log("Unlocking Player");
    }
    public IEnumerator CoolDown(float _time)
    {
        yield return new WaitForSeconds(_time);
        wasUsed = false;
    }

	
}
