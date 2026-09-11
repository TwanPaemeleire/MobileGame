using System;
using System.Collections;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;

public class UnityServicesHandler : MonoSingleton<UnityServicesHandler>
{
    private bool _initialized;
    public bool Initialized => _initialized;

    [SerializeField] private IAPHandler _IAPHandler;
    [SerializeField] private AdIntegrationHandler _adHandler;
    [SerializeField] private AuthenticationHandler _authenticationHandler;
    [SerializeField] private CloudSaveHandler _cloudSaveHandler;
    [SerializeField] private LeaderboardHandler _leaderboardHandler;

    public IAPHandler IapHandler => _IAPHandler;
    public AdIntegrationHandler AdHandler => _adHandler;
    public AuthenticationHandler AuthenticationHandler => _authenticationHandler;
    public CloudSaveHandler CloudSaveHandler => _cloudSaveHandler;
    public LeaderboardHandler LeaderboardHandler => _leaderboardHandler;


    public UnityEvent AllInitializationsDone = new UnityEvent();
    protected override void Init()
    {
        DoInitialize();
    }

    private async void DoInitialize()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return;
        }
        StartCoroutine(InitializationCoroutine());
    }

    private IEnumerator InitializationCoroutine()
    {

        _IAPHandler.Initialize();
        _adHandler.Initialize();

        _authenticationHandler.Initialize();
        yield return new WaitUntil(() => _authenticationHandler.IsSignedIn);
        _cloudSaveHandler.Initialize();
        _leaderboardHandler.Initialize();
        yield return new WaitUntil(() => PlayerDataHandler.Instance.DataLoaded);
        TimedResetHandler.Instance.Initialize();

        yield return new WaitUntil(() => AllInitializationsFinished());
        _initialized = true;
        AllInitializationsDone.Invoke();
    }

    private bool AllInitializationsFinished()
    {
        bool timedResetData = TimedResetHandler.Instance.IsReadyForUse;
        bool saveData = PlayerDataHandler.Instance.DataLoaded;
        bool authentication = _authenticationHandler.IsSignedIn;
        bool leaderboard = _leaderboardHandler.IsReadyForUse;
        bool ads = _adHandler.SDKInitialized;
        bool iap = _IAPHandler.StoreConnected;
        return timedResetData && saveData && authentication && leaderboard && ads && iap;
    }
}