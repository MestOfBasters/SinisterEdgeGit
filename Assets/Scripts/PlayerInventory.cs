using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

    public List<string> collectedPictures = new List<string>();


    public bool hasKey = false;

	public Transform myKeys;
    public Transform myPics;

	private string oldKey = "";

	public List<DeactivateTrigger> currEndTrigger = new List<DeactivateTrigger>();
    //public Image keyPic;

    void Start()
    {


    }
    void OnLevelWasLoaded(int level)
    {
        if(currEndTrigger.Count > 0)
            currEndTrigger = new List<DeactivateTrigger>();

    }

    public void ChangeKey(bool _hasOne, float _delay)
    {

        if (_delay > 0f)
        {
            StartCoroutine(WaitForKey(_delay,_hasOne));
            return;
        }

		hasKey = _hasOne;
		if (_hasOne)
        {
            
            
            //keyPic.enabled = true;
			
			Game.master.keysToKill++;
			StartCoroutine("SeeKey");

            if (Game.handler.storyState == StoryStates.HouseThree)
            {
                if (!IAPManager.hasFullVersion)
                {
                    GameObject.FindGameObjectWithTag("DemoEnd").GetComponent<Animation>().Play();
                    return;
                }

            }
            Game.handler.NewstoryState();
        }
        else
        {
            //keyPic.enabled = false;
			if(oldKey.Contains("key") || oldKey.Contains("Key"))
            	Game.handler.musicManager.PlayUsedKeyClip();
        }
    }

    private IEnumerator WaitForKey(float _wait, bool _key)
    {

        yield return new WaitForSeconds(_wait);

        ChangeKey(_key, 0f);
    }

    private IEnumerator SeeKey () {

		yield return new WaitForSeconds (1f);
		

		myKeys.GetComponent<Animation>().Play();
		if(myKeys.GetComponent<AudioSource>().clip)
			myKeys.GetComponent<AudioSource>().Play();

		if (myKeys.transform.childCount == 0)
			StopCoroutine("SeeKey");

		myKeys.transform.GetChild (0).gameObject.SetActive (true);

		oldKey = myKeys.transform.GetChild (0).gameObject.name;

        Game.handler.musicManager.PlayItemRecievedClip();

		yield return new WaitForSeconds (myKeys.GetComponent<Animation>().clip.length);

//		Destroy (myKeys.transform.GetChild (0).gameObject);


		myKeys.transform.GetChild (0).gameObject.SetActive (false);
		myKeys.transform.GetChild (0).parent = null;
	}

    public void AddPicture(string _picName)
    {

        StartCoroutine(SeePic((int.Parse(_picName)) - 1));

        if (!collectedPictures.Contains("Drawing"+_picName))
        {

            
            

            collectedPictures.Add("Drawing" + _picName);

        }

        
    }

    private IEnumerator SeePic(int _nmbr)
    {

        yield return new WaitForSeconds(0.5f);


        Game.handler.musicManager.PlayDrawingFoundClip();
        myPics.GetComponent<Animation>().Play();
        if (myPics.transform.childCount == 0)
            StopCoroutine("SeePic");

        myPics.transform.GetChild(_nmbr).gameObject.SetActive(true);


        yield return new WaitForSeconds(myPics.GetComponent<Animation>().clip.length);

        myPics.transform.GetChild(_nmbr).gameObject.SetActive(false);

    }

	public void DeactivateAllTrigger (){

		foreach (DeactivateTrigger _dTrig in currEndTrigger)
			_dTrig.playerIsInMe = true;
	}

    
	public void KillKeys(int _kill) {

		
		for(int i=0;i<_kill;i++){
			if(myKeys.transform.childCount > 0)
				myKeys.transform.GetChild (0).parent = null;
//			myKeys.transform.GetChild (0).gameObject.SetActive (false);
		}

	}
}
