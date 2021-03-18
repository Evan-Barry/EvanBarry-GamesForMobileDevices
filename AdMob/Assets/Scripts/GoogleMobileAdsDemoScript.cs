using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    public bool interstitialPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });

        //this.RequestBanner();
        //this.RequestInterstitialAd();
        this.RequestRewardedAd();

        rewardedAd.OnAdLoaded += handleOnAdLoaded;
        rewardedAd.OnAdClosed += handleOnAdClosed;
        rewardedAd.OnAdFailedToLoad += handleOnAdFailedToLoad;
        rewardedAd.OnAdFailedToShow += handleOnAdFailedToShow;
        rewardedAd.OnAdOpening += handleOnAdOpening;
        rewardedAd.OnUserEarnedReward += handleOnUserEarnedReward;

        if(rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }

        else
        {
            Debug.Log("Rewarded ad not loaded");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // if(interstitialPlayed == false)
        // {
        //     if (this.interstitial.IsLoaded()) 
        //     {
        //         this.interstitial.Show();
        //         interstitialPlayed = true;
        //     }
        // }
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);
    }

    private void RequestInterstitialAd()
    {
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        this.interstitial = new InterstitialAd(adUnitId);

        AdRequest request = new AdRequest.Builder().Build();

        this.interstitial.LoadAd(request);
    }

    private void RequestRewardedAd()
    {
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";

        this.rewardedAd = new RewardedAd(adUnitId);

        AdRequest request = new AdRequest.Builder().Build();

        this.rewardedAd.LoadAd(request);
    }

    public void handleOnAdLoaded(object sender, System.EventArgs args)
    {
        Debug.Log("Ad has been loaded");
    }

    public void handleOnAdFailedToLoad(object sender, AdErrorEventArgs args)
    {

    }

    public void handleOnAdFailedToShow(object sender, AdErrorEventArgs args)
    {

    }

    public void handleOnAdOpening(object sender, System.EventArgs args)
    {

    }

    public void handleOnAdClosed(object sender, System.EventArgs args)
    {

    }

    public void handleOnUserEarnedReward(object sender, Reward args)
    {
        Debug.Log("You got " + args.Amount + " " + args.Type);
    }

}
