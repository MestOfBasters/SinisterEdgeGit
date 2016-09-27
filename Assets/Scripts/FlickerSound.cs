using UnityEngine;
using System.Collections;

public class FlickerSound : MonoBehaviour
{

    private AudioSource audio;
    private GameObject player;

    [HideInInspector]
    public JumpScareTrigger tmpTrig;

    public AudioClip[] shortNoise;
    public AudioClip scareJump;
	// Use this for initialization
	void Start ()
	{
        
	    audio = GetComponent<AudioSource>();
	}

    public void PlayFlickerAudio()
    {
        int i = Random.Range(0, shortNoise.Length);
        audio.clip = shortNoise[i];
        audio.Play();
    }

    
	// Update is called once per frame
	void Update () {
        
	}

    public void PlayJSAudio()
    {
        scareJump = tmpTrig.audioScare;
        audio.clip = scareJump;
        audio.Play();
    }
}
