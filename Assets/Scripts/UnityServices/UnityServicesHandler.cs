using System;
using System.Collections;
using Unity.Services.Core;
using UnityEngine;

public class UnityServicesHandler : MonoSingleton<UnityServicesHandler>
{
    private bool _initialized;
    public bool Initialized => _initialized;

    [SerializeField] private IAPHandler _IAPHandler;
    [SerializeField] private AdIntegrationHandler _adHandler;
    [SerializeField] private AuthenticationHandler _authenticationHandler;
    [SerializeField] private CloudSaveHandler _cloudSaveHandler;

    public IAPHandler IapHandler => _IAPHandler;
    public AdIntegrationHandler AdHandler => _adHandler;
    public AuthenticationHandler AuthenticationHandler => _authenticationHandler;
    public CloudSaveHandler CloudSaveHandler => _cloudSaveHandler;

    private float _loadProgress = 0.0f;
    public float LoadProgress => _loadProgress;

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
        float targetAmount = 4.0f;
        float loadProgress = 0.0f;
        // Can be used to progress a loading bar or something in here
        yield return _IAPHandler.Initialize();
        _loadProgress = ++loadProgress / targetAmount;
        Debug.Log("IAP Init done");
        _adHandler.Initialize();
        yield return new WaitUntil(() => _adHandler.SDKInitialized);
        _loadProgress = ++loadProgress / targetAmount;
        Debug.Log("Ad Init done");
        _authenticationHandler.Initialize();
        yield return new WaitUntil(() => _authenticationHandler.IsSignedIn);
        _loadProgress = ++loadProgress / targetAmount;
        Debug.Log("Auth Init done");
        yield return _cloudSaveHandler.Initialize();
        _loadProgress = ++loadProgress / targetAmount;
        Debug.Log("CloudSave Init done");
        _initialized = true;
    }
}