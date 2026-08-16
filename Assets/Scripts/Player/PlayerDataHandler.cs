using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class PlayerDataCollection
{
    public PlayerInventoryData Inventory;
    public PlayerCurrencyData Currency;
}

[System.Serializable]
public class PlayerInventoryData
{
    public int Item = 90;
}

[System.Serializable]
public class PlayerCurrencyData
{
    public int Coins = 10;
}

public class PlayerDataHandler : MonoSingleton<PlayerDataHandler>
{
    // Player data collection
    private PlayerDataCollection _playerDataCollection = new PlayerDataCollection();
    public PlayerDataCollection PlayerDataCollection => _playerDataCollection;
    private bool _dataLoaded = false;
    public bool DataLoaded => _dataLoaded;

    // Player data parts
    public PlayerInventoryData PlayerInventory {get { return _playerDataCollection.Inventory; } set { _playerDataCollection.Inventory = value; } }

    // File saving & loading
    string _saveFileName = "SaveFile.json";

    private Task _saveTask = null;
    public Task SaveTask => _saveTask;

    private Task<string> _loadTask = null;
    public Task LoadTask => _loadTask;

    protected override void Init()
    {
        RequestSave();
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
        Debug.Log(saveFileJson);
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
