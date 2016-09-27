using UnityEngine;
using System.Collections;

[System.Serializable]
public class ForcedSpawn  {

    public float stayAfterSpawn = 10f;
    
	public bool hardSpawn = false;

    [HideInInspector]
    public Vector3 SpawnOne, SpawnTwo;

    



    public bool SpawnOnTriggerEnter()
    {
				
		
        Slender.state = SlenderBehave.ForcedPos;
                
        NavMeshHit hit;


		if (hardSpawn) {
			Game.slender.GetComponent<SlenderHandler> ().mansionSpawn = true;
            Game.slender.GetComponent<SlenderHandler>().WaitAtSpawn();
			if ((NavMesh.SamplePosition(SpawnOne, out hit, 1f, NavMesh.AllAreas)))
			{
				Slender.ForceSpawn(hit.position, stayAfterSpawn);
                //Debug.Log("TriggerSpawn!");
				return true;
			}

		}
        if (Slender.playerSight.PointPositionIsInLightBeam(SpawnOne))
        {

            if ((NavMesh.SamplePosition(SpawnTwo, out hit, 1f, NavMesh.AllAreas)))
            {
                Slender.ForceSpawn(hit.position, stayAfterSpawn);
                //Debug.Log("TriggerSpawn!");
                return true;
            }

        }
        else
        {

            if ((NavMesh.SamplePosition(SpawnOne, out hit, 1f, NavMesh.AllAreas)))
            {
                Slender.ForceSpawn(hit.position, stayAfterSpawn);
                //Debug.Log("TriggerSpawn!");
                return true;
            }

        }

        //Debug.LogWarning("No Machable TriggerSpawn!");   
        return false;

    }
	
	
}
