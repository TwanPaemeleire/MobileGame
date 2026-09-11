using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class PlayerDataCollection
{
    public PlayerStatisticsData Statistics = new PlayerStatisticsData();
    public PlayerInventoryData Inventory = new PlayerInventoryData();
    public PlayerCurrencyData Currency = new PlayerCurrencyData();
    public PlayerTimedResetData TimedReset = new PlayerTimedResetData();
    public PlayerQuestData Quests = new PlayerQuestData();
}

[System.Serializable]
public class PlayerStatisticsData
{
    public SerializedDictionary<TimedDataType, SerializedDictionary<string, int>> Statistics = new SerializedDictionary<TimedDataType, SerializedDictionary<string, int>>();
}

[System.Serializable]
public class PlayerQuestData
{
    public SerializedDictionary<TimedDataType, List<string>> QuestsData = new SerializedDictionary<TimedDataType, List<string>>();
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
        { CurrencyType.Coins, 0},
        { CurrencyType.Gems, 0}
    };
}

[System.Serializable]
public enum TimedDataType
{
    Daily,
    Weekly,
    Lifetime
}

[System.Serializable]
public class PlayerTimedResetData
{
    public int LastDailyResetDay = -1;
    public int LastWeeklyResetWeek = -1;
}

public class PlayerDataHandler : MonoSingleton<PlayerDataHandler>
{
    // Player data handlers
    [SerializeField] private PlayerStatistics _playerStatistics;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerCurrency _playerCurrency;
    [SerializeField] private PlayerQuests _playerQuests;

    public PlayerStatistics PlayerStatistics => _playerStatistics;
    public PlayerInventory PlayerInventory => _playerInventory;
    public PlayerCurrency PlayerCurrency => _playerCurrency;
    public PlayerQuests PlayerQuests => _playerQuests;

    // Player data collection
    private PlayerDataCollection _playerDataCollection = new PlayerDataCollection();
    public PlayerDataCollection PlayerDataCollection { get { return _playerDataCollection; } set { _playerDataCollection = value; } }
    private bool _dataLoaded = false;
    public bool DataLoaded { get { return _dataLoaded; } set { _dataLoaded = value;} }

    // Player data parts
    public PlayerInventoryData PlayerInventoryData { get { return _playerDataCollection.Inventory; } set { _playerDataCollection.Inventory = value; } }
    public PlayerCurrencyData PlayerCurrencyData { get { return _playerDataCollection.Currency; } set { _playerDataCollection.Currency = value; } }
    public PlayerStatisticsData PlayerStatisticsData { get { return _playerDataCollection.Statistics; } set { _playerDataCollection.Statistics = value; } }
    public PlayerTimedResetData PlayerTimedResetData { get { return _playerDataCollection.TimedReset; } set { _playerDataCollection.TimedReset = value; } }
    public PlayerQuestData PlayerQuestsData => _playerDataCollection.Quests;
}