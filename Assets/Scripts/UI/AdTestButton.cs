using UnityEngine;

public class AdTestButton : MonoBehaviour
{
   public async void RequestStartAd()
   {
        UnityServicesHandler.Instance.AdHandler.PlayRewardedAd(() => PlayerDataHandler.Instance.PlayerStatistics.IncreaseStatValueAllTimeScopes("AdsWatched", 1));
        await UnityServicesHandler.Instance.CloudSaveHandler.SavePlayerDataToCloud();
   }
}
