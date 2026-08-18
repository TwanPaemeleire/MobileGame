using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PlayerDataCollection
{
    public PlayerInventoryData Inventory = new PlayerInventoryData();
    public PlayerCurrencyData Currency = new PlayerCurrencyData();
}

[System.Serializable]
public class PlayerInventoryData
{
    public int Item = 90;
}

[System.Serializable]
public enum CurrencyType
{
    Coins, 
    Gems
}

[System.Serializable]
public class PlayerCurrencyData
{
    public SerializedDictionary<CurrencyType, int> Currencies = new SerializedDictionary<CurrencyType, int>()
    {
        { CurrencyType.Coins, 5000},
        { CurrencyType.Gems, 2}
    };
}

public class PlayerDataHandler : MonoSingleton<PlayerDataHandler>
{
    // Player data collection
    private PlayerDataCollection _playerDataCollection = new PlayerDataCollection();
    public PlayerDataCollection PlayerDataCollection { get { return _playerDataCollection; } set { _playerDataCollection = value; } }
    private bool _dataLoaded = false;
    public bool DataLoaded { get { return _dataLoaded; } set { _dataLoaded = value; } }

    // Player data parts
    public PlayerInventoryData PlayerInventoryData { get { return _playerDataCollection.Inventory; } set { _playerDataCollection.Inventory = value; } }
    public PlayerCurrencyData PlayerCurrencyData { get { return _playerDataCollection.Currency; } set { _playerDataCollection.Currency = value; } }

    // File saving & loading
    string _saveFileName = "SaveFile.json";

    private Task _saveTask = null;
    public Task SaveTask => _saveTask;

    private Task<string> _loadTask = null;
    public Task LoadTask => _loadTask;

    protected override void Init()
    {
        //RequestSave();
        //RequestLoad();
    }

    public async void RequestSave()
    {
        await Save();
    }

    private async Task Save()
    {
        string saveFileJson = JsonUtility.ToJson(PlayerDataCollection);
        _saveTask = File.WriteAllTextAsync(Application.persistentDataPath + _saveFileName, saveFileJson);
        await SaveTask;
        _saveTask = null;
    }

    public async void RequestLoad()
    {
        await Load();
    }

    private async Task Load()
    {
        if (_loadTask != null) return;
        _loadTask = File.ReadAllTextAsync(Application.persistentDataPath + _saveFileName);
        await _loadTask;
        string saveFileJson = _loadTask.Result;
        _loadTask = null;
        _playerDataCollection = JsonUtility.FromJson<PlayerDataCollection>(saveFileJson);
        _dataLoaded = true;
    }
}
