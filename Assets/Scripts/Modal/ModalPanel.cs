using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

//  This script will be updated in Part 2 of this 2 part series.
public class ModalPanel : MonoBehaviour {

	public Text question;
	public Image iconImage;
	public Button yesButton;
	public Button noButton;
	public Button cancelButton;
	public GameObject modalPanelObject;

	private static ModalPanel modalPanel;

	public static ModalPanel Instance () {
		if (!modalPanel) {
			modalPanel = FindObjectOfType(typeof (ModalPanel)) as ModalPanel;
			if (!modalPanel)
				Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
		}

		return modalPanel;
	}

	// Yes/No/Cancel: A string, a Yes event, a No event and Cancel event
	public void Choice (string question, UnityAction yesEvent, UnityAction noEvent, UnityAction cancelEvent) {
		modalPanelObject.SetActive (true);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);

		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener (noEvent);
		noButton.onClick.AddListener (ClosePanel);

		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener (cancelEvent);
		cancelButton.onClick.AddListener (ClosePanel);

		this.question.text = question;

		this.iconImage.gameObject.SetActive (false);
		yesButton.gameObject.SetActive (true);
		noButton.gameObject.SetActive (true);
		cancelButton.gameObject.SetActive (true);
	}

	public void Choice (string question, UnityAction yesEvent, UnityAction noEvent) {
		modalPanelObject.SetActive (true);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);

		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener (noEvent);
		noButton.onClick.AddListener (ClosePanel);

		cancelButton.onClick.RemoveAllListeners();
//		cancelButton.onClick.AddListener (cancelEvent);
//		cancelButton.onClick.AddListener (ClosePanel);

		this.question.text = question;

		this.iconImage.gameObject.SetActive (false);
		yesButton.gameObject.SetActive (true);
		noButton.gameObject.SetActive (true);
		cancelButton.gameObject.SetActive (false);
	}

	public void Choice (string question, UnityAction yesEvent) {
		modalPanelObject.SetActive (true);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);

		noButton.onClick.RemoveAllListeners();
//		noButton.onClick.AddListener (noEvent);
//		noButton.onClick.AddListener (ClosePanel);

		cancelButton.onClick.RemoveAllListeners();
//		cancelButton.onClick.AddListener (cancelEvent);
//		cancelButton.onClick.AddListener (ClosePanel);

		this.question.text = question;

		this.iconImage.gameObject.SetActive (false);
		yesButton.gameObject.SetActive (true);
		noButton.gameObject.SetActive (false);
		cancelButton.gameObject.SetActive (false);
	}

	public void MasterInfo (string question, params string[] _events) {
		modalPanelObject.SetActive (true);

		AddListeneres(_events);

		this.question.text = question;

		this.iconImage.gameObject.SetActive (false);


	}

	public void MasterChoise (string question, params UnityAction[] _events) {
		modalPanelObject.SetActive (true);

		AddListeneres(_events);

		this.question.text = question;

		this.iconImage.gameObject.SetActive (false);


	}

	public void MasterDialog (string question, string[] _texts,params UnityAction[] _events) {
		modalPanelObject.SetActive (true);

		AddListeneres(_events,_texts);

		this.question.text = question;

		this.iconImage.gameObject.SetActive (false);


	}




	void ClosePanel () {
		modalPanelObject.SetActive (false);
	}

	void AddListeneres(UnityAction[] list){

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (list[0]);
		yesButton.onClick.AddListener (ClosePanel);
		yesButton.gameObject.SetActive (true);

		noButton.onClick.RemoveAllListeners();
		cancelButton.onClick.RemoveAllListeners();

		if(list.Length > 1){
				noButton.onClick.AddListener (list[1]);
				noButton.onClick.AddListener (ClosePanel);
				noButton.gameObject.SetActive (true);
		}else{
			noButton.gameObject.SetActive (false);
		}

		if(list.Length > 2){
			cancelButton.onClick.AddListener (list[2]);
			cancelButton.onClick.AddListener (ClosePanel);
			cancelButton.gameObject.SetActive (true);

		}else{
			cancelButton.gameObject.SetActive (false);
		}
				
	}

	void AddListeneres(string[] list){

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (ClosePanel);
		yesButton.GetComponentInChildren<Text>().text = list[0];
		yesButton.gameObject.SetActive (true);

		noButton.onClick.RemoveAllListeners();
		cancelButton.onClick.RemoveAllListeners();

		if(list.Length > 1){
			noButton.onClick.AddListener (ClosePanel);
			noButton.GetComponentInChildren<Text>().text = list[1];
			noButton.gameObject.SetActive (true);
		}else{
			noButton.gameObject.SetActive (false);
		}

		if(list.Length > 2){
			cancelButton.onClick.AddListener (ClosePanel);
			cancelButton.GetComponentInChildren<Text>().text = list[2];
			cancelButton.gameObject.SetActive (true);

		}else{
			cancelButton.gameObject.SetActive (false);
		}

	}

	void AddListeneres(UnityAction[] _listE, string[] _listT){

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (ClosePanel);
		yesButton.onClick.AddListener (_listE[0]);
		yesButton.GetComponentInChildren<Text>().text = _listT[0];
		yesButton.gameObject.SetActive (true);

		noButton.onClick.RemoveAllListeners();
		cancelButton.onClick.RemoveAllListeners();

		if(_listE.Length == 1 && _listT.Length == 1){
			noButton.gameObject.SetActive (false);
		}else {
			noButton.onClick.AddListener (ClosePanel);

			noButton.gameObject.SetActive (true);

			if(_listE.Length > 1){
				noButton.onClick.AddListener (_listE[1]);
			}

			if(_listT.Length > 1){
				noButton.GetComponentInChildren<Text>().text = _listT[1];
			}


		}
			

		if((_listE.Length == 2 && _listT.Length == 2) || (_listE.Length == 1 && _listT.Length == 1)){
			cancelButton.gameObject.SetActive (false);
		}else {
			cancelButton.onClick.AddListener (ClosePanel);
			
			cancelButton.gameObject.SetActive (true);

			if(_listE.Length > 2){
				cancelButton.onClick.AddListener (_listE[2]);
			}

			if(_listT.Length > 2){
				cancelButton.GetComponentInChildren<Text>().text = _listT[2];
			}


		}

	}
}