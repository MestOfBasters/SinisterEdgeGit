using UnityEngine;
using System.Collections;

public class ShockerHandler : MonoBehaviour {

	public bool shockDone = false;
	public AudioClip shockSound;

	public Transform dirChild;

	public float shockDelay = 1f;

	public float stayDelay = 1.5f;

	public string animToPlay = "";
    
	public bool destroyAfter = true;

	public bool justStay = false;

	void Start (){
        
		if (!dirChild)
			dirChild = this.transform.GetChild (0).transform;

		if(justStay)
			StartCoroutine ("ShockCoStay");
	}

	public void ShockMe (){

		shockDone = true;

		StartCoroutine ("ShockCo");
        
	}

	private IEnumerator ShockCoStay (){


		yield return new WaitForSeconds(15f);


		shockDone = true;

		if (destroyAfter) {
			this.gameObject.SetActive(false);
		}
	}

	private IEnumerator ShockCo (){

		if(justStay)
			StopCoroutine ("ShockCoStay");

		Game.handler.musicManager.PlayShockingClip (shockSound);

		if(shockDelay > 0f)
			yield return new WaitForSeconds(shockDelay);



//		Game.player.GetComponent<PlayerController>().animGrain.SetTrigger("Grain");
		Game.player.GetComponent<PlayerHealth>().Health = -70f;

		if(stayDelay > 0f)
			yield return new WaitForSeconds(stayDelay);

		if (this.GetComponent<Animation> ()) {

			if(animToPlay != "") this.GetComponent<Animation> ().Play (animToPlay);
			else this.GetComponent<Animation> ().Play ();

		}

		if (destroyAfter) {
			this.gameObject.SetActive(false);
		}
	}

}
