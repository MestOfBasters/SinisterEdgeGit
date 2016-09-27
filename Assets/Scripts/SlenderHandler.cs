using UnityEngine;
using System.Collections;

public enum SlenderBehave { None = -10, IsNear = 4, IsInSight = 14, ForcedPos = 20 }

static class Slender
{
        
    static public Transform lookTarget;
    static public Transform me;
    static public Transform player;
    static public PlayerSight playerSight;
    static public SlenderBehave state = SlenderBehave.None;
    static public bool playerSightedMe = false;
	static public Vector3 oldPos;


    static public int lastPlayerSightCounter = 1;


    static public void GotTarget()
    {
        
        state = SlenderBehave.IsNear;
        lastPlayerSightCounter = 0;
        if (lookTarget == null)
        {
            lookTarget = player;
            
        }

    }

    static public void NoTarget()
    {
        state = SlenderBehave.None;
    }

    static public float currentDist()
    {
        return Vector3.Distance(player.position,me.position);
    }

    static public void ForceSpawn(Vector3 _pos, float _timer){

		if (Slender.me.GetComponent<SlenderHandler> ().mansionSpawn)
			oldPos = Slender.me.transform.position;

		Slender.me.transform.position = _pos;
		if(Slender.me.GetComponent<SlenderSearchHandler>().isActiveAndEnabled)
        	Slender.me.GetComponent<SlenderSearchHandler>().ForceWait(_timer);
    }
}

public class SlenderHandler : MonoBehaviour {

    private Vector3 tarVec;
    public float slenderDmgInterval = 0.8f;
  
	[HideInInspector]
	public bool mansionSpawn = false;
	public bool mansionTemp = false;

    void Awake()
    {
        Slender.player = GameObject.FindGameObjectWithTag("Player").transform;
        Slender.playerSight = Slender.player.GetComponent<PlayerSight>();
        Slender.me = this.transform;
        Slender.lookTarget = Slender.player;
    }

    void Start()
    {
        
        
    }
	
	
	void Update () {

        

        if (Slender.lookTarget != null)
        {
            tarVec = new Vector3(Slender.lookTarget.transform.position.x, this.transform.position.y, Slender.lookTarget.transform.position.z);
            transform.LookAt(tarVec);
        }
	}

   

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        Slender.GotTarget();
            
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        Slender.NoTarget();
            
    //    }
    //}

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        if (other.GetComponent<PlayerSight>().inSight == Sighting.Slenderman)
    //        {
    //            if (Slender.state != SlenderBehave.IsInSight)
    //            {

    //                Slender.playerSightedMe = true;
    //                Slender.state = SlenderBehave.IsInSight;

    //            }
    //        }
    //        else
    //        {
    //            if (Slender.state == SlenderBehave.IsInSight)
    //                Slender.state = SlenderBehave.IsNear;
               
    //        }
            
    //    }
    //}

    public void WaitAtSpawn()
    {

        //StartCoroutine("PlayTheCreepyGuy");

        if (this.GetComponent<AudioSource>())
            if (!this.GetComponent<AudioSource>().isPlaying)
                this.GetComponent<AudioSource>().Play();
    }

    //public IEnumerator PlayTheCreepyGuy()
    //{
        

    //    while (mansionSpawn)
    //    {
    //        if (this.GetComponent<AudioSource>())
    //            if(!this.GetComponent<AudioSource>().isPlaying)
    //                this.GetComponent<AudioSource>().Play();

    //        yield return null;
    //    }

    //    this.GetComponent<AudioSource>().Stop();
    //}
	public void SawMeAtMansion(){
		mansionSpawn = false;
		StartCoroutine ("GoAwayMansion");
	}

	private IEnumerator GoAwayMansion(){
		mansionTemp = true;
		yield return new WaitForSeconds(1f);
        
		if(Slender.playerSight.inSight == Sighting.Slenderman)
			Slender.playerSight.inSight = Sighting.Slenderman;

        if (this.GetComponent<AudioSource>().isPlaying)
            this.GetComponent<AudioSource>().Stop();

		this.transform.position = Slender.oldPos;
		mansionTemp = false;
	}
   
}

