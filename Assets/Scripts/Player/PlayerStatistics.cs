using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public PlayerStatisticsData CurrencyData => PlayerDataHandler.Instance.PlayerStatisticsData;

    public int GetDailyStatValue(string statName)
    {
        if(!CurrencyData.DailyStatistics.ContainsKey(statName))
        {
            Debug.LogWarning($"Statistic '{statName}' does not exist in the daily statistics dictionary");
            CurrencyData.DailyStatistics.Add(statName, 0);
        }
        return CurrencyData.DailyStatistics[statName];
    }
}