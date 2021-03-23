using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rewardedAdsButton : MonoBehaviour
{
    #if UNITY_IOS
    private string gameId = "4045807";
    private string mySurfacingId = "Rewarded_iOS";
    #elif UNITY_ANDROID
    private string gameId = "4045806";
    private string mySurfacingId = "Rewarded_Android";
    #endif
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
