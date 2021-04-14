using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;

[RequireComponent (typeof (Button))]
public class InitializeAdsScript : MonoBehaviour, IUnityAdsListener
{
    #if UNITY_IOS
        //Unity Variables
        private string gameId = "4060649";
        private string mySurfacingIdRewarded = "Rewarded_iOS";
        private string mySurfacingIdBanner = "Banner_iOS";
        private string mySurfacingIdInterstitial = "Interstitial_iOS";

        //Google Variables
        string appId = "ca-app-pub-6074173583459415~2674239925";
        string bannerAdUnitId = "ca-app-pub-6074173583459415/7944055501";
        string interstitialAdUnitId = "ca-app-pub-6074173583459415/6220087905";
        string rewrdedAdUnitId = "ca-app-pub-6074173583459415/4993075644";

    #elif UNITY_ANDROID
        //Unity Variables
        private string gameId = "4060648";
        private string mySurfacingIdRewarded = "Rewarded_Android";
        private string mySurfacingIdBanner = "Banner_Android";
        private string mySurfacingIdInterstitial = "Interstitial_Android";

        //Google Variables
        string appId = "ca-app-pub-6074173583459415~3681566812";
        string bannerAdUnitId = "ca-app-pub-6074173583459415/9670687255";
        string interstitialAdUnitId = "ca-app-pub-6074173583459415/1792197233";
        string rewrdedAdUnitId = "ca-app-pub-6074173583459415/7974354402";
    #endif

    //Unity Variables
    bool testMode = true;
    bool iAdShown = false;
    bool startTimer = false;
    public float countdownTime = 5f;
    Button myButton;

    //Google Variables
    private BannerView googleBannerView;
    private InterstitialAd googleInterstitial;
    private RewardedAd googleRewarded;


    // Start is called before the first frame update
    void Start()
    {
        myButton = FindObjectOfType<Button>();

        if(myButton) 
        {
            if(Random.Range(0f,1f) <= 0.5f)
            {
                //Unity Rewarded
                myButton.onClick.AddListener(showRewardedVideo);
                Debug.Log("Unity Rewarded");
            }

            else
            {
                //Google Rewarded
                Debug.Log("Google Rewarded");
                myButton.onClick.AddListener(showGoogleRewardedVideo);
            }
        }

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);

        MobileAds.Initialize(initStatus => { });
        
        if(Random.Range(0f,1f) <= 0.5f)
        {
            //Unity Banner
            Debug.Log("Unity Banner");
            StartCoroutine(showBannerWhenInitialized());
        }

        else
        {
            //Google Banner
            Debug.Log("Google Banner");
            this.requestBanner();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!iAdShown)
        {
            if(Random.Range(0f,1f) <= 0.5f)
            {
                //Unity Interstitial
                showInterstitialAd();
                Debug.Log("Unity Interstitial");
            }

            else
            {
                //Google Interstitial
                requestInterstitial();
                if(this.googleInterstitial.IsLoaded())
                {
                    this.googleInterstitial.Show();
                    countdownTime = 5f;
                    startTimer = true;
                    Time.timeScale = 1f;
                    Debug.Log("Google Interstitial");
                }
                
            }
        }

        if(startTimer && countdownTime >= 0.0f)
        {
            countdownTime -= Time.deltaTime;
        }

        if(this.googleInterstitial != null && this.googleInterstitial.IsLoaded() && countdownTime <= 0.0f)
        {
            googleInterstitial.Destroy();
            startTimer = false;
        }
    }

    //Unity Banner
    IEnumerator showBannerWhenInitialized()
    {
        while(!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(mySurfacingIdBanner);
    }

    //Google Banner
    private void requestBanner()
    {
        // Create a 320x50 banner at the top of the screen.
        this.googleBannerView = new BannerView(bannerAdUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Called when an ad request has successfully loaded.
        this.googleBannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.googleBannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.googleBannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.googleBannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.googleBannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();

        this.googleBannerView.LoadAd(request);
    }

    //Unity Interstitial
    public void showInterstitialAd()
    {
        if(Advertisement.IsReady(mySurfacingIdInterstitial))
        {
            Advertisement.Show();
            //Debug.Log("Unity Interstitial");
            iAdShown = true;
        }
        
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later");
        }
    }

    //Google Interstitial
    private void requestInterstitial()
    {
        this.googleInterstitial = new InterstitialAd(interstitialAdUnitId);

        AdRequest request = new AdRequest.Builder().Build();

        this.googleInterstitial.LoadAd(request);
        
        iAdShown = true;
    }

    //Unity Rewarded
    public void showRewardedVideo()
    {
        if(Advertisement.IsReady(mySurfacingIdRewarded))
        {
            countdownTime = 5f;
            Advertisement.Show(mySurfacingIdRewarded);
            Debug.Log("Unity Rewarded");
        }

        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    //Google Rewarded
    private void requestRewarded()
    {
        this.googleRewarded = new RewardedAd(rewrdedAdUnitId);

        AdRequest request = new AdRequest.Builder().Build();

        // Called when the user should be rewarded for interacting with the ad.
        this.googleRewarded.OnUserEarnedReward += HandleUserEarnedReward;

        this.googleRewarded.LoadAd(request);
    }

    private void showGoogleRewardedVideo()
    {
        requestRewarded();
        if(this.googleRewarded.IsLoaded())
        {
            this.googleRewarded.Show();
        }
    }

    //Unity event handlers
    public void OnUnityAdsDidFinish (string surfacingId, ShowResult showResult) 
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) 
        {
            //Shows when close button pressed for interstitial and rewarded ads
            //Debug.Log("You finished the ads! Here is your reward!");
            if(surfacingId == mySurfacingIdRewarded)
            {
                if(countdownTime > 0f)
                {
                    Debug.Log("Unity - You closed the ad before a reward could be given - " + countdownTime + " seconds left");
                }

                else
                {
                    Debug.Log("Unity - You finished the ads! Here is your reward!");
                    GetComponent<gameManager>().setHints(1);
                    startTimer = false;
                }
            }

            else if(surfacingId == mySurfacingIdInterstitial)
            {
                Debug.Log("Unity - You closed the ad");
            }
        }
        else if (showResult == ShowResult.Skipped) 
        {
            //Shows when skip button pressed for interstitial ad
            Debug.Log("Unity - You skipped the ad. No reward for you");
        } 
        else if (showResult == ShowResult.Failed) 
        {
            Debug.Log("Unity - The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady (string surfacingId) 
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingIdRewarded || surfacingId == mySurfacingIdBanner) {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
            myButton.interactable = Advertisement.IsReady(mySurfacingIdRewarded);
        }
    }

    public void OnUnityAdsDidError (string message) 
    {
        // Log the error.
        Debug.Log("Unity - Ad Error - " + message);
    }

    public void OnUnityAdsDidStart (string surfacingId) 
    {
        // Optional actions to take when the end-users triggers an ad.
        if(surfacingId == mySurfacingIdRewarded)
        {
            startTimer = true;
        }
    } 

    // Google event handlers
    public void OnDestroy() 
    {
        Advertisement.RemoveListener(this);
    }

    public void HandleOnAdLoaded(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("Google - HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("Google - HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("Google - HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("Google - HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("Google - HandleAdLeavingApplication event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for 1 hint");
        GetComponent<gameManager>().setHints(1);
    }

}
