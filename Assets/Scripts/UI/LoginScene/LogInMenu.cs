using TMPro;
using UnityEngine;

public class LogInMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _username;
    [SerializeField] private TMP_InputField _password;

    private void Start()
    {
        UnityServicesHandler.Instance.AuthenticationHandler.OnSignInCompleted.AddListener(OnSignInComplete);
    }

    public async void OnLoginPressed()
    {
        await UnityServicesHandler.Instance.AuthenticationHandler.StartSignInAttempt(_username.text, _password.text);
    }

    public async void OnRegisterPressed()
    {
        await UnityServicesHandler.Instance.AuthenticationHandler.StartSignUpAttempt(_username.text, _password.text);
    }

    private void OnSignInComplete()
    {
        gameObject.SetActive(false);
    }
}
