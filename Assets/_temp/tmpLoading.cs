using UnityEngine;
using System.Collections;

public class tmpLoading : MonoBehaviour {

    private GameMasterScript master;

    public int loadTestLevel = 3;

    public float waitForAuto = 6f;

    void Awake()
    {
        master = GameObject.FindGameObjectWithTag("Gamemaster").GetComponent<GameMasterScript>();
    }
    void Start()
    {
        master.playerDefeated = true;
        //master.currentLevel = loadTestLevel;

        StartCoroutine("NextLevel");
    }

    
    private IEnumerator NextLevel()
    {

        yield return new WaitForSeconds(waitForAuto);

        Application.LoadLevel("Loading");


    }

    
}
