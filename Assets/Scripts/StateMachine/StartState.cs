using UnityEngine;
using System.Collections;

public class StartState : GameState {
	


	public override void OnStateEntered (){
		
		Game.state = TheStates.Start;

		
	}

	public override void OnStateExit (){

	}

	

	public override void StateUpdate (){


        Game.handler.NewGameState(Game.handler.normalState);
	}

	

}