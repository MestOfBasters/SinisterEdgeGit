using UnityEngine;
using System.Collections;

public class OnRemoteUse : MonoBehaviour {

	private bool saveMe = true;
	
	public string saveDataName;
	
	public void OnRemote () {
		
		
		if (saveDataName == "" || !saveMe || saveDataName == Game.master.currentPart)
			return;
		
		
		Game.handler.SaveMyGame (saveDataName);
		
		saveMe = false;
	}

}
