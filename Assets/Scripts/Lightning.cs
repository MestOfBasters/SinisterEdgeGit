using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{

    public AudioClip[] lightning;
    private AudioSource audio;
   
    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }
	// Use this for initialization
    public void PlayLightningSound()
    {
        audio.PlayOneShot(lightning[Random.Range(0, lightning.Length)]);
    }

}
