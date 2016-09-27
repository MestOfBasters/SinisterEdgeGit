using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameMasterScript : MonoBehaviour {

    public int beginAt = 2;

    public static GameMasterScript control;

    private GameObject plyr;
    
    public int currentLevel;


	public bool canContinue;


	public bool playerDefeated = false;

	public bool virtualReality = false;

	public bool controllerSupport = false;

    private bool dontUseFile = false;

	private StoryStates loadStory;

	[HideInInspector]
	public int keysToKill = 0;
	[HideInInspector]
	public bool hasKeySave = false;
	[HideInInspector]
	public string currentPart;

	[HideInInspector]
	public int lastLevel;

	public List<Interaction> iActsToSave 	= new List<Interaction>();
	public List<TriggerScript> trigToSave 	= new List<TriggerScript>();
	
	public void AddToSave (Interaction _inA){

		if(!iActsToSave.Contains(_inA)){
//			if(_inA.GetComponent<TriggerScript>())
//				if(!trigToSave.Contains(_inA.GetComponent<TriggerScript>()))
//					trigToSave.Add(_inA.GetComponent<TriggerScript>());

			iActsToSave.Add(_inA);

		}
	}

	public void AddToSave (TriggerScript _trig){

		if(!trigToSave.Contains(_trig))
			trigToSave.Add(_trig);
	}

	void Awake () {

        //Debug.Log ("Try to Awake");
        if (control == null)
        {
        DontDestroyOnLoad(gameObject);
            //Debug.Log ("Me");
            control = this;
        }
        else if (control != this)
        {
            //Debug.Log ("NotMe");
            Destroy(this.gameObject);
            return;
        }

        currentLevel = 0;


        LoadPrio();
        

	}


	void OnLevelWasLoaded(int level) {


		if(level != 2){
			
			lastLevel = level;
		}

        if (level == 0)
        {
        	LoadPrio();

			return;
		}



        if(!GameObject.FindGameObjectWithTag("Player"))
            return;

        plyr = GameObject.FindGameObjectWithTag("Player");


        //Camera Adjust, wenn VR / Nicht VR
		if (virtualReality) {
			
            if(!plyr.GetComponent<Cardboard>().VRModeEnabled)
			    plyr.GetComponent<Cardboard>().VRModeEnabled = true;

            if (level > 2)
            {
                if (plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.activeSelf)
                    plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);

                if (!plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).gameObject.activeSelf)
                    plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(true);
            }
        }else{

             if(plyr.GetComponent<Cardboard>().VRModeEnabled)
			    plyr.GetComponent<Cardboard>().VRModeEnabled = false;


             if (level > 2)
             {
                 if(!plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.activeSelf)
                    plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);

                 if(plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).gameObject.activeSelf)
                    plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(false);
             }
        }


        
		


        if (level < 3)
            return;


        if (controllerSupport)
        {
            plyr.GetComponent<PlayerController>().remoteControll = true;

            if (!virtualReality)
            {
                plyr.GetComponent<PlayerController>().hardRemoteC = true;
                plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<CardboardHead>().trackRotation = false;
                plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<CardboardHead>().trackPosition = false;

            }
            else
            {
                plyr.GetComponent<PlayerController>().hardRemoteC = false;
                plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<CardboardHead>().trackRotation = true;
                plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<CardboardHead>().trackPosition = true;
                

            }

            //disable
            plyr.GetComponent<PlayerController>().virtualControl = false;

            if (plyr.GetComponent<PlayerController>().virtController)
                plyr.GetComponent<PlayerController>().virtController.SetActive(false);
        }
        else
        {
            plyr.GetComponent<PlayerController>().remoteControll = false;

            if (virtualReality)
            {
                plyr.GetComponent<PlayerController>().hardRemoteC = false;
                plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<CardboardHead>().trackRotation = true;
                plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<CardboardHead>().trackPosition = true;

                //disable
                plyr.GetComponent<PlayerController>().virtualControl = false;

                if (plyr.GetComponent<PlayerController>().virtController)
                    plyr.GetComponent<PlayerController>().virtController.SetActive(false);

            }
            else
            {

                plyr.GetComponent<PlayerController>().remoteControll = true;

                plyr.GetComponent<PlayerController>().hardRemoteC = true;
                plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<CardboardHead>().trackRotation = false;
                plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<CardboardHead>().trackPosition = false;

                //enable
                plyr.GetComponent<PlayerController>().virtualControl = true;

                if (plyr.GetComponent<PlayerController>().virtController)
                    plyr.GetComponent<PlayerController>().virtController.SetActive(true);
            }

        }

		this.GetComponent<PlayerInventory>().myKeys = plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).FindChild("TheKeys").transform;
		
		this.GetComponent<PlayerInventory>().myPics = plyr.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).FindChild("ThePics").transform;



		//SetTheLoadStuff
		if(canContinue){
			LoadGame();
			Game.handler.storyState = loadStory;
			if(keysToKill>0){
				this.GetComponent<PlayerInventory>().KillKeys(keysToKill);
				this.GetComponent<PlayerInventory>().hasKey = hasKeySave;
			}


			canContinue = false;
		}
	}



	void Update () {

		if (Application.loadedLevel > 2) {

			if(Input.GetKeyDown(KeyCode.Escape))
				Application.LoadLevel("MainMenu");

		}

        if (Application.loadedLevel == 1)
        {
            if (Input.GetButtonDown("A") || Input.GetMouseButtonDown(0) || Cardboard.SDK.Triggered)
            {
                SavePrio(2);
                Application.LoadLevel("Loading");
            }
        }

	}

    public void SavePrio(int _lvl)
    {
        if (dontUseFile)
        {
            
            return;

        }

//		Debug.Log ("Saved");
		//create BF
        BinaryFormatter bf = new BinaryFormatter();

        //create File
        FileStream file = File.Create(Application.persistentDataPath + "/SinGameInfo.dat");

        //create Container
        PrioGameData data = new PrioGameData();

        ////assign Container
        data.currentLevel = _lvl;
		data.currentPart = currentPart;
        data.virtualReality = virtualReality;
        data.controllerSupport = controllerSupport;

		if(GameObject.FindGameObjectWithTag("Gamehandler")){

			data.keysToKill = keysToKill;
			data.hasKeySave = Game.handler.playerInventory.hasKey;
			data.storyToLoad = Game.handler.storyState;

		}

//		Debug.Log (virtualReality);
        //write Container to file
        bf.Serialize(file, data);

        //close file
        file.Close();
    }

    public void LoadPrio()
    {
        if (dontUseFile)
            return;

        if (File.Exists(Application.persistentDataPath + "/SinGameInfo.dat"))
        {

            //create BF
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/SinGameInfo.dat", FileMode.Open);

            PrioGameData data = (PrioGameData)bf.Deserialize(file);


            currentLevel = data.currentLevel;
            virtualReality = data.virtualReality;
            controllerSupport = data.controllerSupport;
			currentPart = data.currentPart;


			loadStory = data.storyToLoad;
			keysToKill = data.keysToKill;
			hasKeySave = data.hasKeySave;

			file.Close();
//			Debug.Log ("Loaded");
        }
        
        

		if (File.Exists(Application.persistentDataPath + "/SinSaveGame.dat"))
		{
			canContinue = true;
		}

    }

	public void SaveGame()
	{
		if (dontUseFile)
		{
			
			return;
			
		}
		
		//		Debug.Log ("Saved");
		//create BF
		BinaryFormatter bf = new BinaryFormatter();
		
		//create File
		FileStream file = File.Create(Application.persistentDataPath + "/SinSaveGame.dat");
		
		//create Container
		TheSaveGame data = new TheSaveGame();

		data.iActs = new List<ObjSave>();
		data.trigs = new List<ObjSave>();

		foreach (Interaction _act in iActsToSave){
			data.iActs.Add(new ObjSave(Game.handler.iActLib.IndexOf(_act), _act.interactDone, _act.gameObject.activeInHierarchy));
		}

		foreach (TriggerScript _trig in trigToSave){
			data.trigs.Add(new ObjSave(Game.handler.trigLib.IndexOf(_trig), _trig.triggerDone, false));
		}
		
		//		Debug.Log (virtualReality);
		//write Container to file
		bf.Serialize(file, data);
		
		//close file
		file.Close();
	}
	
	public void LoadGame()
	{
		if (dontUseFile)
			return;
		
		if (File.Exists(Application.persistentDataPath + "/SinSaveGame.dat"))
		{

			iActsToSave.Clear();
			trigToSave.Clear ();  
			//create BF
			BinaryFormatter bf = new BinaryFormatter();
			
			FileStream file = File.Open(Application.persistentDataPath + "/SinSaveGame.dat", FileMode.Open);
			
			TheSaveGame data = (TheSaveGame)bf.Deserialize(file);

			iActsToSave = new List<Interaction>();
			trigToSave = new List<TriggerScript>();


			foreach (ObjSave _trig in data.trigs){
				
				Game.handler.trigLib[_trig.index].triggerDone = _trig.isDone;

				trigToSave.Add (Game.handler.trigLib[_trig.index]);
			}

			foreach (ObjSave _act in data.iActs){
				

				Game.handler.iActLib[_act.index].interactDone = _act.isDone;
				Game.handler.iActLib[_act.index].gameObject.SetActive(_act.isActive);
				Game.handler.iActLib[_act.index].OnLoadCall();
				iActsToSave.Add (Game.handler.iActLib[_act.index]);
			}


			file.Close();
			//			Debug.Log ("Loaded");


		}
		
		
		
		
	}

    public void Delete() // new Game
    {
		canContinue = false;

        if (dontUseFile)
            return;

		if (File.Exists (Application.persistentDataPath + "/SinGameInfo.dat"))
			File.Delete (Application.persistentDataPath + "/SinGameInfo.dat");

		if (File.Exists (Application.persistentDataPath + "/SinSaveGame.dat"))
			File.Delete (Application.persistentDataPath + "/SinSaveGame.dat");

		iActsToSave.Clear();
		trigToSave.Clear (); 

		loadStory = StoryStates.None;

		keysToKill = 0;
		hasKeySave = false;
		currentPart = "";
    }

    public void NextChapter()
    {
		iActsToSave.Clear();
		trigToSave.Clear ();    
		keysToKill = 0;


        if (Application.loadedLevel > 2)
        {
			currentPart = "Chapter"+Application.loadedLevel;
			SaveGame();
            SavePrio(Application.loadedLevel);
        }
        Application.LoadLevel("Loading");
    }

	public void NewGame (){
        Delete ();
		currentLevel = beginAt;
		Application.LoadLevel("IntroScene");
	}

	public void Continue (){
		if (canContinue) {

			if(loadStory == StoryStates.HouseThree && currentPart == "GotEye"){


				if(IAPManager.hasFullVersion){
					Debug.Log("hasFull");
					loadStory ++;
				}

			}


			Application.LoadLevel ("Loading");
		}
	}

	public void SaveMyGame (string _saveName) {

		currentPart = _saveName;

		SaveGame();
		SavePrio(Application.loadedLevel-1);

	}
}

[Serializable]
class PrioGameData
{
    public int currentLevel;

	public string currentPart;



    public bool virtualReality;

    public bool controllerSupport;

	public StoryStates storyToLoad;

	public int keysToKill;

	public bool hasKeySave;

    // + options

}

[Serializable]
class TheSaveGame
{
	public List<ObjSave> iActs;
	public List<ObjSave> trigs;
	
	// + options
	
}

[Serializable]
class ObjSave
{
	public int index;
	public bool isDone;
	
	public bool isActive;

	public ObjSave (int _index, bool _done, bool _active) {
		index = _index;
		isDone = _done;
		isActive = _active;

	}

	public ObjSave () {}

}


