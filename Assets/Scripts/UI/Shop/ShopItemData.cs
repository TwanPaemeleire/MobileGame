using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemData", menuName = "CustomSOs/ShopItemData", order = 3)]
public class ShopItemData : ScriptableObject
{
    public CurrencyType CurrencyType = CurrencyType.Coins;
    public int Amount = 0;
    public string Title = string.Empty;
    public string Description = string.Empty;
    public Sprite ImageSprite = null;
    public float Price = 4.99f;
    public string PurchaseId = string.Empty;
}