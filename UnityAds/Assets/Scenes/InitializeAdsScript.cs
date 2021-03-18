using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAdsScript : MonoBehaviour, IUnityAdsListener
{

    string gameId = "4045807";
    string mySurfacingId = "Rewarded_iOS";
    bool testMode = true;
    bool iAdShown = false;
    bool rAdShown = false;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    // Update is called once per frame
    void Update()
    {
        if(!iAdShown)
        {
            //showInterstitialAd();
        }

        if(!rAdShown)
        {
            showRewardedVideo();
        }
    }

    public void showInterstitialAd()
    {
        if(Advertisement.IsReady())
        {
            Advertisement.Show();
            iAdShown = true;
        }
        
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later");
        }
    }

    public void showRewardedVideo()
    {
        if(Advertisement.IsReady(mySurfacingId))
        {
            Advertisement.Show(mySurfacingId);
            rAdShown = true;
        }

        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    public void OnUnityAdsDidFinish (string surfacingId, ShowResult showResult) 
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            // Reward the user for watching the ad to completion.
        } else if (showResult == ShowResult.Skipped) {
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady (string surfacingId) 
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingId) {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError (string message) 
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string surfacingId) 
    {
        // Optional actions to take when the end-users triggers an ad.
    } 

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy() 
    {
        Advertisement.RemoveListener(this);
    }

}
