using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class FootstepHandler : MonoBehaviour
{
    public float myVelocity = 2f;

    private AudioSource audio;
    private NavMeshAgent navAgent;
    public AudioClip[] forestClips;
    public AudioClip[] concreteClips;
    public material ground;
    public enum material 
    {
        indoor, outdoor
    }
	// Use this for initialization
	void Start ()
	{

	    audio = this.GetComponent<AudioSource>();
	    navAgent = GetComponentInParent<NavMeshAgent>();

        
	}
	
	// Update is called once per frame
	void Update () {
        if (navAgent.velocity.magnitude > myVelocity && !audio.isPlaying)
	    {
	        audio.volume = Random.Range(0.1f, .2f);
            audio.pitch = Random.Range(0.8f, 1.1f);
            
            if(ground == material.indoor)
                audio.clip = concreteClips[Random.Range(0, concreteClips.Length)];
            if (ground == material.outdoor)
                audio.clip = forestClips[Random.Range(0, forestClips.Length)];
            
            audio.Play();
	    }
	}
}
