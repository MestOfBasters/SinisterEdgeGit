using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum KeyState { None, NeedKey, GivesKey, RemoteUsed };

[System.Serializable]
public class KeyScript {

    public KeyState keyState = KeyState.None;
    
    
    public List<Interaction> resetInteraction;
    public List<Interaction> remoteInteraction;

    public bool resetOnlyWithKey = false;
    
    public bool remoteOnlyWithKey = false;


    public float delayReset = 0f;
    public float delayRemote = 0f;

	public bool activatesNewStateAdd = false;

    public float delayedKey = 0f;

    public bool OpenMe()
    {
		if (keyState == KeyState.None) {
            if (activatesNewStateAdd)
            {
                Game.handler.NewstoryState();
                activatesNewStateAdd = false;
            }

		}
        if (!resetOnlyWithKey)
            Reset();
        if (!remoteOnlyWithKey)
            Remote();

        if (keyState == KeyState.RemoteUsed)
            return false;

        if (keyState != KeyState.NeedKey)
            return true;

        if (Game.handler.playerInventory.hasKey)
        {
			if(activatesNewStateAdd)
				Game.handler.NewstoryState();

            Game.handler.playerInventory.ChangeKey(false,0f);
            keyState = KeyState.None;

            if (resetOnlyWithKey)
                Reset();
            if (remoteOnlyWithKey)
                Remote();

            
                
            return true;

        }
        else
            return false;

    }




    private void Remote()
    {
        if (remoteInteraction.Count > 0)
        {
            foreach(Interaction _in in remoteInteraction)
                _in.RemoteUse(delayRemote);

        }

    }



    private void Reset()
    {
        if (resetInteraction.Count > 0)
        {
            foreach (Interaction _in in resetInteraction)
                _in.ResetInteraction(delayReset);

        }

    }

	public bool RemoteFromTrigger(){
		
		if (remoteInteraction.Count > 0)
		{
			foreach(Interaction _in in remoteInteraction)
                _in.RemoteUse(delayRemote);
			
		}
		return true;
	}

	public bool ResetFromTrigger(){
		
		if (resetInteraction.Count > 0)
		{
			foreach (Interaction _in in resetInteraction)
                _in.ResetInteraction(delayReset);
			
		}
		return true;
	}
    
}
