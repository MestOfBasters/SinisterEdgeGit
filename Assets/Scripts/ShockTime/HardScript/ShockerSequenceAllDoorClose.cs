using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShockerSequenceAllDoorClose : ShockerSequence {


    public List<Interaction> horrorDoors = new List<Interaction>();

    public AudioClip doorCloseSound;

    public float soundMinDist = 4f;

    private List<float> oldSoundMinDist = new List<float>();
    
    private float doorSpeed = 2f;

    public float doorCloseInterval = 2f;
    public float maxDoorCloseInterval = 0.3f;

    private int horrorDoorsCounter = 0;

    public AudioClip melody;
    public float melodyDelay = 1.5f;

    private bool isDone = false;

    public override void StartTheShocker()
    {
        if (!isDone)
        {
            StartCoroutine("CrazyHorrorMelody");
            StartCoroutine("CrazyHorror");
            isDone = true;
        }
    }

    private void DoorGoCrazy(Interaction _door)
    {
        _door.GetComponent<AudioSource>().clip = doorCloseSound;
        oldSoundMinDist.Add(_door.GetComponent<AudioSource>().minDistance);
        _door.GetComponent<AudioSource>().minDistance = soundMinDist;
        _door.GetComponent<Animation>()["fermeture"].speed = doorSpeed;

        _door.GetComponent<Animation>().Play("fermeture");
        _door.GetComponent<AudioSource>().Play();

        _door.keyValue.keyState = KeyState.None;
        _door.activeAtStoryState = StoryStates.None;
        if( !_door.interactDone)
            _door.interactDone = true;
    }

    private IEnumerator CrazyHorrorMelody()
    {

        yield return new WaitForSeconds(melodyDelay);

        if (melody)
        {
            Game.handler.musicManager.PlayShockingClip(melody);

        }

    }

    private IEnumerator CrazyHorror()
    {

        while (horrorDoors.Count > horrorDoorsCounter)
        {
            DoorGoCrazy(horrorDoors[horrorDoorsCounter]);

            yield return new WaitForSeconds(doorCloseInterval);

            if(doorCloseInterval > maxDoorCloseInterval)
                doorCloseInterval = doorCloseInterval  * 0.7f;
            horrorDoorsCounter++;

            yield return null;
        }

        Finish();
    }

    private void Finish()
    {
        foreach (Interaction _in in horrorDoors)
        {
            _in.GetComponent<AudioSource>().minDistance = oldSoundMinDist[horrorDoors.IndexOf(_in)];
            _in.interactDone = false;

        }
    }
}
