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

    public void Initialize()
    {
        AuthenticationService.Instance.SignedIn += OnSignedIn;
        AuthenticationService.Instance.SignInFailed += OnSignInFail;
        AuthenticationService.Instance.SignedOut += OnSignedOut;
        AuthenticationService.Instance.Expired += OnExpire;
    }

    private async void OnSignedIn()
    {
        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        if(_wasSignUp) await UnityServicesHandler.Instance.CloudSaveHandler.SavePlayerDataToCloud();
        _isSignedIn = true;
        OnSignInCompleted.Invoke();
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
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public async Task StartSignInAttempt(string username, string password)
    {
        try
        {
            _wasSignUp = false;
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}