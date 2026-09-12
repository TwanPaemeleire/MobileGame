using System.Collections.Generic;
using UnityEngine;

public class PopUpWindowRequestHandler : MonoSingleton<PopUpWindowRequestHandler>
{
    [SerializeField] private RewardPopUpWindow _rewardPopUp;

    public void RequestRewardPopUpWindow(List<RewardData> rewardsData)
    {
        _rewardPopUp.OpenWindow();
        _rewardPopUp.Initialize(rewardsData);
    }
}