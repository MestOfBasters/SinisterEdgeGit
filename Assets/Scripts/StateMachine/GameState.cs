using UnityEngine;
using System.Collections;

public abstract class GameState : MonoBehaviour{

	
	public abstract void OnStateEntered();
	public abstract void OnStateExit();
	public abstract void StateUpdate();
}