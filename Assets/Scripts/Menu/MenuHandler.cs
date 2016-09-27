using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using ChartboostSDK;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuHandler : MonoBehaviour
{
	private ModalPanel modalPanel;

    private bool toggleVRBool = false;
	private bool toggleCTRLBool = false;
    public Text vrStatusText;
	public Text ctrlStatusText;
    public Color enabledToggleColor;
    public Color disabledToggleColor;
    public AudioClip selectSound;
    private AudioSource audio;
    public Image blackGroup;
    public Texture2D shareTexture;
    public string facebookPageURL;
    public string gPlusURL;

	public Button continueButton;
	public Text notContBtn;
    public GameObject MoreApps;
    private bool hasMoreApps;
  //  public Image mainmenuGroup;
    
	public Text versionText;

	public Button vrButn,ctrlButn;

	private GameMasterScript master;
    

	void Awake(){
		modalPanel = ModalPanel.Instance ();
	}


	void Start ()
	{
	    audio = GetComponent<AudioSource>();
	    StartCoroutine(UiFadeObject(blackGroup, 2f, 5f, 0f, true));
  //      StartCoroutine(UiFadeObject(blackGroup, 4f, 3f, 1f));
	    //mainmenuGroup.CrossFadeAlpha(0, 5f, false);
	   // InitalizeGooglePlusButton();
//        Chartboost.cacheMoreApps(CBLocation.Default);
//        Chartboost.setAutoCacheAds(true);
		master = GameObject.FindGameObjectWithTag ("Gamemaster").GetComponent<GameMasterScript> ();

		toggleVRBool = master.virtualReality;
		if (master.virtualReality)
		{
			vrStatusText.color = enabledToggleColor;
			vrStatusText.text = " O N";
		}
		else
		{
			vrStatusText.color = disabledToggleColor;
			vrStatusText.text = " O F F";
		}

		toggleCTRLBool = master.controllerSupport;
		if (master.controllerSupport)
		{
			ctrlStatusText.color = enabledToggleColor;
			ctrlStatusText.text = " O N";
		}
		else
		{
			ctrlStatusText.color = disabledToggleColor;
			ctrlStatusText.text = " O F F";
		}

		if(!SystemInfo.supportsAccelerometer && !Application.isEditor){

			master.virtualReality = false;
			toggleVRBool = master.virtualReality;

			vrStatusText.color = disabledToggleColor;
			vrStatusText.text = " O F F";


			master.controllerSupport = true;
			toggleCTRLBool = master.controllerSupport;

			ctrlStatusText.color = enabledToggleColor;
			ctrlStatusText.text = " O N";


			if(vrButn){

				ctrlButn.gameObject.SetActive(false);
				ctrlStatusText.gameObject.SetActive(false);
				vrStatusText.gameObject.SetActive(false);
				vrButn.gameObject.SetActive(false);
			}

		}


		if (master.canContinue) {
			notContBtn.gameObject.SetActive(false);
			
			continueButton.gameObject.SetActive(true);
		}else{
			notContBtn.gameObject.SetActive(true);
			
			continueButton.gameObject.SetActive(false);
		}

		versionText.text = "Version " + Application.version;
	}
    private int frameCount = 0;



    void Update()
    {
//        frameCount++;
//        if (frameCount > 30)
//        {
//            // update these periodically and not every frame
//            hasMoreApps = Chartboost.hasMoreApps(CBLocation.Default);
//            frameCount = 0;
//        }
//        MoreApps.SetActive(hasMoreApps);


//		if(!EventSystem.current.currentSelectedGameObject)
//			EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
//
//		if(Input.GetMouseButtonDown(0)){
//			EventSystem.current.GetComponent<TouchInputModule>().enabled = true;
//			EventSystem.current.GetComponent<StandaloneInputModule>().enabled = false;
//
//			EventSystem.current.GetComponent<TouchInputModule>().forceModuleActive = true;
//			EventSystem.current.GetComponent<StandaloneInputModule>().forceModuleActive = false;
//		}
//
//		if(Input.GetMouseButtonUp(0)){
//			EventSystem.current.GetComponent<TouchInputModule>().enabled = false;
//			EventSystem.current.GetComponent<StandaloneInputModule>().enabled = true;
//
//			EventSystem.current.GetComponent<TouchInputModule>().forceModuleActive = false;
//			EventSystem.current.GetComponent<StandaloneInputModule>().forceModuleActive = true;
//		}


//		if(Input.touchCount > 0)
//			if(Input.GetTouch(0).phase == TouchPhase.Ended)
//				Cursor.lockState = CursorLockMode.Locked;

    }

    void OnEnable()
    {
        SetupDelegates();
    }
    void SetupDelegates()
    {
        // Listen to all impression-related events
    //    Chartboost.didCacheMoreApps += didCacheMoreApps;
        //Debug.Log("Deligates are set up");
#if UNITY_IPHONE
//		Chartboost.didCompleteAppStoreSheetFlow += didCompleteAppStoreSheetFlow;
#endif
    }

//    AN_PlusButton PlusButton;
//    public void InitalizeGooglePlusButton()
//    {
//        
//        string PlusUrl = gPlusURL;
//        PlusButton =  new AN_PlusButton(PlusUrl, AN_PlusBtnSize.SIZE_STANDARD, AN_PlusBtnAnnotation.ANNOTATION_BUBBLE);
//        PlusButton.ButtonClicked += GPlusButtonClicked;
//        PlusButton.SetGravity(TextAnchor.LowerCenter);
//        if(!PlusButton.IsShowed)
//            PlusButton.Show();
//    }
//
////    void didCacheMoreApps(CBLocation location)
////    {
////        //AndroidMessage.Create("more apps cached",string.Format("didCacheMoreApps at location: {0}", location));
////        MoreApps.SetActive(true);
////    }
//    private void GPlusButtonClicked () {
//        AndroidMessage.Create("Thanks for +1", "Thank you for your support!");
//    }

    public IEnumerator UiFadeObject(Image _img, float _delay, float _time, float _alpha, bool _toggleRayCastTarget)
    {
        yield return new WaitForSeconds(_delay);
        _img.CrossFadeAlpha(_alpha, _time, false);
        if (_toggleRayCastTarget)
            _img.raycastTarget = false;
    }
    public void PlayButtonSound()
    {
        audio.clip = selectSound;
        audio.Play();
//        Debug.Log("Play button sound");
    }

    public void MoreGamesPressed()
    {
      //  Chartboost.showMoreApps(CBLocation.Default);
        Debug.Log("tying to show more apps");
    }

    public void NewGamePressed()
    {
		string _vrNode = "";

		if(toggleVRBool)
			_vrNode = "Note: You won't be able to change the volume after this point.";
		else
			_vrNode = "";

		modalPanel.MasterDialog( "- Please plug in your earphones. \n" +
			"- Do you hear the music playing loud?\n" +
			"\n"+
			_vrNode, new string[]{"Yes, im ready", "No, back to menu"},TheNewGame,EmptyVoid);

//        AndroidDialog dialog = AndroidDialog.Create("Checklist", "- Please plug in your earphones. \n" +
//                                                                 "- Do you hear the music playing loud?\n" +
//                                                                 "\n"+
//		                                            			_vrNode, "yes, im ready", "no, back to menu");
        

    }

    public void FacebookButtonPressed()
    {
        Application.OpenURL(facebookPageURL);
    }
//    public void CreditsPressed()
//    {
//        AndroidSocialGate.StartShareIntent("Hello Share Intent", "Sharing your Achivement", shareTexture);
//    }
   

    public void TheContinue()
    {
       // PlusButton.Hide();
        master.Continue();
    }

	public void TheNewGame(){
		//PlusButton.Hide();
		master.NewGame();
	}

	public void EmptyVoid(){

	}
    // Toggle VR
    public void ToggleVR()
    {

        toggleVRBool = !toggleVRBool;
		master.virtualReality = toggleVRBool;
        if (toggleVRBool)
        {
            vrStatusText.color = enabledToggleColor;
            vrStatusText.text = " O N";

			if(!SystemInfo.supportsGyroscope){


				modalPanel.MasterDialog( "Attention \n" +
					"Your device is not fully capable for VR-Mode. \n" +
					"Your VR-experience could be limited.\n" +
					"Missing: gyroscope", new string[]{"Ok", "Change back"},EmptyVoid,ToggleVR);
			}


        }
        else
        {
            vrStatusText.color = disabledToggleColor;
            vrStatusText.text = " O F F";
        }



    }

	// Toggle VR
	public void ToggleCTRL()
	{
		toggleCTRLBool = !toggleCTRLBool;
		master.controllerSupport = toggleCTRLBool;
		if (toggleCTRLBool)
		{
			ctrlStatusText.color = enabledToggleColor;
			ctrlStatusText.text = " O N";
		}
		else
		{
			ctrlStatusText.color = disabledToggleColor;
			ctrlStatusText.text = " O F F";
		}
	}



}
