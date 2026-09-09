using TMPro;
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

    public void CheckValuesAndRefreshVisuals(StatTrackEntryData entry)
    {
        _entry = entry;
        _descriptionText.text = _entry.Description;
        int currentValue = PlayerDataHandler.Instance.PlayerStatistics.GetDailyStatValue(_entry.StatName);
        _progressBar.InitializeProgressBar(currentValue, _entry.TargetAmount);
        _regularRewardElement.AmountText.text = _entry.Reward.Amount.ToString();
        _regularRewardElement.IconImage.sprite = _currencyInfoCollection.CurrencyInfoDictionary[_entry.Reward.CurrencyType];
        _trackRewardElement.AmountText.text = _entry.ContributionToTrack.ToString();

        if (currentValue >= _entry.TargetAmount)
        {
            _claimButton.interactable = true;
        }
    }

    public void OnValueChanged(string statName, int newValue)
    {
        if (statName == _entry.StatName)
        {
            _progressBar.SetValue(newValue);
            if (newValue >= _entry.TargetAmount)
            {
                _claimButton.interactable = true;
            }
        }
    }

    public void ClaimReward()
    {
        gameObject.SetActive(false);
    }
}