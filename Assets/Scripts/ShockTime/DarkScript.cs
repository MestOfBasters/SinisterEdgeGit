using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class DarkScript : MonoBehaviour {

	private Image myImage;
	private float darknessSpeed = 0.2f;

	private bool darknessEnabled = false;

	void Start(){

		myImage = this.GetComponent<Image> ();
	}


	public void MakeItDarkFor(float _timer){

		StartCoroutine (MakeItDarkCo(_timer));
	}

	public bool DarknessEnabled{

		get{
			return darknessEnabled;
		}

		set{
			darknessEnabled = value;

			if(darknessEnabled){

				myImage.enabled = true;
				myImage.fillAmount = 1;
			}else{ 
				myImage.enabled = false;
				myImage.fillAmount = 0;
			}

		}
	}

    public bool DarknessEnabledEffect
    {

        get
        {
            return darknessEnabled;
        }

        set
        {
            darknessEnabled = value;

            if (darknessEnabled)
            {

                this.GetComponent<Animation>().Play();
            }
            else
            {
                myImage.enabled = false;
                myImage.fillAmount = 0;
            }

        }
    }

	private IEnumerator MakeItDarkCo(float _timer){

		myImage.enabled = true;

		while (myImage.fillAmount<1)
			myImage.fillAmount += darknessSpeed;

		myImage.fillAmount = 1;
		yield return new WaitForSeconds(_timer);


		while (myImage.fillAmount>0)
			myImage.fillAmount -= darknessSpeed;
		
		myImage.fillAmount = 0;

		myImage.enabled = false;
	}
}
