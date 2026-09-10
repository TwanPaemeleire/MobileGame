using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public class RewardInfo
{
    public CurrencyType CurrencyType;
    public int Amount;
}

[CreateAssetMenu(fileName = "RewardTrackData", menuName = "CustomSOs/RewardTrackData")]
public class RewardTrackData : ScriptableObject
{
    public List<StatTrackEntryData> QuestEntries = new List<StatTrackEntryData>();
    public SerializedDictionary<int, RewardInfo> QuestProgresRewardEntries = new SerializedDictionary<int, RewardInfo>();
    public TimedDataType TimeScope;
}