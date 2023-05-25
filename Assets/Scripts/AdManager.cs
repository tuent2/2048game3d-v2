using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HuaweiMobileServices.Ads;
using HuaweiMobileServices.IAP;
using HuaweiMobileServices.Utils;
using HuaweiMobileServices.Base;
using HmsPlugin;
public class AdManager : MonoBehaviour
{
    public static AdManager Instance { set; get; }

    public string InterstitialID;
    public string bannerAdUnitID = "YOUR_BANNER_AD_UNIT_ID";
    public string RewardVideoID = "YOUR_AD_UNIT_ID";

    public RectTransform BannerWidthImage;

    float BannerSize;


    public float AdTimer;
    float timer;
    bool adsRewardCompleted;

    string SDK_Key = "mR6HVBpGDQhcB0enTTV5FswAGv8g-camal7ASD0-4FFTlbnGmUXkRwIwKuem6yalmmu54O0yoNW0JLSoyFvTHz";

    void Awake()
    {

        BannerSize = BannerWidthImage.rect.width / 2;

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads
        };





        if (PlayerPrefs.GetInt("Purchase", 0) == 0)
        {
            // MaxSdk.SetSdkKey(SDK_Key);
            // MaxSdk.InitializeSdk();
            // Debug.Log("All Ads Initilized");
            InitializeInterstitialAds();
            InitializeBannerAds();
            InitializeRewardedAds();
        }

        InvokeRepeating("LaunchProjectile", AdTimer, AdTimer);

    }


    void Start()
    {
        Instance = this;



        // #region SetNonPersonalizedAd , SetRequestLocation

        // var builder = HwAds.RequestOptions.ToBuilder();

        // builder
        //     .SetConsent("tcfString")
        //     .SetNonPersonalizedAd((int)NonPersonalizedAd.ALLOW_ALL)
        //     .Build();

        // bool requestLocation = true;
        // var requestOptions = builder.SetConsent("testConsent").SetRequestLocation(requestLocation).Build();

        // UnityEngine.Debug.Log($"RequestOptions NonPersonalizedAds:  {requestOptions.NonPersonalizedAd}");
        // UnityEngine.Debug.Log($"Consent: {requestOptions.Consent}");

        // #endregion

        // if (PlayerPrefs.GetInt("Purchase", 0) == 0)
        // {

        // if (timer > AdTimer)
        // {
        //     timer = 0;
        //     UIManager.Instance.RespondAds.gameObject.SetActive(true);
        // }
        // else
        // {
        //     timer += 0.01f;
        //     Debug.Log(timer);
        // }
        // }
        if (HMSIAPKitSettings.Instance.Settings.GetBool(HMSIAPKitSettings.InitializeOnStart))
        {
            HMSIAPManager.Instance.InitializeIAP();
        }

    }

    private void OnEnable()
    {
        HMSIAPManager.Instance.OnBuyProductSuccess += OnBuyProductSuccess;
        HMSIAPManager.Instance.OnInitializeIAPSuccess += OnInitializeIAPSuccess;
        HMSIAPManager.Instance.OnInitializeIAPFailure += OnInitializeIAPFailure;
        HMSIAPManager.Instance.OnBuyProductFailure += OnBuyProductFailure;
    }

    private void OnDisable()
    {
        HMSIAPManager.Instance.OnBuyProductSuccess -= OnBuyProductSuccess;
        HMSIAPManager.Instance.OnInitializeIAPSuccess -= OnInitializeIAPSuccess;
        HMSIAPManager.Instance.OnInitializeIAPFailure -= OnInitializeIAPFailure;
        HMSIAPManager.Instance.OnBuyProductFailure -= OnBuyProductFailure;
    }

    private static readonly string PRODUCT_ID = "your_product_id";

    private IIapClient iapClient;

    private void RestoreProducts()
    {
        // OwnedPurchasesReq ownedPurchasesReq = new OwnedPurchasesReq();

        // ITask<OwnedPurchasesResult> task = iapClient.ObtainOwnedPurchases(ownedPurchasesReq);
        // task.AddOnSuccessListener((result) =>
        // {
        //     Debug.Log("HMSP: recoverPurchases");
        //     foreach (string inAppPurchaseData in result.InAppPurchaseDataList)
        //     {
        //         ConsumePurchaseWithPurchaseData(inAppPurchaseData);
        //         Debug.Log("HMSP: recoverPurchases result> " + result.ReturnCode);
        //     }
        //     OnRecoverPurchasesSuccess?.Invoke();
        // }).AddOnFailureListener((exception) =>
        // {
        //     Debug.Log($"HMSP: Error on recoverPurchases {exception.StackTrace}");
        //     OnRecoverPurchasesFailure?.Invoke(exception);
        // });


        // var ownedPurchaseRequest = new OwnedPurchasesReq() { PriceType = priceType };

        // ITask<OwnedPurchasesResult> task = iapClient.ObtainOwnedPurchaseRecord(ownedPurchaseRequest);

        // task.AddOnSuccessListener((result) =>
        // {
        //     OnObtainOwnedPurchaseRecordSuccess?.Invoke(result);

        // }).AddOnFailureListener((exception) =>
        // {
        //     Debug.LogError($"[{Tag}]: ObtainOwnedPurchaseRecord - Fail");

        //     OnObtainOwnedPurchaseRecordFailure?.Invoke(exception);
        // });


        HMSIAPManager.Instance.RestorePurchaseRecords((restoredProducts) =>
            {
                foreach (var item in restoredProducts.InAppPurchaseDataList)
                {
                    if ((IAPProductType)item.Kind == IAPProductType.Consumable)
                    {
                        UnityEngine.Debug.Log($"aaaaaaaaaaaaaa Consumable: ProductId {item.ProductId} , SubValid {item.SubValid} , PurchaseToken {item.PurchaseToken} , OrderID  {item.OrderID}");
                        // consumablePurchaseRecord.Add(item);
                    }
                }
            });

        HMSIAPManager.Instance.RestoreOwnedPurchases((restoredProducts) =>
           {
               Debug.Log("kkkkkkdadenday" + restoredProducts.InAppPurchaseDataList.Count);
               foreach (var item in restoredProducts.InAppPurchaseDataList)
               {
                   Debug.Log("kkkk" + (IAPProductType)item.Kind);
                   if ((IAPProductType)item.Kind == IAPProductType.Subscription)
                   {
                       Debug.Log("kkkkkkdadendaySub");
                       Debug.Log($"Subscription: ProductId {item.ProductId} , ExpirationDate {item.ExpirationDate} , AutoRenewing {item.AutoRenewing} , PurchaseToken {item.PurchaseToken} , OrderID {item.OrderID}");
                       InAppPurchase.Intance.OnPurchaseComplate();
                   }

                   else if ((IAPProductType)item.Kind == IAPProductType.NonConsumable)
                   {
                       Debug.Log("kkkkkkdadendayNONConsumable");
                       Debug.Log($"NonConsumable: ProductId {item.ProductId} , DaysLasted {item.DaysLasted} , SubValid {item.SubValid} , PurchaseToken {item.PurchaseToken} ,OrderID {item.OrderID}");
                       InAppPurchase.Intance.OnPurchaseComplate();
                   }
               }
           });

    }

    private void OnBuyProductSuccess(PurchaseResultInfo obj)
    {
        UnityEngine.Debug.Log($"aaaaaaaaaaaaaa OnBuyProductSuccess");
        InAppPurchase.Intance.OnPurchaseComplate();
        // if (obj.InAppPurchaseData.ProductId == "removeads")
        // {
        //     IAPLog?.Invoke("Ads Removed!");
        // }
        // else if (obj.InAppPurchaseData.ProductId == "coins100")
        // {
        //     IAPLog?.Invoke("coins100 Purchased!");
        // }
        // else if (obj.InAppPurchaseData.ProductId == "premium")
        // {
        //     IAPLog?.Invoke("premium subscribed!");
        // }
    }

    private void OnInitializeIAPFailure(HMSException obj)
    {
        UnityEngine.Debug.Log("aaaaaaaaaaaaaa IAP is not ready.");
        InAppPurchase.Intance.OnPurchaseFailed();
    }

    private void OnInitializeIAPSuccess()
    {
        UnityEngine.Debug.Log("aaaaaaaaaaaaaa IAP is ready.");

        RestoreProducts();
    }

    private void OnBuyProductFailure(int code)
    {
        UnityEngine.Debug.Log("aaaaaaaaaaaaaa Purchase Fail.");
    }

    public void buyIAPChecked(string productID)
    {
        HMSIAPManager.Instance.PurchaseProduct(productID);
    }

    public void ShowRewardBasedVideo()
    {

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            LoadRewardedAd();
            // ShowSplashVideo();

        }
        else
            StartCoroutine(waitToSetting());
    }

    IEnumerator waitToSetting()
    {
        yield return new WaitForSeconds(1f);
        GameManager.isGameScreen = true;
    }
    private void LateUpdate()
    {
        // if (PlayerPrefs.GetInt("Purchase", 0) == 0)
        // {
        //     InvokeRepeating("LaunchProjectile", 0f, AdTimer);
        //     // if (timer > AdTimer)
        //     // {
        //     //     timer = 0;
        //     //     UIManager.Instance.RespondAds.gameObject.SetActive(true);
        //     // }
        //     // else
        //     // {
        //     //     timer += 0.01f;
        //     //     Debug.Log(timer);
        //     // }
        // }
        // adsIsOver();
    }
    void LaunchProjectile()
    {
        if (GameManager.isGameScreen == true)
        {
            UIManager.Instance.RespondAds.gameObject.SetActive(true);
        }

    }

    #region Interstitial
    void InitializeInterstitialAds()
    {
        // MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        // MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        // MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;


        // LoadInterstitial();


    }


    private void LoadInterstitial()
    {
        // MaxSdk.LoadInterstitial(InterstitialID);
    }


    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Debug.Log("Ads failed to load");
        // LoadInterstitial();
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // LoadInterstitial();
    }

    private void OnInterstitialDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {

        // Debug.Log("Ads Showing successfull");
        // LoadInterstitial();

    }


    public void ShowInterstitial()
    {
        HMSAdsKitManager.Instance.ShowInterstitialAd();
        // if (MaxSdk.IsInterstitialReady(InterstitialID))
        // {
        //     MaxSdk.ShowInterstitial(InterstitialID);
        // }
    }

    #endregion



    #region Banner
    void InitializeBannerAds()
    {
        // // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        // MaxSdk.CreateBanner(bannerAdUnitID, MaxSdkBase.BannerPosition.BottomCenter);

        // MaxSdk.SetBannerWidth(bannerAdUnitID, BannerSize);


        // //MaxSdk.SetBannerExtraParameter(bannerAdUnitID, "adaptive_banner", "false");


        // // Set background or background color for banners to be fully functional
        // MaxSdk.SetBannerBackgroundColor(bannerAdUnitID, Color.clear);
        BannerShow();
    }

    public void BannerShow()
    {
        HMSAdsKitManager.Instance.ShowBannerAd();
        //     MaxSdk.ShowBanner(bannerAdUnitID);
    }
    public void HideBanner()
    {
        HMSAdsKitManager.Instance.HideBannerAd();
        // MaxSdk.HideBanner(bannerAdUnitID);
    }

    public void DestroyBanner()
    {
        // MaxSdk.DestroyBanner(bannerAdUnitID);
    }
    #endregion




    #region Rewarding Video
    void InitializeRewardedAds()
    {
        // // Attach callback
        // MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        // MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        // MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        // MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        // MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        // MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        // MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        // MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        HMSAdsKitManager.Instance.OnRewardAdCompleted = OnRewarded;
        HMSAdsKitManager.Instance.OnRewardedAdFailedToLoad = OnRewardedAdFailedToLoad;
        // // Load the first rewarded ad
        // LoadRewardedAd();

    }
    public void LoadRewardedAd()
    {
        // MaxSdk.LoadRewardedAd(RewardVideoID);
        // UnityEngine.Debug.Log("[HMS] AdsDemoManager rewarded!");
        // HMSAdsKitManager.Instance.OnRewardAdCompleted();
        // adsRewardCompleted = true;
        UnityEngine.Debug.Log("[HMS] AdsDemoManager ShowRewardedAd");
        // HMSAdsKitManager.Instance.ShowRewardedAd();
        // StartCoroutine(DelaySetUsingItem());
        if (HMSAdsKitManager.Instance.ShowRewardedAd() == false)
        {
            UnityEngine.Debug.Log("[HMS] AdsDemoManager ShowRewardedAdccccccccccccccccccccccccccccccccccccccccccccccccccccccc");
            // MenuReference.THIS.Repond.gameObject.SetActive(true);
            // GameManager.isUsingItem = false;
            StartCoroutine(DelayRespondRewardAds());
            // UIManager.Instance.RespondRewardAds.gameObject.SetActive(true);
            Debug.Log("kkkkkkkkkkkkk" + UIManager.Instance.RespondRewardAds.gameObject);
        }
    }


    IEnumerator DelayRespondRewardAds()
    {
        yield return new WaitForSeconds(0.1f);
        // GameManager.isUsingItem = false;
        Debug.Log("kkkkkkkkkkkkk12" + UIManager.Instance.RespondRewardAds.gameObject);
        GameManager.isGameScreen = true;
        UIManager.Instance.RespondRewardAds.gameObject.SetActive(true);
    }

    public void OnRewarded()
    {
        UnityEngine.Debug.Log("[HMS] AdsDemoManager rewarded!");
        // HMSAdsKitManager.Instance.OnRewardAdCompleted();
        // adsRewardCompleted = true;s
        GameManager.isGameScreen = true;
        GameManager.Coin = GameManager.Coin + 50;
        UIManager.Instance.UpdateCoinText(PlayerPrefs.GetInt("Coin", 0) + 50);

    }

    void adsIsOver()
    {
        if (adsRewardCompleted == true)
        {
            GameManager.isUsingItem = true;
        }
        adsRewardCompleted = false;

    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

    }

    public void OnRewardedAdFailedToLoad(int errorCode)
    {
        StartCoroutine(DelayRespondRewardAds());
        // UnityEngine.Debug.Log("[HMS] AdsDemoManager FailedToLoad rewarded!");
        // UIManager.Instance.RespondRewardAds.gameObject.SetActive(true);

    }
    // private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    // {

    // }


    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
    }


    public void ShowVideo()
    {
        // if (MaxSdk.IsRewardedAdReady(RewardVideoID))
        // {
        //     MaxSdk.ShowRewardedAd(RewardVideoID);
        // }
    }



    #endregion

}
