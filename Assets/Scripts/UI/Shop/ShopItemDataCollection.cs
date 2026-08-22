using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShopItemDataCollection", menuName = "CustomSOs/ShopItemDataCollection", order = 2)]
public class ShopItemDataCollection : ScriptableObject
{
    public List<ShopItemData> Items;
}