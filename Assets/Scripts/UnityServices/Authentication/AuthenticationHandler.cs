using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;

public class AuthenticationHandler : MonoBehaviour
{
    public UnityEvent OnSignInCompleted = new UnityEvent();
    public UnityEvent OnSignInFailed = new UnityEvent();
    public UnityEvent OnSignOutCompleted = new UnityEvent();
    public UnityEvent OnExpired = new UnityEvent();

    private bool _isSignedIn = false;
    public bool IsSignedIn => _isSignedIn;
    private bool _wasSignUp = false;

    private string _username = string.Empty;
    private string _cachedPlayerName = string.Empty;

    public void Initialize()
    {
        AuthenticationService.Instance.SignedIn += OnSignedIn;
        AuthenticationService.Instance.SignInFailed += OnSignInFail;
        AuthenticationService.Instance.SignedOut += OnSignedOut;
        AuthenticationService.Instance.Expired += OnExpire;
    }

    public string GetPlayerName()
    {
        return _cachedPlayerName;
    }

    public string GetPlayerNameWithoutId()
    {
        return GetPlayerName().Substring(0, GetPlayerName().LastIndexOf("#"));
    }

    public string GetPlayerId()
    {
        return AuthenticationService.Instance.PlayerId;
    }

    private async void OnSignedIn()
    {
        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        try
        {
            if (_wasSignUp)
            {
                await UnityServicesHandler.Instance.CloudSaveHandler.SavePlayerDataToCloud();
                await AuthenticationService.Instance.UpdatePlayerNameAsync(_username);
            }
            _cachedPlayerName = await AuthenticationService.Instance.GetPlayerNameAsync();
            Debug.Log($"PlayerName: {_cachedPlayerName}");

            _isSignedIn = true;
            OnSignInCompleted.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            OnSignInFailed.Invoke();
        }
    }

    private void OnSignInFail(RequestFailedException exception)
    {
        Debug.LogError(exception);
        OnSignInFailed.Invoke();
    }

    private void OnSignedOut()
    {
        Debug.Log("Player signed out");
        OnSignOutCompleted.Invoke();
    }

    private void OnExpire()
    {
        Debug.Log("Session expired");
        OnExpired.Invoke();
    }

    public async Task StartSignUpAttempt(string username, string password)
    {
        try
        {
            _wasSignUp = true;
            _username = username;
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public async Task StartSignInAttempt(string username, string password)
    {
        try
        {
            _wasSignUp = false;
            _username = username;
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}