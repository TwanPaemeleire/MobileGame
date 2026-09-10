using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TimedResetHandler : MonoSingleton<TimedResetHandler>
{
    private DateTime _serverStartDateUTC;
    private int _secondsUntilNextDailyReset = -1;
    private int _secondsUntilNextWeeklyReset = -1;

    public PlayerTimedResetData PlayerTimedResetData => PlayerDataHandler.Instance.PlayerTimedResetData;
    private bool _serverStartDateLoaded = false;
    public bool ServerStartDateLoaded => _serverStartDateLoaded;

    public UnityEvent OnDailyReset = new UnityEvent();
    public UnityEvent OnWeeklyReset = new UnityEvent();

    protected override void Init()
    {
        InitializeTime();
    }

    private async void InitializeTime()
    {
        Dictionary<string, Item> serverStartDateData = await CloudSaveService.Instance.Data.Custom.LoadAllAsync("ServerStartDate");
        if (serverStartDateData.TryGetValue("ServerStartDate", out var serverStartDateEntry))
        {
            string serverStartDateString = serverStartDateEntry.Value.GetAs<string>();
            _serverStartDateUTC = DateTime.Parse(serverStartDateString, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
        _serverStartDateLoaded = true;
        StartCoroutine(TimePassingCoroutine());
        Debug.Log($"Server Start Date: {_serverStartDateUTC.ToString("o")}");
    }

    public bool IsDailyResetDue()
    {
        TimeSpan elapsed = DateTime.UtcNow - _serverStartDateUTC;
        int daysSinceStart = (int)elapsed.TotalDays;
        int lastResetDay = PlayerTimedResetData.LastDailyResetDay;

        if (daysSinceStart > lastResetDay)
        {
            PlayerTimedResetData.LastDailyResetDay = daysSinceStart;
            RequestSavePlayerData();
            return true;
        }
        return false;
    }

    public bool IsWeeklyResetDue()
    {
        int weeksSinceStart = (int)((DateTime.UtcNow - _serverStartDateUTC).TotalDays / 7);
        int lastResetWeek = PlayerTimedResetData.LastWeeklyResetWeek;

        if (weeksSinceStart > lastResetWeek)
        {
            PlayerTimedResetData.LastWeeklyResetWeek = weeksSinceStart;
            RequestSavePlayerData();
            return true;
        }
        return false;
    }

    private async void RequestSavePlayerData()
    {
        await UnityServicesHandler.Instance.CloudSaveHandler.SavePlayerDataToCloud();
    }

    private void CheckResets()
    {
        if (IsDailyResetDue())
        {
            PlayerDataHandler.Instance.PlayerStatistics.ResetData(TimedDataType.Daily);
            OnDailyReset.Invoke();
        }
        if (IsWeeklyResetDue())
        {
            PlayerDataHandler.Instance.PlayerStatistics.ResetData(TimedDataType.Weekly);
            OnWeeklyReset.Invoke();
        }
    }

    // NOTE TO SELF: STORE ID OF ALL THE CLAIMED QUESTS IN THE TIMED DATE STUFF IN PLAYER DATA HANDLER, THEN MAKE EVERYTHING JUST DATA-DRIVEN

    private IEnumerator TimePassingCoroutine()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1f);

            _secondsUntilNextDailyReset = Mathf.Max(0, (int)((_serverStartDateUTC.AddDays((int)(DateTime.UtcNow - _serverStartDateUTC).TotalDays + 1)) - DateTime.UtcNow).TotalSeconds);

            _secondsUntilNextWeeklyReset = Mathf.Max(0,(int)((_serverStartDateUTC.AddDays(((int)((DateTime.UtcNow - _serverStartDateUTC).TotalDays / 7) + 1) * 7)) - DateTime.UtcNow).TotalSeconds);

            CheckResets();
        }
    }
}
