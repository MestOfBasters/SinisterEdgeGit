using UnityEngine;
using System.Collections;

public enum MyActions { None, RollLeft, RollRight, Abort}

public enum IActsInput { None, Cardb, Accel, Control}

public class InteractState : GameState
{
    public MyActions myAction = MyActions.None;

	public IActsInput myInputs = IActsInput.Accel;

    private CardboardHead head;

    public float angleMin = 15f, angleMax = 50f;

    public Transform inspectingObj;
    public bool newObj = false;
    RaycastHit hit;
    Vector3 offset;

    private bool remote = false;

    void Start()
    {
        head = Camera.main.GetComponent<StereoController>().Head;
        offset = Game.player.GetComponent<PlayerController>().offset;

        if (Game.player.GetComponent<PlayerController>().hardRemoteC)
            remote = true;

        if (!Game.master.virtualReality)
			myInputs = IActsInput.Accel;
		else
			myInputs = IActsInput.Cardb;

		if(!SystemInfo.supportsAccelerometer)
			myInputs = IActsInput.Control;
			
    }

    public override void OnStateEntered()
    {

        Game.state = TheStates.Interaction;
        newObj = false;

		if(inspectingObj)
			if ((remote && inspectingObj.GetComponent<Interaction>().interactType == InteractionType.Obstacles) || 
			    (remote && inspectingObj.GetComponent<Interaction>().interactDone))
	            return;

		if(remote)
			if(inspectingObj)
				if(!inspectingObj.GetComponent<Interaction>().LegitControlStop())
					return;


        Game.player.GetComponent<PlayerController>().canMove = false;


    }

    public override void OnStateExit()
    {
        inspectingObj = null;
    }


    public MyActions MyControll()
    {
        //controller: l = - R = +

        if (newObj)
        {
            if (myAction != MyActions.Abort)
                myAction = MyActions.Abort;
            return MyActions.Abort;
        }

        if (head.transform.rotation.eulerAngles.x > 65f && head.transform.rotation.eulerAngles.x < 260f)
        {
            if (myAction != MyActions.Abort)
                myAction = MyActions.Abort;
             return MyActions.Abort;
        }
		switch (myInputs){
		case IActsInput.Cardb:

			if (head.transform.rotation.eulerAngles.z > angleMin && head.transform.rotation.eulerAngles.z < (angleMax))
			{
				
				if (myAction != MyActions.RollLeft)
					myAction = MyActions.RollLeft;
				return MyActions.RollLeft;
			}

			if (head.transform.rotation.eulerAngles.z < (360f - angleMin) && head.transform.rotation.eulerAngles.z > (360f - angleMax))
			{
				if (myAction != MyActions.RollRight)
					myAction = MyActions.RollRight;
				return MyActions.RollRight;
			}

			break;

		case IActsInput.Accel:
			if (Input.acceleration.x < -0.3f)
			{
				
				if (myAction != MyActions.RollLeft)
					myAction = MyActions.RollLeft;
				return MyActions.RollLeft;
			}
			if (Input.acceleration.x > 0.3f)
			{
				if (myAction != MyActions.RollRight)
					myAction = MyActions.RollRight;
				return MyActions.RollRight;
			}
			break;

		case IActsInput.Control:
			if (Input.GetAxis ("hori1") < -0.3f)
			{
				
				if (myAction != MyActions.RollLeft)
					myAction = MyActions.RollLeft;
				return MyActions.RollLeft;
			}
			if (Input.GetAxis ("hori1") > 0.3f)
			{
				if (myAction != MyActions.RollRight)
					myAction = MyActions.RollRight;
				return MyActions.RollRight;
			}
			break;

		default:

			Debug.Log ("NO input");
			break;


		}
        
        if (myAction != MyActions.None)
            myAction = MyActions.None;
        return MyActions.None;
    }

    public override void StateUpdate()
    {
        
        Vector3 fwd = head.transform.TransformDirection(Vector3.forward);
        Debug.DrawLine(Game.player.transform.position + offset, hit.point, Color.green);
        
        if (Physics.Raycast(Game.player.transform.position + offset, fwd, out hit, 50f))
        {

            // wenn interactable -> lade Interaction Component
            if (hit.transform != inspectingObj)
            {
                newObj = true;
            }

                
            
        }
    }

}