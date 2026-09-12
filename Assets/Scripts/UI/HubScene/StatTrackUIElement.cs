using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class StatTrackUIElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private RewardElement _trackRewardElement;
    [SerializeField] private RewardElement _regularRewardElement;
    [SerializeField] private Button _claimButton;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private CurrencyInfoCollection _currencyInfoCollection;
    private StatTrackEntryData _entry;
    public StatTrackEntryData StatTrackEntryData => _entry;
    private bool _claimed = false;

    public void InitializeElement(StatTrackEntryData entry)
    {
        _entry = entry;
        _descriptionText.text = _entry.Description;
        _progressBar.InitializeProgressBar(0, _entry.TargetAmount);
        _regularRewardElement.AmountText.text = _entry.Reward.Amount.ToString();
        _regularRewardElement.IconImage.sprite = _currencyInfoCollection.CurrencyInfoDictionary[_entry.Reward.CurrencyType];
        _trackRewardElement.AmountText.text = _entry.ContributionToTrack.ToString();

        int currentValue = PlayerDataHandler.Instance.PlayerStatistics.GetStatValue(_entry.StatName, TimedDataType.Daily);
        ChangeValue(currentValue);
        if (PlayerDataHandler.Instance.PlayerQuests.IsQuestCompleted(TimedDataType.Daily, _entry.name))
        {
            MarkAsClaimed();
        }
    }

    public void ChangeValue(int newValue)
    {
        _progressBar.SetValue(newValue);
        if (!_claimed && newValue >= _entry.TargetAmount)
        {
            _claimButton.interactable = true;
        }
    }

    public void ClaimReward()
    {
        MarkAsClaimed();
        PlayerDataHandler.Instance.PlayerCurrency.AddCurrency(_entry.Reward.Amount, _entry.Reward.CurrencyType);
        PlayerDataHandler.Instance.PlayerQuests.AddCompletedQuest(TimedDataType.Daily, _entry.name);
        List<RewardData> rewards = new();
        RewardData rewardData = new RewardData();
        rewardData.Amount = _entry.Reward.Amount;
        rewardData.CurrencyType = _entry.Reward.CurrencyType;
        rewards.Add(rewardData);
        PopUpWindowRequestHandler.Instance.RequestRewardPopUpWindow(rewards);
    }

    public void MarkAsClaimed()
    {
        Transform parent = transform.parent;
        transform.SetParent(null);
        transform.SetParent(parent);
        _claimButton.interactable = false;
        _claimed = true;
    }
}