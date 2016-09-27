using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControllerSense : MonoBehaviour {

	public GameObject activate;

	bool displayable = true;

	public PlayerController senseRef;

	Slider mySlide;

	GameMasterScript myMaster;

	public bool menuDisplay = false;

	void Start () {
	
		activate.SetActive(false);
		myMaster = GameObject.FindGameObjectWithTag("Gamemaster").GetComponent<GameMasterScript>();

		if(myMaster.virtualReality)
			displayable = false;


		mySlide = GetComponent<Slider>();




		if(Application.loadedLevel == 0){
			displayable = true;

			ChangeDisplayable();

		}

		if(!displayable) return;

		if(senseRef)
			mySlide.value = PlayerPrefs.GetFloat("ctrlSense");
		else{

			if(PlayerPrefs.GetFloat("ctrlSense")!=null && PlayerPrefs.GetFloat("ctrlSense")!= 0f)
				mySlide.value = PlayerPrefs.GetFloat("ctrlSense");
			else{

				mySlide.value = 2.6f;

			}

		}


	}

	public void GotMyTrigger(){

		if(!displayable) return;
		activate.SetActive(true);
	}

	public void ChangeDisplayable(){
		if(!displayable) return;
		activate.SetActive(menuDisplay);
		menuDisplay = !menuDisplay;

	}

	public void OkPressed(){
	
		PlayerPrefs.SetFloat("ctrlSense",mySlide.value);


		activate.SetActive(false);

		menuDisplay = true;
	}

	public void UpdateSense(){

		if(senseRef)
			senseRef.rotBoost = mySlide.value;
		else
			PlayerPrefs.SetFloat("ctrlSense",mySlide.value);
	}
	

}
