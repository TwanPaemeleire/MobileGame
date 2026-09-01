using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.Events;

public class CloudSaveHandler : MonoBehaviour
{
    public UnityEvent OnSuccessfullInit = new UnityEvent();
    public async Task Initialize()
    {
        await LoadPlayerDataFromCloud();
        OnSuccessfullInit.Invoke();
    }

    public async Task SavePlayerDataToCloud()
    {
        var playerData = new Dictionary<string, object>
        {
            {"InventoryData", JsonUtility.ToJson(PlayerDataHandler.Instance.PlayerInventoryData)},
            {"CurrencyData", JsonUtility.ToJson(PlayerDataHandler.Instance.PlayerCurrencyData)}
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
    }

    public async Task LoadPlayerDataFromCloud()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> 
        {
          "InventoryData", "CurrencyData"
        });

        string dataJson = "";
        if (playerData.TryGetValue("InventoryData", out var firstKey))
        {
            dataJson = firstKey.Value.GetAs<string>();
            Debug.Log($"InventoryData value: {dataJson}");
            PlayerDataHandler.Instance.PlayerDataCollection.Inventory = JsonUtility.FromJson<PlayerInventoryData>(dataJson);
        }

        if (playerData.TryGetValue("CurrencyData", out var secondKey))
        {
            dataJson = secondKey.Value.GetAs<string>();
            Debug.Log($"CurrencyData value: {dataJson}");
            PlayerDataHandler.Instance.PlayerDataCollection.Currency = JsonUtility.FromJson<PlayerCurrencyData>(dataJson);
        }

        PlayerDataHandler.Instance.DataLoaded = true;
    }
}
