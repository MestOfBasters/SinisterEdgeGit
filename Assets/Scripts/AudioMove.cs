using UnityEngine;
using System.Collections;


public enum SpawnPosition { 
    North   = 2,
    NE      = 1,
    East    = 0,
    SE      = -1,
    South   = -2,
    SW      = -3,
    West    = 4,
    NW      = 3
}

public enum AudioMoveType
{
    None                = 0,
    TowardsPlayer       = 1,
    AwayFromPlayer      = -1,
    RotateLeft          = -2,
    RotateRight         = 2,
    StraightForward     = 3,
    StraightBackward    = -3,
    StraightLeft        = -4,
    StraightRight       = 4
}

public class AudioMove : MonoBehaviour {

    public AnimationCurve spawnRegion;


    public AudioClip sampleAudio;
    public SpawnPosition spawnPosition;
    [Range (2f,10f)]public float spawnDistance = 3f;
    public AudioMoveType sampleMove;
    public float moveSpeed = 1.2f;


    private Transform myHead;
    private AudioSource myAudio;
    private Vector3 myDirection;

	void Start () {

        myHead = Game.player.GetChild(0);
        myAudio = this.GetComponent<AudioSource>();
	}


    public void SpawnPos(AudioClip _audio, SpawnPosition _pos, float _distToPlayer, AudioMoveType _behave, float _speed)
    {
        //this.transform.rotation = myHead.rotation;

        Vector3 newPos = myHead.position +(

            (myHead.forward * spawnRegion.keys[Mathf.Abs((int)_pos)].value) * NumberSign((int)_pos) +

            (myHead.right * spawnRegion.keys[Mathf.Abs((int)_pos)].time)

            ).normalized * _distToPlayer;

        this.transform.position = newPos;

        if (!myAudio.isPlaying && _audio!=null)
        {
            myAudio.clip = _audio;
            myAudio.Play();
        }

        StartCoroutine(AudioPresent(_behave,_speed));

	}

    private IEnumerator AudioPresent(AudioMoveType _behave, float _speed)
    {
        
       

        transform.LookAt(myHead);

        switch (Mathf.Abs((int)_behave))
        {
            case 1:
                myDirection = this.transform.forward * NumberSign((int)_behave);
                break;
            case 3:
                myDirection = myHead.forward * NumberSign((int)_behave);
                break;
            case 4:
                myDirection = myHead.right * NumberSign((int)_behave);
                break;
            default:

                break;
        }


        while (myAudio.isPlaying)
        {
            
            if (_behave == AudioMoveType.RotateLeft || _behave == AudioMoveType.RotateRight)
                transform.RotateAround(myHead.position, Vector3.up, _speed * NumberSign((int)_behave) * 10f * Time.deltaTime);
            else if (_behave != AudioMoveType.None)
                transform.Translate(myDirection * _speed * Time.deltaTime, Space.World);


            yield return null;
        }

        
    }

    private int NumberSign(int _n)
    {
        if (_n != 0)
            return (_n / Mathf.Abs(_n));
        else
            return 1;
    }
}
