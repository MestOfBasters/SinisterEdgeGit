using UnityEngine;
using System.Collections;

public class PlayerSoundHandler : MonoBehaviour {

    public AudioClip[] scaryBackgrounds;
    public AudioClip[] danger;
    private AudioSource audio;
	// Use this for initialization
	void Start ()
	{
	    audio = GetComponent<AudioSource>();


	}

    //public void temp_shock()
    //{
    //    audio.clip = danger[0];
    //    audio.Play();
    //}
	// Update is called once per frame
	void Update () {
	
	}
}
