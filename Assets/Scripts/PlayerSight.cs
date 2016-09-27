using UnityEngine;
using System.Collections;

public enum Sighting {None, Slenderman, Action };
public class PlayerSight : MonoBehaviour
{
    
    public Sighting inSight;                      // Whether or not the player is currently sighted.

    private Transform camObj;

    private SphereCollider col;

    [HideInInspector]
    public  float lbAngle;

    public float directionLightLength = 50f;
    public float directionLightWidth = 50f; //32f  NVR 85f

    public float zoneWeight = 10f; //wie "spannend" ist die derzeitige Umgebung gewichtet?

    private PlayerHealth myHealth;

    Vector3 dirSlendU;
    Vector3 dirSlendL;
    Vector3 dirSlendR;

	Vector3 dirShock;

	private float slendH,slendW;

    private float dirLightWVR = 55f; //32f  NVR 85f
    private float dirLightWNVR = 85f;

    void Awake()
    {
        col = GetComponent<SphereCollider>();
        camObj = this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform;

        myHealth = this.GetComponent<PlayerHealth>();
    }

    void Start()
    {
        
        
		if (GameObject.FindGameObjectWithTag("Slenderman")) {

			slendH = GameObject.FindGameObjectWithTag("Slenderman").GetComponent<CapsuleCollider>().height * 0.56f;
			slendW = GameObject.FindGameObjectWithTag("Slenderman").GetComponent<CapsuleCollider>().radius * 0.5f;
		}
    }

    public void SetSight(bool _Nvr)
    {
        if (!_Nvr)
            directionLightWidth = dirLightWVR;
        else
            directionLightWidth = dirLightWNVR;


        lbAngle = Vector3.Angle(LightBeamDirLeft(false), camObj.transform.forward);
    }

    void LateUpdate()
    {
        //lbAngle = Vector3.Angle(LightBeamDirLeft(false), camObj.transform.forward);

        //Debug.DrawRay(camObj.transform.position, LightBeamDirLeft(false), Color.blue);
        //Debug.DrawRay(camObj.transform.position, LightBeamDirRight(false), Color.blue);

        //Debug.DrawRay(camObj.transform.position, LightBeamDirLeft(true), Color.cyan);
        //Debug.DrawRay(camObj.transform.position, LightBeamDirRight(true), Color.cyan);
    }

    void OnTriggerStay(Collider other)
    {
        if (Time.timeSinceLevelLoad <= 2.0f)
            return;

        // If the player has entered the trigger sphere...
        if (other.tag == "Slenderman")
        {
            // By default the player is not in sight.
            inSight = Sighting.None;

            // Create a vector from the enemy to the player and store the angle between it and forward.
			dirSlendU = other.transform.position + (other.transform.up * slendH * 0.9f) - camObj.transform.position;
			dirSlendL = (other.transform.position - (other.transform.right * slendW)) + (other.transform.up * slendH/1.7f) - camObj.transform.position;
            dirSlendR = (other.transform.position + (other.transform.right * slendW)) + (other.transform.up * slendH / 1.7f) - camObj.transform.position;

            //Debug.DrawRay(camObj.transform.position, dirSlendU, Color.white);
            //Debug.DrawRay(camObj.transform.position, dirSlendL, Color.red);
            //Debug.DrawRay(camObj.transform.position, dirSlendR, Color.red);



            if (PositionIsInLightBeam(dirSlendL) ||
                PositionIsInLightBeam(dirSlendR))
            {
                
                RaycastHit myHit;
               
                
                // ... and if a raycast towards the player hits something...
				if (RayHitTheObject("Slenderman",out myHit))
                {

                    SlenderRay(other);


                }
                

            }
           
            
        }

		if (other.tag == "Shocker") 
		{
            dirShock = (other.GetComponent<ShockerHandler>().dirChild.position) - camObj.transform.position;


			if (PositionIsInLightBeam(dirShock) )
			{
				RaycastHit hitShock;
                if (Physics.Raycast(camObj.transform.position, dirShock.normalized, out hitShock, col.radius))
                    if (hitShock.collider.tag == "Shocker") 
					    ShockRay(other);

			}

		}

        if (other.tag == "RayTrigger")
        {
            
            
                
            RaycastHit hitRay;
            if (Physics.Raycast(camObj.transform.position, camObj.transform.forward.normalized, out hitRay, col.radius))
            {
                if (hitRay.collider.tag == "RayTrigger")
                {
                    hitRay.transform.GetComponent<TriggerScript>().MyRayEvent();
                }
            }
        }
    }

	bool RayHitTheObject(string _tag, out RaycastHit _theHit)
    {
        RaycastHit _hit;

        if (Physics.Raycast(camObj.transform.position, dirSlendL.normalized, out _hit, col.radius))
        {
			if (_hit.collider.tag == _tag)
            {
                //Debug.DrawLine(camObj.transform.position, _hit.point, Color.magenta);
                _theHit = _hit;
                return true;
            }
        }

        if (Physics.Raycast(camObj.transform.position, dirSlendR.normalized, out _hit, col.radius))
        {
			if (_hit.collider.tag == _tag)
            {
                //Debug.DrawLine(camObj.transform.position, _hit.point, Color.magenta);
                _theHit = _hit;
                return true;
            }
        }
        if (Physics.Raycast(camObj.transform.position, dirSlendU.normalized, out _hit, col.radius))
        {
            if (_hit.collider.tag == _tag)
            {
                //Debug.DrawLine(camObj.transform.position, _hit.point, Color.magenta);
                _theHit = _hit;
                return true;
            }
        }

        _theHit = _hit;
        return false;
    }

    private void SlenderRay(Collider _other)
    {


		if (_other.GetComponent<SlenderHandler> ().mansionTemp) {

			return;
		}
        // ... and if the raycast hits the player...
        if (_other.GetComponent<SlenderHandler> ().mansionSpawn) {

			inSight = Sighting.Slenderman;
			

            myHealth.Health = -70f;
			Game.handler.playerInventory.DeactivateAllTrigger();
			Game.slender.GetComponent<SlenderHandler>().SawMeAtMansion();

			return;
		}

        
        

    }

	private void ShockRay(Collider _other)
	{
		
		//if  Shocker Done
		if (_other.GetComponent<ShockerHandler> ().shockDone) {
			
			return;
		}

		//StartTheShocker
		_other.GetComponent<ShockerHandler> ().ShockMe ();
		
	}

    public bool PositionIsInLightBeam(Vector3 _direction)
    {
        if (Vector3.Angle(_direction, camObj.transform.forward) < lbAngle)
            return true;
        else
            return false;

    }

    public bool PositionIsInLightBeamSpawn(Vector3 _direction)
    {
        if (Vector3.Angle(_direction, camObj.transform.forward) < (lbAngle+15f))
            return true;
        else
            return false;

    }


    public bool PointPositionIsInLightBeam(Vector3 _pos)
    {
        
        //if (Vector3.Distance(_pos, camObj.transform.position) < (col.radius+3f))
        //{
        if (PositionIsInLightBeamSpawn(_pos - camObj.transform.position) && Vector3.Distance(_pos, camObj.transform.position) < (col.radius + 3f))
                return true;
        //}

        return false;
     }

    public Vector3 LightBeamDirLeft(bool _spawn)
    {
        if(_spawn)
            return (((camObj.transform.position - camObj.transform.right * 5f) + camObj.transform.forward * directionLightLength - camObj.transform.right * directionLightWidth / 2) - camObj.transform.position);
        else        
            return (camObj.transform.position + camObj.transform.forward * directionLightLength - camObj.transform.right * directionLightWidth / 2) - camObj.transform.position;
    }

    public Vector3 LightBeamDirRight(bool _spawn)
    {
        if (_spawn)
            return (((camObj.transform.position + camObj.transform.right * 5f) + camObj.transform.forward * directionLightLength + camObj.transform.right * directionLightWidth / 2) - camObj.transform.position);
        else
            return (camObj.transform.position + camObj.transform.forward * directionLightLength + camObj.transform.right * directionLightWidth / 2) - camObj.transform.position;
    }

    private float SeenDamage(float _dmg){

        //Debug.Log(((col.radius + 1.1f) - _dmg) / 10f);
        return ((col.radius) - (_dmg*0.8f)) / 100f;
    }
   
}