using AYellowpaper.SerializedCollections;
using UnityEngine;

public class PopUpWindowRequestHandler : MonoSingleton<PopUpWindowRequestHandler>
{
    [SerializeField] private GameObject _rewardPopUpPrefab;

    public void RequestRewardPopUpWindow(SerializedDictionary<CurrencyType, int> rewardData)
    {
        GameObject popUpObject = Instantiate(_rewardPopUpPrefab);
        RewardPopUpWindow rewardPopUpWindow = popUpObject.GetComponent<RewardPopUpWindow>();
        rewardPopUpWindow.Initialize(rewardData);
    }
}