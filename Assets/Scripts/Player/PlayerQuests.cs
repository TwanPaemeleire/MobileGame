using UnityEngine;
using System.Collections.Generic;

public class PlayerQuests : MonoBehaviour
{
    public PlayerQuestData QuestData => PlayerDataHandler.Instance.PlayerQuestsData;

    public async void AddCompletedQuest(TimedDataType timeScope, string questId)
    {
        if(!QuestData.QuestsData.ContainsKey(timeScope)) QuestData.QuestsData.Add(timeScope, new List<string>());
        if (QuestData.QuestsData[timeScope].Contains(questId)) return;
        QuestData.QuestsData[timeScope].Add(questId);
        await UnityServicesHandler.Instance.CloudSaveHandler.SavePlayerDataToCloud();
    }

    public bool IsQuestCompleted(TimedDataType timeScope, string questId)
    {
        if (!QuestData.QuestsData.ContainsKey(timeScope)) QuestData.QuestsData.Add(timeScope, new List<string>());
        return QuestData.QuestsData[timeScope].Contains(questId);
    }

    public void ResetData(TimedDataType timeScope)
    {
        if (QuestData.QuestsData.ContainsKey(timeScope)) QuestData.QuestsData[timeScope].Clear();
    }
}