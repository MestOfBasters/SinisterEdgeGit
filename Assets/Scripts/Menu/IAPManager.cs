using UnityEngine;
using System.Collections;


public class IAPManager : MonoBehaviour
{

//    private static bool _isInited = false;


	private static bool _hasFullVersion = false;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public const string FULLVERSION 	=  "fullversion";

	private static bool IsInitialized = false;

	void Awake() {
		init();
	}

	public static void init() {


		if(!IsInitialized) {

			//You do not have to add products by code if you already did it in seetings guid
			//Windows -> IOS Native -> Edit Settings
			//Billing tab.
			//IOSInAppPurchaseManager.Instance.AddProductId(FULLVERSION);

			//Event Use Examples
			IOSInAppPurchaseManager.OnVerificationComplete += HandleOnVerificationComplete;
			IOSInAppPurchaseManager.OnStoreKitInitComplete += OnStoreKitInitComplete;


			IOSInAppPurchaseManager.OnTransactionComplete += OnTransactionComplete;
			IOSInAppPurchaseManager.OnRestoreComplete += OnRestoreComplete;


			IsInitialized = true;


			IOSInAppPurchaseManager.Instance.LoadStore();
		} 

	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------


	public static void buyItem(string productId) {
		IOSInAppPurchaseManager.Instance.BuyProduct(productId);
	}



	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	//--------------------------------------
	//  EVENTS
	//--------------------------------------


	private static void UnlockProducts(string productIdentifier) {
		switch(productIdentifier) {
		case FULLVERSION:
			//code for adding small game money amount here
			hasFullVersion = true;
			break;
		}
	}


	private static void OnTransactionComplete (IOSStoreKitResult result) {

		ISN_Logger.Log("OnTransactionComplete: " + result.ProductIdentifier);
		ISN_Logger.Log("OnTransactionComplete: state: " + result.State);

		switch(result.State) {
		case InAppPurchaseState.Purchased:
		case InAppPurchaseState.Restored:
			//Our product been succsesly purchased or restored
			//So we need to provide content to our user depends on productIdentifier
			UnlockProducts(result.ProductIdentifier);
			break;
		case InAppPurchaseState.Deferred:
			//iOS 8 introduces Ask to Buy, which lets parents approve any purchases initiated by children
			//You should update your UI to reflect this deferred state, and expect another Transaction Complete  to be called again with a new transaction state 
			//reflecting the parent’s decision or after the transaction times out. Avoid blocking your UI or gameplay while waiting for the transaction to be updated.
			break;
		case InAppPurchaseState.Failed:
			//Our purchase flow is failed.
			//We can unlock intrefase and repor user that the purchase is failed. 
			ISN_Logger.Log("Transaction failed with error, code: " + result.Error.Code);
			ISN_Logger.Log("Transaction failed with error, description: " + result.Error.Description);


			break;
		}

		if(result.State == InAppPurchaseState.Failed) {
			//IOSNativePopUpManager.showMessage("Transaction Failed", "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Description);
		} else {
			//IOSNativePopUpManager.showMessage("Store Kit Response", "product " + result.ProductIdentifier + " state: " + result.State.ToString());
		}

	}

	private static void OnRestoreComplete (IOSStoreKitRestoreResult res) {
		if(res.IsSucceeded) {
			IOSNativePopUpManager.showMessage("Success", "Restore Completed");
		} else {
			IOSNativePopUpManager.showMessage("Error: " + res.Error.Code, res.Error.Description);
		}
	}	


	static void HandleOnVerificationComplete (IOSStoreKitVerificationResponse response) {
		//IOSNativePopUpManager.showMessage("Verification", "Transaction verification status: " + response.status.ToString());

		ISN_Logger.Log("ORIGINAL JSON: " + response.originalJSON);
	}

	private static void OnStoreKitInitComplete(ISN_Result result) {

		if(result.IsSucceeded) {

			int avaliableProductsCount = 0;
			foreach(IOSProductTemplate tpl in IOSInAppPurchaseManager.instance.Products) {
				if(tpl.IsAvaliable) {
					avaliableProductsCount++;
				}

				if (avaliableProductsCount > 0)
					_hasFullVersion = true;
			}
		


		//	IOSNativePopUpManager.showMessage("StoreKit Init Succeeded", "Available products count: " + avaliableProductsCount +" store is loaded bool: "+IOSInAppPurchaseManager.Instance.IsStoreLoaded);
		//	ISN_Logger.Log("StoreKit Init Succeeded Available products count: " + avaliableProductsCount);

		} else {
		//	IOSNativePopUpManager.showMessage("StoreKit Init Failed but checkin playerprefs",  "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Description);
			            //check player Prefs if bought
			            string fv = SecurePlayerPrefs.GetString("FullVersion", "ThanksForAllTheFish");
			            if (fv == "true")
			                _hasFullVersion = true;
		}
	}


//
//	public static IAPManager iApM;
//
//    void Awake()
//    {
//		//Debug.Log ("Try to Awake");
//		if (iApM == null)
//		{
//			DontDestroyOnLoad(gameObject);
//			//Debug.Log ("Me");
//			iApM = this;
//		}
//		else if (iApM != this)
//		{
//			//Debug.Log ("NotMe");
//			Destroy(this.gameObject);
//			return;
//		}
//    }
//    public static void init()
//    {
//        //listening for purchase and consume events
//        AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;
//
//        //listening for store initilaizing finish
//        AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
//
//        //you may use loadStore function without parametr if you have filled base64EncodedPublicKey in plugin settings
//        AndroidInAppPurchaseManager.Instance.LoadStore();
//
//
//        
//    }
//
//
//    //--------------------------------------
//    //  PUBLIC METHODS
//    //--------------------------------------
//
//
//    public static void purchase(string SKU)
//    {
//        AndroidInAppPurchaseManager.Instance.Purchase(SKU);
//    }
//
//    public static void consume(string SKU)
//    {
//        AndroidInAppPurchaseManager.Instance.Consume(SKU);
//    }
//
//    //--------------------------------------
//    //  GET / SET
//    //--------------------------------------
//
//    public static bool isInited
//    {
//        get
//        {
//            return _isInited;
//        }
//    }
//
    public static bool hasFullVersion
    {
        get
        {
            return _hasFullVersion;
        }
        set
        {
            _hasFullVersion = value;

            if (_hasFullVersion)
            {
                SecurePlayerPrefs.SetString("FullVersion", "true", "ThanksForAllTheFish");
                PlayerPrefs.Save();
            }

        }
    }
//    //--------------------------------------
//    //  EVENTS
//    //--------------------------------------
//
//    private static void OnProcessingPurchasedProduct(GooglePurchaseTemplate purchase)
//    {    
//        //some stuff for processing product purchse. Add coins, unlock track, etc
//        switch (purchase.SKU)
//        {
//		case "fullversion":
////			AndroidMessage.Create ("Thank you!", "You now own the full version of Sinister Edge. You're awesome!", "Yes, I am.");
//			ModalPanel.Instance ().MasterInfo ("Thank you!\n" +
//				"You now own the full version of Sinister Edge. You're awesome!", "Yes, I am.");
//				hasFullVersion = true;
//                break;
//        }
//    }
//
//    private static void OnProcessingConsumeProduct(GooglePurchaseTemplate purchase)
//    {
//        //some stuff for processing product consume. Reduse tip anount, reduse gold token, etc
//        switch (purchase.SKU)
//        {
//            case "fullversion":
//			ModalPanel.Instance ().MasterInfo ("Debug!\n" +
//				"You lost everything. Yes, everything. This includes your full version of Sinister Edge", "Ok :<");    
//			hasFullVersion = false;
//                break;
//        }
//    }
//    
//    private static void OnProductPurchased(BillingResult result)
//    {
//
//
//        if (result.isSuccess)
//        {
//            //AndroidMessage.Create("Product Purchased", result.purchase.SKU + "\n Full Response: " + result.purchase.originalJson);
//            OnProcessingPurchasedProduct(result.purchase);
//        }
//        else
//        {
////            AndroidMessage.Create("Product Purchase Failed!", "User canceled the action.", "O K");
//			ModalPanel.Instance ().MasterInfo ("Product Purchase Failed!\n" +
//				"User canceled the action.", "O K");
//        }
//
//        //Debug.Log("Purchased Responce: " + result.response.ToString() + " " + result.message);
//        //Debug.Log(result.purchase.originalJson);
//    }
//
//    private static void OnProductConsumed(BillingResult result)
//    {
//
//        if (result.isSuccess)
//        {
//            //AndroidMessage.Create("Product Consumed", result.purchase.SKU + "\n Full Response: " + result.purchase.originalJson);
//            OnProcessingConsumeProduct(result.purchase);
//        }
//        else
//        {
//            //AndroidMessage.Create("Product Cousume Failed!", result.response.ToString() + " " + result.message);
//        }
//
//        //Debug.Log("Cousume Responce: " + result.response.ToString() + " " + result.message);
//    }
//
//    private static void OnBillingConnected(BillingResult result)
//    {
//        AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
//
//
//        if (result.isSuccess)
//        {
//            //Store connection is Successful. Next we loading product and customer purchasing details
//            AndroidInAppPurchaseManager.Instance.RetrieveProducDetails();
//            AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
//        }
//
//        
//        //AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
//        //Debug.Log("Connection Responce: " + result.response.ToString() + " " + result.message);
//    }
//
//
//
//
//    private static void OnRetrieveProductsFinised(BillingResult result)
//    {
//        AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;
//
//
//        if (result.isSuccess)
//        {
//            _isInited = true;
//            //AndroidMessage.Create("Success", "Billing init complete inventory contains: " + AndroidInAppPurchaseManager.instance.inventory.purchases.Count + " products");
//
//            if(AndroidInAppPurchaseManager.instance.inventory.purchases.Count > 0)
//            { hasFullVersion = true; }
//            //Debug.Log("Loaded products names");
//            //foreach (GoogleProductTemplate tpl in AndroidInAppPurchaseManager.instance.inventory.products)
//            //{
//            //    Debug.Log(tpl.title);
//            //    Debug.Log(tpl.originalJson);
//            //    if (tpl.title == "The Mansion - Full Version")
//            //    {
//            //        hasFullVersion = true;
//            //    }
//            //}
//        }
//        else
//        {
//            //AndroidMessage.Create("Billing init failed", "getting fullversion check from player prefs");
//            //check player Prefs if bought
//            string fv = SecurePlayerPrefs.GetString("FullVersion", "ThanksForAllTheFish");
//            if (fv == "true")
//                _hasFullVersion = true;
//
//            // AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
//        }
//
//        //Debug.Log("Connection Responce: " + result.response.ToString() + " " + result.message);
//
//    }

}
