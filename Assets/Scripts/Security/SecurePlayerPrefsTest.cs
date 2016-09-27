using UnityEngine;
using System.Collections;

public class SecurePlayerPrefsTest : MonoBehaviour
{
    void Start()
    {
        SecurePlayerPrefs.SetString("FullVersion", "registredFullversion", "password");
        PlayerPrefs.Save();


        string helloWorld = SecurePlayerPrefs.GetString("FullVersion", "password");
        Debug.Log(helloWorld);
    }
}