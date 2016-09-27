using UnityEngine;
using System.Collections;

[System.Serializable]
public class JumpScareTrigger  {

    public string welchen;
    public AudioClip audioScare;

    public void AssignAnimation()
    {
        
    }

    public bool PlayMyJumpScare()
    {
        Game.player.GetComponent<PlayerController>().animFlicker.SetTrigger(welchen);
        Game.player.GetComponentInChildren<FlickerSound>().tmpTrig = this;

        return true;
    }
}
