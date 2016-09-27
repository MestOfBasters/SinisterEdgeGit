using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ActivatedByMechanic {

    public List<Interaction> relatedMechanics;

    public bool disappearMechsAfterDone = false;

    [HideInInspector]
    public List<MechanicScript> realMechs;

    [HideInInspector]
    public Animation anim;

    public bool autoActivate = false;
    public float autoAcDely = 0f;

    public GameObject specialEffect;

    [HideInInspector]
    public Interaction myInteractDad;
    [HideInInspector]
	public bool mDone = false;

    public void ActiveMechStart()
    {
        foreach (Interaction _mechs in relatedMechanics) {
			realMechs.Add (_mechs.mechanic);
            _mechs.mechanic.mechanicToSolved = this;
		}

    }

    public bool MechValid()
    {
        foreach (MechanicScript _rMechs in realMechs)
        {
            if (!_rMechs.mechanicSolved)
                return false;

        }

        anim.Play();

        foreach (Interaction _mechs in relatedMechanics)
        {
            _mechs.interactDone = true;
            _mechs.withCooldownInteract = false;

            if (disappearMechsAfterDone)
				_mechs.DestroyThis();
        }
		mDone = true;
        return true;

    }

	public void TestIfMechanicIsSolved(){

		foreach (MechanicScript _rMechs in realMechs)
		{
			if (!_rMechs.mechanicSolved)
				return;

		}

        if (specialEffect)
            specialEffect.SetActive(true);

        foreach (Interaction _mechs in relatedMechanics)
        {
            _mechs.interactDone = true;
            _mechs.interactType = InteractionType.None;

			if(Game.handler.iActLib.Contains(_mechs))
				GameMasterScript.control.AddToSave(_mechs);

            _mechs.withCooldownInteract = false;

			if(disappearMechsAfterDone){
				_mechs.disappearAfterUse = true;
				_mechs.RemoteUse(0f);
			}
        }
		mDone = true;
        if (myInteractDad.keyValue.remoteOnlyWithKey)
        {

            myInteractDad.keyValue.remoteOnlyWithKey = false;
        }

        if(autoActivate)
            myInteractDad.RemoteUse(autoAcDely);
		else
			Game.handler.musicManager.PlaySolvedTheMechClip ();
	}
}
