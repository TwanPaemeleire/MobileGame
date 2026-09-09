using UnityEngine;

public class RewardTrackWindow : PopUpWindow
{
    [SerializeField] private GameObject _questEntryPrefab;
    [SerializeField] private Transform _questContainerTransform;
    [SerializeField] private RewardTrackData _rewardTrackData;

    private bool _haveMadeQuests = false;

    protected override void OnWindowOpenedInternal()
    {
        if (_haveMadeQuests) return;
        _haveMadeQuests = true;
        foreach (var quest in _rewardTrackData.QuestEntries)
        {
            GameObject questEntryObject = Instantiate(_questEntryPrefab, _questContainerTransform);
            var questEntryComponent = questEntryObject.GetComponent<StatTrackUIElement>();
            questEntryComponent.CheckValuesAndRefreshVisuals(quest);
        }
    }
}