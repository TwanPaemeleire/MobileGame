using System.Collections;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class LeaderboardScoreSubmitting : MonoBehaviour
{
    [SerializeField] private bool _deleteAllAccountsInsteadOfSubmittingScores = false;
    [SerializeField] private int _amount = 50;
    [SerializeField] private int _minScore = 1;
    [SerializeField] private int _maxScore = 40;

    [SerializeField] private TextMeshProUGUI _progressText;

    void Start()
    {
        StartCoroutine(SubmitLeaderboardScores());
    }

    private async void SubmitAllScores()
    {
        int count = 0;
        string password = "TestUser123?";
        _progressText.text = $"Submitted {count}/{_amount} scores";

        while (count < _amount)
        {
            string userName = "Player" + count;
            if (!_deleteAllAccountsInsteadOfSubmittingScores)
            {
                await UnityServicesHandler.Instance.AuthenticationHandler.StartSignUpAttempt(userName, password);
                await AuthenticationService.Instance.UpdatePlayerNameAsync(userName);
                await UnityServicesHandler.Instance.LeaderboardHandler.SubmitScore("TestLeaderboard", Random.Range(_minScore, _maxScore));
                UnityServicesHandler.Instance.AuthenticationHandler.SignOut();
            }
            else
            {
                await UnityServicesHandler.Instance.AuthenticationHandler.StartSignInAttempt(userName, password);
                await UnityServicesHandler.Instance.AuthenticationHandler.StartDeletAccountAttempt();
                UnityServicesHandler.Instance.AuthenticationHandler.SignOut();
            }

            ++count;
            _progressText.text = $"Submitted {count}/{_amount} scores";
        }
        _progressText.text = $"Submitted {count}/{_amount} scores. Done!";
    }

    private IEnumerator SubmitLeaderboardScores()
    {
        yield return UnityServicesHandler.Instance.AuthenticationHandler.StartSignInAttempt("Choclified", "Pamitwan3?");
        yield return new WaitUntil(() => UnityServicesHandler.Instance.Initialized);
        UnityServicesHandler.Instance.AuthenticationHandler.SignOut();
        SubmitAllScores();
    }
}
