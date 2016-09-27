using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
public class AudioZoneTrigger : MonoBehaviour
{

    public AudioMixerSnapshot zone;
    public float time;
    public AudioClip ambientToPlay;
    private AudioSource ambientAudioSource;
    private AudioMixer audioMix;
    public List<GameObject> connectedElement = new List<GameObject>();
    private VisibleHandler visibleHandler;
	public bool trackAudio = true;

	public bool forestSpecial = false;
	// Use this for initialization
	void Start ()
	{
        if (ambientToPlay != null)
	        ambientAudioSource = GameObject.FindGameObjectWithTag("AmbientManager").GetComponent<AudioSource>();

		if(forestSpecial)
			return;

	    visibleHandler = GetComponentInParent<VisibleHandler>();
	}


    public void AddToHandler(List<GameObject> connected)
    {
        if (connectedElement.Count == 0)
            return;

        foreach (GameObject go in connected)
        {
            visibleHandler.toEnable.Add(go);
        }
    }

    public void RemoveFromHandler(List<GameObject> connected)
    {
        if (connectedElement.Count == 0)
            return;
        
        foreach (GameObject go in connected)
        {
            visibleHandler.toDisable.Add(go);
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (other == Game.player.gameObject.GetComponent<CapsuleCollider>())
            {
//                Debug.Log("Player has entered " + gameObject.name);
                //sendconnected to handler
				if(!forestSpecial){
	                AddToHandler(connectedElement);
	                visibleHandler.ChangeTheVisables(true,false);
				}
				if(!trackAudio)
					return;
                if (ambientToPlay)
                {
                    if (ambientAudioSource.clip != ambientToPlay){
                        ambientAudioSource.clip = ambientToPlay;
                            if (!ambientAudioSource.isPlaying)
                                ambientAudioSource.Play();
                        
                    }
                } 

                zone.TransitionTo(time);
//                Debug.Log("changed audio zone to:" + zone.ToString()); 
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {

		if(forestSpecial)
			return;
        if (other.CompareTag("Player"))
        {
            if (other == Game.player.gameObject.GetComponent<CapsuleCollider>())
            {
//                Debug.Log("Player has left "+gameObject.name);
                RemoveFromHandler(connectedElement);
                visibleHandler.ChangeTheVisables(false,true);

            }


        }
    }


}
