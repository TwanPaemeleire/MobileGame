using UnityEngine;
using UnityEngine.Events;

public class PlayerCurrency : MonoBehaviour
{
    public PlayerCurrencyData CurrencyData {  get { return PlayerDataHandler.Instance.PlayerCurrencyData; } set { PlayerDataHandler.Instance.PlayerCurrencyData = value; } }

    public UnityEvent<CurrencyType, int, int> OnCurrencyGained = new UnityEvent<CurrencyType, int, int>(); // Type, old amount, new amount
    public UnityEvent<CurrencyType, int, int> OnCurrencyLost = new UnityEvent<CurrencyType, int, int>(); // Type, old amount, new amount

    public bool CanAfford(int cost, CurrencyType type)
    {
        return CurrencyData.Currencies[type] >= cost;
    }

    public bool TryPurchase(int cost, CurrencyType type)
    {
        if (!CanAfford(cost, type)) return false;
        int prev = CurrencyData.Currencies[type];
        CurrencyData.Currencies[type] -= cost;
        OnCurrencyLost.Invoke(type, prev, CurrencyData.Currencies[type]);
        return true;
    }

    public int GetCurrencyAmount(CurrencyType type)
    {
        return CurrencyData.Currencies[type];
    }

    public void AddCurrency(int amount, CurrencyType type)
    {
        int prev = CurrencyData.Currencies[type];
        CurrencyData.Currencies[type] += amount;
        OnCurrencyGained.Invoke(type, prev, CurrencyData.Currencies[type]);
    }
}
