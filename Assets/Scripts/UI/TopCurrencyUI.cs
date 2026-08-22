using System;
using TMPro;
using UnityEngine;

public class TopCurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    private void Start()
    {
        PlayerCurrency.Instance.OnCurrencyGained.AddListener(OnCurrencyGained);
    }

    private void OnCurrencyGained(CurrencyType type, int oldAmount, int newAmount)
    {
        _coinsText.text = newAmount.ToString();
    }
}
