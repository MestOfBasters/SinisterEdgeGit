using UnityEngine;
using System.Collections;

public enum SpawnCommand { None = -1, 
    RandomSelf              = 0, 
    RandomCloserPlayer      = 1,
    RandomPlayerOut         = 2,
    RandomPlayerIn          = 3,
    DirectAggressiv         = 4,
    Follow                  = 10,
    Forced                  = 11
}

public enum SpawnMode
{
    None,
    OnSphere,
    OnSphereCloser,
    InsideSphere,
    Follow,
    AggressivFront,
    AggressivSide
}

public class SlenderSearchHandler : MonoBehaviour {

    public float noobProtection = 1f;

    

    public float teleportTimer = 4f;

    public float normalPortRange = 20f;
    public float normalPlayerPortRange = 35f;
    public float normalPlayerPortInRange = 7f;

    public int rndSpawnPsAmount = 20;
    public float maxSpawnDistToPlayer = 7f;
    public float validSpawnRegion = 5f;

    [HideInInspector]
    public int playerNotSightedMe = 0;
    public int aggroNormal = 10;

    public AnimationCurve commandCurve;
    
    

	void Start () {
        StartCoroutine("NoobTimer");

        
	}

    bool RandomPoint(Vector3 center, float range, SpawnMode spawnMode, out Vector3 result)
    {

        for (int i = 0; i < rndSpawnPsAmount; i++)
        {
            Vector3 randomPoint;

            switch (spawnMode)
            {
                case SpawnMode.InsideSphere:
                    randomPoint = center + Random.insideUnitSphere * range;
                    break;

                case SpawnMode.Follow:
                    randomPoint = this.transform.position + center.normalized * Random.Range(3f, range-2f);
                    //Debug.LogError(randomPoint);
                    break;

                case SpawnMode.AggressivFront:
                    randomPoint = Slender.player.position + center.normalized * range;
                    break;

                case SpawnMode.AggressivSide:

                    if(center.x <= 0f)
                        randomPoint = Slender.player.position + (Slender.playerSight.LightBeamDirLeft(true)).normalized * Random.Range(2f, range);
                    else
                        randomPoint = Slender.player.position + (Slender.playerSight.LightBeamDirRight(true)).normalized * Random.Range(2f, range);

                    break;

                default:    //OnSphere
                    randomPoint = center + Random.onUnitSphere * range;
                    break;
            }
                

            NavMeshHit hit;

            if ((NavMesh.SamplePosition(randomPoint, out hit, validSpawnRegion, NavMesh.AllAreas)))
            {
                if (spawnMode == SpawnMode.AggressivSide)
                {
                    if ((Vector3.Distance(Slender.player.position, hit.position) > (maxSpawnDistToPlayer)))
                    {
                        result = hit.position;

                        return true;
                        
                    }
                }

                if ((Vector3.Distance(Slender.player.position, hit.position) > (maxSpawnDistToPlayer)) &&
                    !(Slender.playerSight.PointPositionIsInLightBeam(hit.position))
                    )
                {
                    //Debug.Log(Vector3.Angle(hit.position - Slender.playerSight.camObj.transform.position, Slender.playerSight.camObj.transform.forward) );
                    if (spawnMode != SpawnMode.OnSphereCloser)
                    {
                        result = hit.position;
                        
                        return true;
                    }
                    else if (Slender.currentDist() > (Vector3.Distance(Slender.player.position, hit.position)))
                    {
                        {
                            result = hit.position;

                            return true;
                        }


                    }
                }
            }
        }

        result = Vector3.zero;

       
        Debug.LogError("No Matchable Spawn!");

        //Return recursive=!=!=!=!=?!?!?!?!?!??!?!?!?
        return false; 
    }

    private void ChangePosition(SpawnCommand _command)
    {
        
        Vector3 point;
        Vector3 direction;

        switch(_command){

            case SpawnCommand.RandomSelf:

                if (RandomPoint(transform.position, normalPortRange, SpawnMode.OnSphere, out point))
                {
                    this.transform.position = point;
                }

                break;

            case SpawnCommand.RandomCloserPlayer:

                if (RandomPoint(transform.position, normalPortRange, SpawnMode.OnSphereCloser, out point))
                {
                    this.transform.position = point;
                }

                break;
            case SpawnCommand.RandomPlayerOut:

                if (RandomPoint(Slender.player.position, normalPlayerPortRange, SpawnMode.OnSphere, out point))
                {
                    this.transform.position = point;
                }

                break;
            case SpawnCommand.RandomPlayerIn:

                if (RandomPoint(Slender.player.position, normalPlayerPortInRange, SpawnMode.InsideSphere, out point))
                {
                    this.transform.position = point;
                }

                break;

            case SpawnCommand.Follow:
                direction = Slender.player.transform.position - this.transform.position;

                if (RandomPoint(direction, Vector3.Distance(Slender.player.position, this.transform.position), SpawnMode.Follow, out point))
                {
                    this.transform.position = point;
                }
                else
                {
                    Debug.LogError("No Matchable FollowSpawn!");
                }
                break;

            case SpawnCommand.DirectAggressiv:

                
                if (Slender.player.GetComponent<NavMeshAgent>().hasPath)
                {
                    direction = new Vector3(Slender.player.GetChild(0).GetChild(0).transform.forward.x, 0f, Slender.player.GetChild(0).GetChild(0).transform.forward.z);

                    if (RandomPoint(direction, normalPlayerPortRange, SpawnMode.AggressivFront, out point))
                    {
                        this.transform.position = point;
                        
                    }

                }
                else
                {

                    if (Random.Range(0f, 1f) > 0.5f)
                        direction = Vector3.zero;
                    else
                        direction = Vector3.one;

                    if (RandomPoint(direction, normalPlayerPortRange, SpawnMode.AggressivSide, out point))
                    {
                        this.transform.position = point;
                        
                    }
                }

                break;

            
            default:

                Debug.LogError(_command.ToString() + " is not possible at this point!");
                break;

        }

    }    

    private IEnumerator TeleportTimer()
    {

        while (Slender.state == SlenderBehave.IsInSight)
            yield return null;


        while (true)
        {
            //vllt isnear = return oder wait longer? oder ab ner gewissen dist



            if (Slender.state == SlenderBehave.None)
                Slender.lastPlayerSightCounter++;

            if (Slender.state != SlenderBehave.IsInSight)
            {
                if (Slender.playerSightedMe)
                {
                    ChangePosition(SpawnCommand.Follow);
                    Slender.playerSightedMe = false;
                }
                else
                {
                    ChangePosition(CalculateSlenderSpawn());
                    //ChangePosition(SpawnCommand.DirectAggressiv);
                    
                }
            }
            playerNotSightedMe++;

            yield return new WaitForSeconds((teleportTimer / 1f) + Random.Range(1f, 4f));

            if (Slender.state == SlenderBehave.ForcedPos)
            {
                Slender.state = SlenderBehave.IsNear;
            }

            yield return null;
        }
        
    }

    public void ForceWait(float _timer)
    {
        StopCoroutine("TeleportTimer");

        StartCoroutine(ForceTimer( _timer));

    }

    private IEnumerator ForceTimer( float _timer)
    {

        
        yield return new WaitForSeconds(_timer);

        StartCoroutine("TeleportTimer"); 

    }


    private SpawnCommand CalculateSlenderSpawn()
    {

        float _chance = (Random.Range(0f, 100f) *
            PercentOf(playerNotSightedMe) *
            PercentOf(Slender.lastPlayerSightCounter) *
            PercentOf((float)Slender.state))
            ;

        if(Slender.lastPlayerSightCounter==0 && Slender.state == SlenderBehave.IsNear){
            if (Random.Range(0f, 100f) < _chance)
            {
                Debug.Log("Follow! ");
                return SpawnCommand.Follow;
            }
        }

        if (_chance > 100f)
            _chance = 100f;

        //Debug.Log("Roll: " + _chance + " % - " + (SpawnCommand)(commandCurve.Evaluate(_chance / 100f)));
        return (SpawnCommand)(commandCurve.Evaluate(_chance/100f));
       
    }


    private float PercentOf(float _factor)
    {
        return (1f + (_factor/200f));
    }


    private IEnumerator NoobTimer()
    {
        yield return new WaitForSeconds(noobProtection);

        StartCoroutine("TeleportTimer");
    }
}
