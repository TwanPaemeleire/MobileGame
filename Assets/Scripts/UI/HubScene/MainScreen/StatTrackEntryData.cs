using AYellowpaper.SerializedCollections;
using UnityEngine;

[System.Serializable]
public class RewardData
{
    public CurrencyType CurrencyType;
    public int Amount;
}

[CreateAssetMenu(fileName = "StatTrackEntryData", menuName = "CustomSOs/StatTrackEntryData")]
public class StatTrackEntryData : ScriptableObject
{
    public string Description = string.Empty;
    public string StatName = string.Empty;
    public int TargetAmount = 0;
    public int ContributionToTrack = 0;
    public RewardData Reward;
}