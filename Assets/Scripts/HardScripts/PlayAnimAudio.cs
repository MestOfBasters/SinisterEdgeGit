using UnityEngine;
using System.Collections;

public class PlayAnimAudio : MonoBehaviour {

    public void PlayTheAudio()
    {

        this.GetComponent<AudioSource>().Play();

    }
}
