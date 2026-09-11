using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.Events;

public class CloudSaveHandler : MonoBehaviour
{
    public UnityEvent OnSuccessfullInit = new UnityEvent();
    public void Initialize()
    {
        DoInitialization();
    }

    private async void DoInitialization()
    {
        await LoadPlayerDataFromCloud();
        OnSuccessfullInit.Invoke();
    }

    public async Task SavePlayerDataToCloud()
    {
        var playerData = new Dictionary<string, object>
        {
            {"InventoryData", JsonUtility.ToJson(PlayerDataHandler.Instance.PlayerInventoryData)},
            {"CurrencyData", JsonUtility.ToJson(PlayerDataHandler.Instance.PlayerCurrencyData)},
            {"StatisticsData", JsonUtility.ToJson(PlayerDataHandler.Instance.PlayerStatisticsData)},
            {"TimedResetData", JsonUtility.ToJson(PlayerDataHandler.Instance.PlayerTimedResetData)},
            {"QuestsData", JsonUtility.ToJson(PlayerDataHandler.Instance.PlayerQuestsData) }
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
    }

    public async Task LoadPlayerDataFromCloud()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> 
        {
          "InventoryData", "CurrencyData", "StatisticsData", "TimedResetData", "QuestsData"
        });

        string dataJson = "";
        if (playerData.TryGetValue("InventoryData", out var firstKey))
        {
            dataJson = firstKey.Value.GetAs<string>();
            PlayerDataHandler.Instance.PlayerDataCollection.Inventory = JsonUtility.FromJson<PlayerInventoryData>(dataJson);
        }

        if (playerData.TryGetValue("CurrencyData", out var secondKey))
        {
            dataJson = secondKey.Value.GetAs<string>();
            PlayerDataHandler.Instance.PlayerDataCollection.Currency = JsonUtility.FromJson<PlayerCurrencyData>(dataJson);
        }

        if (playerData.TryGetValue("StatisticsData", out var thirdKey))
        {
            dataJson = thirdKey.Value.GetAs<string>();
            PlayerDataHandler.Instance.PlayerDataCollection.Statistics = JsonUtility.FromJson<PlayerStatisticsData>(dataJson);
        }

        if(playerData.TryGetValue("QuestsData", out var fourthKey))
        {
            dataJson = fourthKey.Value.GetAs<string>();
            PlayerDataHandler.Instance.PlayerDataCollection.Quests = JsonUtility.FromJson<PlayerQuestData>(dataJson);
        }

        if(playerData.TryGetValue("TimedResetData", out var fifthKey))
        {
            dataJson = fifthKey.Value.GetAs<string>();
            PlayerDataHandler.Instance.PlayerDataCollection.TimedReset = JsonUtility.FromJson<PlayerTimedResetData>(dataJson);
        }

        PlayerDataHandler.Instance.DataLoaded = true;
    }
}
