using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopScreen : BaseScreen
{
    [SerializeField] private ShopItemDataCollection _collection;
    [SerializeField] private GameObject _shopItemPrefab;
    [SerializeField] private Transform _itemParent;

    private ShopItemData _processingItemData;

    private void Start()
    {
        foreach (ShopItemData data in _collection.Items)
        {
            var gameObject = Instantiate(_shopItemPrefab, _itemParent);
            ShopItem item = gameObject.GetComponentInChildren<ShopItem>();
            item.TitleText.text = data.Title;
            item.DescriptionText.text = data.Description;
            item.PriceText.text = "$" + data.Price;
            item.Image.sprite = data.ImageSprite;
            item.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                OnPress(data);
            });
        }
    }

    private void OnPress(ShopItemData shopItemData)
    {
        _processingItemData = shopItemData;
        UnityServicesHandler.Instance.IapHandler.Purchase(shopItemData.PurchaseId, OnPurchasePending);
    }

    private bool OnPurchasePending(PendingOrder order)
    {
        if (_processingItemData == null) return false;
        PlayerCurrency.Instance.AddCurrency(_processingItemData.Amount, CurrencyType.Coins);
        return true;
    }

    public override void OnScreenClosedInternal()
    {
    }

    public override void OnScreenOpenedInternal()
    {
    }
}