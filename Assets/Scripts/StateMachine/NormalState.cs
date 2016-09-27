using UnityEngine;
using System.Collections;

public class NormalState : GameState {


	public override void OnStateEntered (){
		
		Game.state = TheStates.Normal;
        Game.player.GetComponent<PlayerController>().canMove = true;
		
	}
	
	public override void OnStateExit (){
		
	}
	
	
	public override void StateUpdate (){
		
		
		
	}

}