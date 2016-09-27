using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public float regainAmount = 0.15f;

    public float lastSeenCD = 5f;
    public bool lastSeenBool = false;

    public float life = 100;
    private float maxLife = 100;
    public bool dead = false;
   

    public float Health
    {

        get
        {
            //Some other code
            return life;
        }
        set
        {
            //Some other code
            life += value;

            if (life >= maxLife)
                life = maxLife;
            if (life <= 0)
            {
                life = 0;
                //Game.master.playerDefeated = true;
                //Game.handler.NewGameState(Game.handler.endState);
               
                //dead = true;
            }
            //else if (dead)
            //    dead = false;



            if (life < maxLife && value < 0)
            {
                if (!lastSeenBool)
                    StartCoroutine("LastSeenCD");

                
                StopCoroutine("StartHealthRegain");
                StartCoroutine("StartHealthRegain");

            }

        }


    }

    public bool FullHealth
    {
        get
        {
            //Some other code
            if (life == maxLife)
                return true;
            else
                return false;
        }
        set
        {
            Debug.Log("No way");
        }

    }

    private IEnumerator StartHealthRegain()
    {
        
        while (life < maxLife)
        {
            if(Slender.state != SlenderBehave.IsInSight)
                life += regainAmount;
            
            yield return null;
        }
        life = maxLife;
        yield return 0;
    }

    private IEnumerator LastSeenCD()
    {
        lastSeenBool = true;

        if(this.GetComponent<PlayerSight>().inSight == Sighting.Slenderman)
            StartCoroutine(this.GetComponent<PlayerController>().Flicker());

        yield return new WaitForSeconds(lastSeenCD);
        lastSeenBool = false;
    }

    public float GetLostLifePercentage()
    {

        return 1f-((life / maxLife));
    }

}
