using UnityEngine;

public class AdTestButton : MonoBehaviour
{
   public void RequestStartAd()
    {
        UnityServicesHandler.Instance.AdHandler.PlayRewardedAd(() => PlayerDataHandler.Instance.PlayerStatistics.IncreaseStatValue("AdsWatched", 1));
    }
}
