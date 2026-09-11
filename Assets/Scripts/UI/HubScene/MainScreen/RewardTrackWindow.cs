using UnityEngine;
using System.Collections.Generic;

public class RewardTrackWindow : PopUpWindow
{
    [SerializeField] private GameObject _questEntryPrefab;
    [SerializeField] private Transform _questContainerTransform;
    [SerializeField] private RewardTrackData _rewardTrackData;

    private List<StatTrackUIElement> _questEntryComponents = new List<StatTrackUIElement>();

    protected override void OnWindowOpenedInternal()
    {
        List<StatTrackUIElement> claimedElements = new List<StatTrackUIElement>();
        foreach (var quest in _rewardTrackData.QuestEntries)
        {
            GameObject questEntryObject = Instantiate(_questEntryPrefab, _questContainerTransform);
            var questEntryComponent = questEntryObject.GetComponent<StatTrackUIElement>();
            questEntryComponent.InitializeElement(quest);
            _questEntryComponents.Add(questEntryComponent);
            if (PlayerDataHandler.Instance.PlayerQuests.IsQuestCompleted(_rewardTrackData.TimeScope, quest.name))
            {
                claimedElements.Add(questEntryComponent);
            }
        }

        foreach(var claimedElement in claimedElements)
        {
            claimedElement.MarkAsClaimed();
        }

        PlayerDataHandler.Instance.PlayerStatistics.OnStatIncreased.AddListener(OnStatChanged);
    }

    protected override void OnWindowClosedInternal()
    {
        foreach (var questEntry in _questEntryComponents)
        {
            Destroy(questEntry.gameObject);
        }
        _questEntryComponents.Clear();
    }

    private void OnStatChanged(string statName, int newValue, TimedDataType type)
    {
        if (_rewardTrackData.TimeScope != type) return;
        foreach (var questEntry in _questEntryComponents)
        {
            if (questEntry.StatTrackEntryData.StatName == statName)
            {
                questEntry.ChangeValue(newValue);
            }
        }
    }
}