using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

[RequireComponent (typeof (Button))]
public class rewardedAdsButton : MonoBehaviour, IUnityAdsListener
{
    #if UNITY_IOS
    private string gameId = "4060649";
    private string mySurfacingId = "Rewarded_iOS";
    #elif UNITY_ANDROID
    private string gameId = "4060648";
    private string mySurfacingId = "Rewarded_Android";
    #endif

    Button myButton;
    // Start is called before the first frame update
    void Start()
    {
        myButton = GetComponent<Button>();

        myButton.interactable = Advertisement.IsReady(mySurfacingId);

        if(myButton) 
        {
            myButton.onClick.AddListener(ShowRewardedVideo);
        }

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    void ShowRewardedVideo() 
    {
        Advertisement.Show(mySurfacingId);
    }

    public void OnUnityAdsReady(string surfacingId) 
    {
        if (surfacingId == mySurfacingId) 
        {        
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult) 
    {
        if (showResult == ShowResult.Finished) 
        {
            // Reward the user for watching the ad to completion.
        } else if (showResult == ShowResult.Skipped) 
        {
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) 
        {
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message) 
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId) 
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
