using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverState : GameState {

    public AudioClip dragSound;

	public override void OnStateEntered (){
		
		Game.state = TheStates.GameOver;

        FreezePlayer();

        
	}
	
	public override void OnStateExit (){
		
	}
	
	
	public override void StateUpdate (){
		
		
		
	}

    private void FreezePlayer()
    {
        Game.player.GetComponent<PlayerController>().canMove = false;
		Game.player.GetComponent<PlayerController>().canTOTALMove = false;
        Game.player.GetComponentInChildren<CardboardHead>().trackRotation = false;
        Game.player.GetComponentInChildren<CardboardHead>().trackPosition = false;

		if(Game.player.GetComponent<PlayerController>().virtualControl)
			Game.player.GetComponent<PlayerController>().virtController.SetActive(false);

        StartCoroutine("FreezeAnim");
        
    }

    private IEnumerator FreezeAnim()
    {

        //Game.handler.darknessHandler.DarknessEnabled = true;

        //yield return new WaitForSeconds(0.5f);
        //Game.handler.darknessHandler.DarknessEnabled = false;
        Game.player.GetComponent<PlayerController>().nAgent.ResetPath();
        Game.player.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(Game.player.GetComponent<Animation>().clip.length -0.2f);

        Game.handler.darknessHandler.DarknessEnabled = true;

        //Game.player.GetComponentInChildren<CardboardHead>().transform.LookAt(Game.player.position - Game.player.forward - Game.player.right);
        
        yield return new WaitForSeconds(0.7f);
        

        Game.player.GetComponent<Animation>().Play("PosToDragSequence");

        if (dragSound)
        {
            Game.player.GetComponent<AudioSource>().clip = dragSound;
            Game.player.GetComponent<AudioSource>().Play();
        }


        yield return new WaitForSeconds(dragSound.length);
        yield return new WaitForSeconds(0.7f);
        
        Game.handler.darknessHandler.DarknessEnabled = false;

        yield return new WaitForSeconds(0.7f);
        Game.handler.GetComponent<AudioSource>().Play();

        Game.player.GetComponent<Animation>().Play("TheDrag");
        

        
    }

    
    
   //  //old version
   //yield return new WaitForSeconds(0.7f);
        

   //     Game.player.GetComponent<Animation>().Play("PosRight");

   //     yield return new WaitForSeconds(0.7f);
   //     Game.handler.darknessHandler.DarknessEnabled = false;
   //     yield return new WaitForSeconds(1.5f);

   //     Game.player.GetComponent<Animation>().Play("RotateToMove");
     
}
