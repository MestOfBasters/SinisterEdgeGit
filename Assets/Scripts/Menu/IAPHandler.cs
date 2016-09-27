using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IAPHandler : MonoBehaviour
{


    public Text initstate;
    public GameObject BuyButton, demoTitle, fullTitle, demoSub, fullSub;

	void Awake () {
		
		
		BuyButton.SetActive(!IAPManager.hasFullVersion);
		demoTitle.SetActive(!IAPManager.hasFullVersion);
		demoSub.SetActive(!IAPManager.hasFullVersion);
		
		fullTitle.SetActive(IAPManager.hasFullVersion);
		fullSub.SetActive(IAPManager.hasFullVersion);
	}

	void Start () {
        //Initialize Billing
        
		//IOSTEST .aus
		//IAPManager.init();

		BuyButton.SetActive(!IAPManager.hasFullVersion);
		demoTitle.SetActive(!IAPManager.hasFullVersion);
		demoSub.SetActive(!IAPManager.hasFullVersion);
		
		fullTitle.SetActive(IAPManager.hasFullVersion);
		fullSub.SetActive(IAPManager.hasFullVersion);
	}

    void FixedUpdate()
    {

			//IOSTEST .aus

//        if (IAPManager.isInited)
//        {
//            initstate.text = "connection initialized!";
//        }
//        else
//        {
//            initstate.text = "No internet connection!";
//        }

        //PlayFullVersion.interactable = IAPManager.hasFullVersion;

        BuyButton.SetActive(!IAPManager.hasFullVersion);
		demoTitle.SetActive(!IAPManager.hasFullVersion);
		demoSub.SetActive(!IAPManager.hasFullVersion);

		fullTitle.SetActive(IAPManager.hasFullVersion);
		fullSub.SetActive(IAPManager.hasFullVersion);
    }

    public void BuyFullVersion()
    {
		//IOSTEST .aus

//		if (IOSInAppPurchaseManager.Instance.IsStoreLoaded)
//        {
			IOSInAppPurchaseManager.Instance.BuyProduct("fullversion");
            //AndroidInAppPurchaseManager.Instance.Purchase("fullversion");
//        }
//        else
//        {
////            AndroidMessage.Create("Error!", "Please make sure you are connected to the internet, then restart Sinister Edge.");
//			if(IOSInAppPurchaseManager.Instance.IsWaitingLoadResult)
//			{
//				ModalPanel.Instance ().MasterInfo ("Error!\n" +
//					"Payment system not initialize, please wait a few seconds and try again", "Ok");
//			}
//
////			ModalPanel.Instance ().MasterInfo ("Error!\n" +
////				"Please make sure you are connected to the internet, then restart Sinister Edge.", "Ok");
//		}
//
    }
	//IOSTEST .aus

//    public void Reset()
//    {
//        if (IAPManager.isInited)
//        {
//            if (AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased("fullversion"))
//            {
//                IAPManager.consume("fullversion");
//            }
//            else
//            {
////                AndroidMessage.Create("Error!", "You do not own product to reset");
//
//				ModalPanel.Instance ().MasterInfo ("Error!\n" +
//					"You do not own product to reset", "Ok");
//            }
//
//        }
//        else
//        {
////            AndroidMessage.Create("Error!", "PaymnetManagerExample not yet inited");
//			ModalPanel.Instance ().MasterInfo ("Error!\n" +
//				"PaymnetManagerExample not yet inited", "Ok");
//        }
//    }
}
