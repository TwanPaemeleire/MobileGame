using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPopUpWindow : PopUpWindow
{
    [SerializeField] private GameObject _rewardItemUIPrefab;
    [SerializeField] private RectTransform _rewardItemUIParent;
    [SerializeField] private CurrencyInfoCollection _currencyInfoCollection;
    [SerializeField] private float _delayBetweenRewardSpawns = 0.1f;

    public void Initialize(List<RewardData> rewardsData)
    {
        StopAllCoroutines();
        StartCoroutine(RewardsAppearCoroutine(rewardsData));
    }

    protected override void OnWindowClosedInternal()
    {
        for(int index = 0; index < _rewardItemUIParent.childCount; ++index)
        {
            Destroy(_rewardItemUIParent.GetChild(index).gameObject);
        }
    }

    private IEnumerator RewardsAppearCoroutine(List<RewardData> rewardsData)
    {
        foreach (var reward in rewardsData)
        {
            GameObject rewardItemUI = Instantiate(_rewardItemUIPrefab, _rewardItemUIParent);
            RewardElement rewardElement = rewardItemUI.GetComponent<RewardElement>();
            rewardElement.AmountText.text = reward.Amount.ToString();
            rewardElement.IconImage.sprite = _currencyInfoCollection.CurrencyInfoDictionary[reward.CurrencyType];
            rewardElement.DoPopAnimation();
            yield return new WaitForSeconds(_delayBetweenRewardSpawns);
        }
    }
}