using UnityEngine;
using System.Collections;

public class EndState : GameState {

    

	public override void OnStateEntered (){
		
		Game.state = TheStates.End;
        Game.player.GetComponent<PlayerController>().canTOTALMove = false;
        if (Game.master.playerDefeated)
            StartCoroutine("DefeatSequenz");
        else
            Game.master.NextChapter();

        
	}
	
	public override void OnStateExit (){
		
	}
	
	
	public override void StateUpdate (){
		
		
		
	}

    private IEnumerator DefeatSequenz()
    {
        yield return new WaitForSeconds(4f);
        Application.LoadLevel("Loading");
    }
}
