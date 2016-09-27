using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioSpawn {


    public AudioClip sampleAudio;
    public SpawnPosition spawnPosition;
    [Range(2f, 10f)]
    public float spawnDistance = 3f;
    public AudioMoveType sampleMove;
    public float moveSpeed = 1.2f;

    public bool DoMyAudio()
    {
        Game.handler.playerAudioSpawn.SpawnPos(sampleAudio, spawnPosition, spawnDistance, sampleMove, moveSpeed);

        return true;
    }
}
