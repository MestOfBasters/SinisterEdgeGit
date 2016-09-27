using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public enum TheStates { None, Start, Normal, Interaction, End, GameOver };
public enum StoryStates { None = -5, PreForest = -2, Forest = -1, ForestKey = 0, HouseOne = 1, HouseTwo = 2, HouseThree = 3, HouseFour = 4,
HouseFive = 5, HouseSix = 6, HouseSeven = 7, HouseEight = 8, HouseNine = 9, HouseTen = 10, HouseEleven = 11, HouseTwelfe = 12,
HouseThirteen = 13, End = 15
};

static class Game
{

    static public TheStates state = TheStates.None;

    static public GameHandler handler;
    static public GameMasterScript master;

    static public Transform player;
    static public Transform slender;

}


public class GameHandler : MonoBehaviour {

	public List<Interaction> iActLib;

	public List<TriggerScript> trigLib;

    [HideInInspector]
    public PlayerInventory playerInventory;
    [HideInInspector]
    public AudioMove playerAudioSpawn;
    [HideInInspector]
    public MusicManager musicManager;
    [HideInInspector]
    public AmbSoundHandler ambientManager;
	[HideInInspector]
	public DarkScript darknessHandler;

    [HideInInspector]
    public StartState startState;
    [HideInInspector]
    public EndState endState;
    [HideInInspector]
    public NormalState normalState;
    [HideInInspector]
    public InteractState interactState;
    [HideInInspector]
    public GameOverState gameOverState;

    public StoryStates storyState;

    public GameState currentState;

    private bool cheats = false;
	private bool testingCheats = false;

	public Animation saveIconAnim;

	void Awake () {

        Game.player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if(GameObject.FindGameObjectWithTag("Slenderman") != null)
            Game.slender = GameObject.FindGameObjectWithTag("Slenderman").transform;
        
        Game.handler = this;
        if (GameObject.FindGameObjectWithTag("Gamemaster"))
        {
            Game.master = GameObject.FindGameObjectWithTag("Gamemaster").GetComponent<GameMasterScript>();
            playerInventory = GameObject.FindGameObjectWithTag("Gamemaster").GetComponent<PlayerInventory>();

            playerInventory.myKeys = Game.player.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).FindChild("TheKeys").transform;

            playerInventory.myPics = Game.player.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).FindChild("ThePics").transform;
        }
        playerAudioSpawn = Game.player.GetComponentInChildren<AudioMove>();

        musicManager = GameObject.FindWithTag("MusicManager").GetComponent<MusicManager>();
        ambientManager = GameObject.FindWithTag("AmbientManager").GetComponent<AmbSoundHandler>();
        startState = GetComponent<StartState>();
        normalState = GetComponent<NormalState>();
        interactState = GetComponent<InteractState>();
        endState = GetComponent<EndState>();
        gameOverState = GetComponent<GameOverState>();
        

		darknessHandler = Game.player.GetComponentInChildren<DarkScript> ();
	}

    void Start()
    {
        NewGameState(startState);
        
    }

	public void SaveMyGame (string _saveName) {
		if (saveIconAnim)
			saveIconAnim.Play ();


		Game.master.SaveMyGame (_saveName);
	}

    public Transform testTrigger;


	public bool isNormalState () {

		if(currentState == normalState)
			return true;
		else
			return false;

	}
    void Update()
    {
        
        if (currentState != null)
            currentState.StateUpdate();


//		if (!testingCheats)
//			return;
//
//		if (Input.GetButtonDown("B"))
//		{
//			Game.player.GetComponent<PlayerController>().lightCones[1].SetActive(!Game.player.GetComponent<PlayerController>().lightCones[0].activeSelf);
//			
//		}
//
//		if (Input.GetButtonDown("A"))
//		{
//			Game.player.GetComponent<PlayerController>().lightCones[0].SetActive(!Game.player.GetComponent<PlayerController>().lightCones[0].activeSelf);
//
//		}
//
//        if (!cheats)
//            return;
//
//        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("B"))
//        {
//            playerInventory.ChangeKey(true, 0f);
//
//        }
//
//        if (Input.GetKeyDown(KeyCode.H))
//        {
//            playerInventory.AddPicture("1");
//
//        }
//
//        if (Input.GetKeyDown(KeyCode.J))
//        {
//            playerInventory.AddPicture("2");
//
//        }
//
//        if (Input.GetKeyDown(KeyCode.K))
//        {
//            playerInventory.AddPicture("3");
//
//        }
//
//        if (Input.GetKeyDown(KeyCode.N) || Input.GetButtonDown("A"))
//        {
//
//            NewstoryState();
//
//        }
//
//        if (Input.GetKeyDown(KeyCode.U))
//        {
//            NewGameState(gameOverState);
//
//        }
//
//        if (Input.GetKeyDown(KeyCode.T))
//        {
//            if (testTrigger)
//            {
//
//                testTrigger.GetComponent<RepeatTillTrigger>().StartHorrorTrigger();
//            }
//
//        }
//
//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            darknessHandler.MakeItDarkFor(5f);
//
//        }
        
    }

    public void NewGameState(GameState newState)
    {

        if (null != currentState)
        {
            currentState.OnStateExit();
            
        }


        currentState = newState;
        currentState.OnStateEntered();

    }

    public void NewstoryState()
    {

        //"AntiCheatSchutz"

        //if (Game.handler.storyState >= StoryStates.HouseFour)
        //{
        //    if (!IAPManager.hasFullVersion)
        //    {
        //        Application.LoadLevel(0);
        //        return;
        //    }

        //}

		if (storyState == StoryStates.None) {
			storyState = StoryStates.PreForest;
			return;
		}
        if(storyState!= StoryStates.End)
            storyState++;
        

    }
}
