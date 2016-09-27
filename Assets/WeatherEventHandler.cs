using UnityEngine;
using System.Collections;

public class WeatherEventHandler : MonoBehaviour
{
    private Animator lightning;
	// Use this for initialization
	void Start ()
	{
	    lightning = GetComponentInChildren<Animator>();
        InvokeRepeating("LightningSpawn",30f, 30f);
	}

    public void LightningSpawn()
    {
        if (Random.Range(0, 100) < 50)
        {
            lightning.SetTrigger("Lightning");
        }
//        else
//        {
//            Debug.Log("No Lightning");
//        }
    }

}
