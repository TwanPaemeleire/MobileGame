using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.Events;

public class LeaderboardHandler : MonoBehaviour
{
    public UnityEvent OnSuccesfullInit = new UnityEvent();
    private bool _isReadyForUse = false;
    public bool IsReadyForUse => _isReadyForUse;
    public void Initialize()
    {
        _isReadyForUse = true;
        OnSuccesfullInit.Invoke();
    }

    public async Task<LeaderboardEntry> SubmitScore(string leaderboardName, int score)
    {
        return await UnityServices.Instance.GetLeaderboardsService().AddPlayerScoreAsync(leaderboardName, score);
    }

    public async Task<List<LeaderboardEntry>> GetEntries(string leaderboardName, int startOffset, int amountOfEntries)
    {
        GetScoresOptions options = new GetScoresOptions();
        options.Offset = startOffset;
        options.Limit = amountOfEntries;
        LeaderboardScoresPage scoresPage = await UnityServices.Instance.GetLeaderboardsService().GetScoresAsync(leaderboardName, options);
        return scoresPage.Results;
    }

    public async Task<LeaderboardEntry> GetPlayerEntry(string leaderboardName)
    {
        return await UnityServices.Instance.GetLeaderboardsService().GetPlayerScoreAsync(leaderboardName);
    }
}