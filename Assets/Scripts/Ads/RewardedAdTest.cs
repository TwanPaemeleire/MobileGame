using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardedAdTest : MonoSingleton<RewardedAdTest>
{
    [SerializeField]
    private Button _rewardedAdButton;

    [SerializeField]
    private TextMeshProUGUI _coinsText;

    private int _amountOfCoins = 0;

    private void Start()
    {
        SetAddButtonInactive();
        AdIntegrationHandler.Instance.OnRewardedAdFinishedLoading.AddListener(SetAddButtonActive);
    }

    public void OnRewardedAdButtonClicked()
    {
        AdIntegrationHandler.Instance.PlayRewardedAd(() => AddCoins(20));
    }

    public void AddCoins(int coinsToAdd, bool setInactive = true)
    {
        _amountOfCoins += coinsToAdd;
        _coinsText.text = "Coins: " + _amountOfCoins;
        if (!setInactive) return;
        SetAddButtonInactive();
    }

    private void SetAddButtonInactive()
    {
        _rewardedAdButton.interactable = false;
        _rewardedAdButton.GetComponentInChildren<TextMeshProUGUI>().text = "Loading ad...";
    }

    private void SetAddButtonActive()
    {
        _rewardedAdButton.interactable = true;
        _rewardedAdButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ad loaded!";
    }
}
