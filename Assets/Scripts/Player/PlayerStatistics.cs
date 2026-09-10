using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatistics : MonoBehaviour
{
    public PlayerStatisticsData CurrencyData => PlayerDataHandler.Instance.PlayerStatisticsData;

    public UnityEvent<string, int, TimedDataType> OnStatIncreased = new UnityEvent<string, int, TimedDataType>(); // Statname, new value, time scope type

    public int GetStatValue(string statName, TimedDataType type)
    {
        EnsureStatPresence(statName, type);
        return CurrencyData.Statistics[type][statName];
    }

    public void IncreaseStatValue(string statName, int amount, TimedDataType type)
    {
        EnsureStatPresence(statName, type);
        CurrencyData.Statistics[type][statName] += amount;
        OnStatIncreased.Invoke(statName, CurrencyData.Statistics[type][statName], type);
    }

    public void IncreaseStatValueAllTimeScopes(string statName, int amount)
    {
        foreach (TimedDataType type in Enum.GetValues(typeof(TimedDataType)))
        {
            EnsureStatPresence(statName, type);
            CurrencyData.Statistics[type][statName] += amount;
            OnStatIncreased.Invoke(statName, CurrencyData.Statistics[type][statName], type);
        }
    }

    public void ResetData(TimedDataType type)
    {
        if (CurrencyData.Statistics.ContainsKey(type)) CurrencyData.Statistics.Remove(type);
    }

    private void EnsureStatPresence(string statName, TimedDataType type)
    {
        if (!CurrencyData.Statistics.ContainsKey(type)) CurrencyData.Statistics.Add(type, new());
        if (!CurrencyData.Statistics[type].ContainsKey(statName))
        {
            CurrencyData.Statistics[type].Add(statName, 0);
        }
    }
}