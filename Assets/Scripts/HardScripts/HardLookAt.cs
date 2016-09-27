using UnityEngine;
using System.Collections;

public class HardLookAt : MonoBehaviour {

    private Transform theTarget;

    public Vector3 lookRot;

    void Start()
    {
        theTarget = Game.player;
    }

	void Update () {

        transform.LookAt(theTarget);
        transform.Rotate(lookRot);
	}
}
