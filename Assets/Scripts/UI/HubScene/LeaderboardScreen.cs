using UnityEngine;
using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;

public class LeaderboardScreen : BaseScreen
{
    [SerializeField] private GameObject _UIentryPrefab;
    [SerializeField] private string _leaderboardName;
    protected override async void OnScreenOpenedInternal()
    {
        // Show loading animation
        List<LeaderboardEntry> entries = await UnityServicesHandler.Instance.LeaderboardHandler.GetEntries(_leaderboardName, 0, 100);
        LeaderboardEntry playerEntry = await UnityServicesHandler.Instance.LeaderboardHandler.GetPlayerEntry(_leaderboardName);
    }
}