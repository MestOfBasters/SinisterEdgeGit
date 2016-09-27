using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;
using System.Collections;

//  This script will be updated in Part 2 of this 2 part series.
public class TestModal : MonoBehaviour {
	private ModalPanel modalPanel;

//	private UnityAction myYesAction;
//	private UnityAction myNoAction;
//	private UnityAction myCancelAction;

	void Awake () {
		modalPanel = ModalPanel.Instance ();

//		myYesAction = new UnityAction (TestYesFunction);
//		myNoAction = new UnityAction (TestNoFunction);
//		myCancelAction = new UnityAction (TestCancelFunction);
	}

	//  Send to the Modal Panel to set up the Buttons and Functions to call
	public void TestYNC () {
		modalPanel.MasterChoise("Do you like stick fish?",TestYesFunction, TestNoFunction, TestCancelFunction);
//		modalPanel.Choice ("Do you want to spawn this sphere?", TestYesFunction, TestNoFunction, TestCancelFunction);
		//      modalPanel.Choice ("Would you like a poke in the eye?\nHow about with a sharp stick?", myYesAction, myNoAction, myCancelAction);
	}

	public void TestYN () {
		modalPanel.MasterChoise("Do you like stick fish?",TestYesFunction, TestNoFunction);
//		modalPanel.Choice ("Do you want to spawn this sphere?", TestYesFunction, TestNoFunction);
	}

	public void TestY () {
		modalPanel.MasterChoise("Do you like stick fish?",TestYesFunction);
//		modalPanel.Choice ("Do you want to spawn this sphere?", TestYesFunction);
	}

	//  Send to the Modal Panel to set up the Buttons and Functions to call
	public void TestYNCI () {
		modalPanel.MasterInfo("Do you like stick A?","A", "B","C");
		//		modalPanel.Choice ("Do you want to spawn this sphere?", TestYesFunction, TestNoFunction, TestCancelFunction);
		//      modalPanel.Choice ("Would you like a poke in the eye?\nHow about with a sharp stick?", myYesAction, myNoAction, myCancelAction);
	}

	public void TestYNI () {
		modalPanel.MasterInfo("Do you like stick A?","A", "B");
		//		modalPanel.Choice ("Do you want to spawn this sphere?", TestYesFunction, TestNoFunction);
	}

	public void TestYI () {
		modalPanel.MasterInfo("Do you like stick A?","A");
		//		modalPanel.Choice ("Do you want to spawn this sphere?", TestYesFunction);
	}


	//  Send to the Modal Panel to set up the Buttons and Functions to call
	public void TestYNCD () {
		modalPanel.MasterDialog("Do you like stick A?",new string[]{"A", "B", "C"},TestYesFunction, TestNoFunction, TestCancelFunction);
		//		modalPanel.Choice ("Do you want to spawn this sphere?", TestYesFunction, TestNoFunction, TestCancelFunction);
		//      modalPanel.Choice ("Would you like a poke in the eye?\nHow about with a sharp stick?", myYesAction, myNoAction, myCancelAction);
	}

	public void TestYND () {
		modalPanel.MasterDialog("Do you like stick A?",new string[]{"A", "B"},TestYesFunction, TestNoFunction);
		//		modalPanel.Choice ("Do you want to spawn this sphere?", TestYesFunction, TestNoFunction);
	}

	public void TestYD () {
		modalPanel.MasterDialog("Do you like stick A?",new string[]{"A"},TestYesFunction);
		//		modalPanel.Choice ("Do you want to spawn this sphere?", TestYesFunction);
	}

	//  These are wrapped into UnityActions
	void TestYesFunction () {
//		displayManager.DisplayMessage ("Heck yeah! Yup!");
		Debug.Log("Heck yeah! Yup!");
	}

	void TestNoFunction () {
//		displayManager.DisplayMessage ("No way, José!");
		Debug.Log("Heck No! Nope!");
	}

	void TestCancelFunction () {
//		displayManager.DisplayMessage ("I give up!");
		Debug.Log("GetTheFuckOUT");
	}
}