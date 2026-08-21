using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class LeaderboardHandler : MonoBehaviour
{
    public void Initialize()
    {
        //try
        //{
        //    LeaderboardEntry entry = await UnityServices.Instance.GetLeaderboardsService().AddPlayerScoreAsync("TestLeaderboard", 20);
        //
        //    Debug.Log($"Rank: {entry.Rank} Name: {entry.PlayerName} Score: {entry.Score}");
        //}
        //catch (Exception e)
        //{
        //    Debug.LogException(e);
        //}
        //LeaderboardEntry entry = await SubmitScore("TestLeaderboard", 20);
        //Debug.Log("After submit");
        //Debug.Log("Rank: " + entry.Rank + " Name: " + entry.PlayerName + " Score: " + entry.Score);
    }

    public async Task<LeaderboardEntry> SubmitScore(string leaderboardName, int score)
    {
        return await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardName, score);
    }

    public async Task<List<LeaderboardEntry>> GetEntries(string leaderboardName, int startOffset, int amountOfEntries)
    {
        GetScoresOptions options = new GetScoresOptions();
        options.Offset = startOffset;
        options.Limit = amountOfEntries;
        LeaderboardScoresPage scoresPage = await LeaderboardsService.Instance.GetScoresAsync(leaderboardName, options);
        return scoresPage.Results;
    }

    public async Task<LeaderboardEntry> GetPlayerEntry(string leaderboardName)
    {
        return await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardName);
    }
}