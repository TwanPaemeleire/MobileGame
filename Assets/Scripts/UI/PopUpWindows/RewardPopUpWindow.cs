using AYellowpaper.SerializedCollections;
using UnityEngine;

public class RewardPopUpWindow : MonoBehaviour
{
    [SerializeField] private GameObject _rewardItemUIPrefab;
    [SerializeField] private RectTransform _rewardItemUIParent;
    [SerializeField] private CurrencyInfoCollection _currencyInfoCollection;
    public void Initialize(SerializedDictionary<CurrencyType, int> rewardData)
    {
        foreach (var reward in rewardData)
        {
            GameObject rewardItemUI = Instantiate(_rewardItemUIPrefab, _rewardItemUIParent);
            RewardElement rewardElement = rewardItemUI.GetComponent<RewardElement>();
            rewardElement.AmountText.text = reward.Value.ToString();
            rewardElement.IconImage.sprite = _currencyInfoCollection.CurrencyInfoDictionary[reward.Key];
        }
    }
}