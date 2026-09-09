using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyInfoCollection", menuName = "CustomSOs/CurrencyInfoCollection")]
public class CurrencyInfoCollection : ScriptableObject
{
    public SerializedDictionary<CurrencyType, Sprite> CurrencyInfoDictionary;
}
