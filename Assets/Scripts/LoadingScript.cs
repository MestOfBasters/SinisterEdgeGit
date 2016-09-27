using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour {

    private GameMasterScript master;

    public Text loadingInc;
    public Text loadingInfo;

	private int loadingCounter = 0;

    private bool asyncLoading = false;

	public bool showProgressScreen = false;

	public GameObject skull;
	public GameObject progressPanel;
	public Image progressPic;

    void Awake()
    {
        master = GameMasterScript.control;

		if(master.lastLevel == 0){
//			Debug.Log ("show progress");
			showProgressScreen = true;
		}else{
//			Debug.Log ("dont show progress");
			showProgressScreen = false;
			
		}


    }

	void Start () {

		
		skull.SetActive(!showProgressScreen);
		progressPanel.SetActive(showProgressScreen);

		progressPic.sprite = Resources.Load <Sprite> ("Screens/"+master.currentPart);

        if (master.playerDefeated)
            StartCoroutine("DeadPlayer");
        else
            StartCoroutine("NextLevel");

	}

    private IEnumerator DeadPlayer()
    {
        master.playerDefeated = false;

        //loadingInfo.text = "You are dead! - Reloading";
        yield return new WaitForSeconds(1f);

        if(asyncLoading)
            StartCoroutine(AsyncLevelLoading(master.currentLevel));
        else
            Application.LoadLevel(master.currentLevel);





    }
    private IEnumerator NextLevel()
    {
        
        yield return new WaitForSeconds(1f);


        if (master.currentLevel > (Application.levelCount - 1)) //Start 0, Loading 1 , StereoCredits 2, 
            Application.LoadLevel("MainMenu");
        else
            master.currentLevel++;

        if (asyncLoading)
            StartCoroutine(AsyncLevelLoading(master.currentLevel));
        else
            Application.LoadLevel(master.currentLevel);


    }

    private IEnumerator AsyncLevelLoading(int _level)
    {

        //loadingInfo.text += " \n Chapter: " + (master.currentLevel - 2);
        yield return new WaitForSeconds(2f);
        //Application.LoadLevel(_level);

        //AsyncOperation async = Application.LoadLevelAsync(_level);
        //
        //while (!async.isDone)
        //{
        //    //Hier wird geladen!
        //    if(async.progress*5f > loadingCounter){

        //        loadingInc.text += ". ";
        //        loadingCounter++;
        //    }
            
        //    yield return null;
        //}

        Debug.Log("Loading complete");

    }

    
}
