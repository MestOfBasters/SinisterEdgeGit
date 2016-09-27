using UnityEngine;
using System.Collections;

public class DrawingSoundHandler : MonoBehaviour
{

    public AudioClip[] DrawingsAudioClips;
    public AudioSource audio;
	// Use this for initialization
	void Start ()
	{
	    audio = GetComponent<AudioSource>();
	}

    public void PlayDrawingUnfoldSound()
    {
//        audio.PlayOneShot(DrawingsAudioClips[Random.Range(0, DrawingsAudioClips.Length)]);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
