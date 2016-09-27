using UnityEngine;
using System.Collections;

public class PendulumAudio : MonoBehaviour {

    

    public void PlayPendulum()
    {

        this.GetComponents<AudioSource>()[1].Play();

    }
}
