using System;
using TMPro;
using Unity.Services.LevelPlay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AdIntegrationHandler : MonoBehaviour
{
    [SerializeField]
    private bool _runTestSuite = false;
    private string _rewardedAdUnitId = "snm7yvaysvvkfsny";
    private LevelPlayRewardedAd _rewardedAd;
    private bool _SDKInitialized = false;
    private bool _adIsLoading = false;
    private Action _onCurrentRewardedAddFinished = null;

    public bool AdIsLoading => _adIsLoading;
    public bool SDKInitialized => _SDKInitialized;
    public UnityEvent OnRewardedAdFinishedLoading = new UnityEvent();

    public void Initialize()
    {
        if (_runTestSuite) LevelPlay.SetMetaData("is_test_suite", "enable");
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
        LevelPlay.Init("279d100a5");
    }

    public bool PlayRewardedAd(Action OnAddRewardAction)
    {
        if (!_SDKInitialized || !_rewardedAd.IsAdReady()) return false;
        _onCurrentRewardedAddFinished = OnAddRewardAction;
        _rewardedAd.ShowAd();
        return true;
    }

    void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.LogError("Init failed");
    }

    void SdkInitializationCompletedEvent(LevelPlayConfiguration configuration)
    {
        _SDKInitialized = true;
        _rewardedAd = new LevelPlayRewardedAd(_rewardedAdUnitId);

        _rewardedAd.OnAdLoaded += RewardedOnAdLoadedEvent;
        _rewardedAd.OnAdLoadFailed += RewardedOnAdLoadFailedEvent;
        _rewardedAd.OnAdDisplayed += RewardedOnAdDisplayedEvent;
        _rewardedAd.OnAdDisplayFailed += RewardedOnAdDisplayFailedEvent;
        _rewardedAd.OnAdRewarded += RewardedOnAdRewardedEvent;
        _rewardedAd.OnAdClosed += RewardedOnAdClosedEvent;

        _rewardedAd.OnAdClicked += RewardedOnAdClickedEvent;
        _rewardedAd.OnAdInfoChanged += RewardedOnAdInfoChangedEvent;

        if(_runTestSuite) LevelPlay.LaunchTestSuite();
        LoadRewardedAd();
    }

    void RewardedOnAdLoadedEvent(LevelPlayAdInfo adInfo) 
    {
        OnRewardedAdFinishedLoading.Invoke();
        _adIsLoading = false;
    }

    void RewardedOnAdLoadFailedEvent(LevelPlayAdError error) 
    {
        Debug.LogError("Ad load failed");
        LoadRewardedAd();
    }

    void RewardedOnAdDisplayedEvent(LevelPlayAdInfo adInfo) 
    {
        LoadRewardedAd();
    }
    void RewardedOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) 
    {
        Debug.LogWarning("Ad display failed" + error.ErrorMessage);
    }

    void RewardedOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward adReward) 
    {
        if(_onCurrentRewardedAddFinished != null) _onCurrentRewardedAddFinished();
        LoadRewardedAd();
    }

    void LoadRewardedAd()
    {
        _adIsLoading = true;
        _rewardedAd.LoadAd();
    }

    void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }
}
