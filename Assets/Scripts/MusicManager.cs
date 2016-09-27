using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioClip DrawingFoundClip;
	public AudioClip horrorAmbient;

	public AudioClip solvedTheMech;

    public AudioClip usedKey;

    public AudioClip itemRecieved;

    private AudioSource audioInteract;
	private AudioSource audioShock;
	// Use this for initialization
	void Start ()
	{
		audioInteract = GetComponents<AudioSource>()[0];
		audioShock = GetComponents<AudioSource>()[1];
	}

    public void PlayDrawingFoundClip()
    {
		audioInteract.PlayOneShot(DrawingFoundClip);
    }

	public void PlayHorrorAmbientClip()
	{
		audioShock.PlayOneShot(horrorAmbient);
	}

	public void PlaySolvedTheMechClip()
	{
		audioInteract.PlayOneShot(solvedTheMech);
	}

	public void PlayShockingClip(AudioClip _shockClip)
	{
        
		audioShock.PlayOneShot(_shockClip);
	}

    public void PlayUsedKeyClip()
    {
		audioInteract.PlayOneShot(usedKey);
    }

    public void PlayItemRecievedClip()
    {
		audioInteract.PlayOneShot(itemRecieved);
    }
}
