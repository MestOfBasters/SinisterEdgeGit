using UnityEngine;
using System.Collections;

public class OnDisableSave : MonoBehaviour {

	private bool saveMe = true;

	public string saveDataName;

	void OnDisable () {


		if (saveDataName == "" || !saveMe)
			return;


		Game.handler.SaveMyGame (saveDataName);

		saveMe = false;
	}


}
