using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public class IAPHandler : MonoBehaviour
{
    private StoreController _storeController;
    private bool _storeConnected = false;
    private Func<PendingOrder, bool> _currentOrderPendingCallback;
    public bool StoreConnected => _storeConnected;
    public UnityEvent OnStoreReadyForPurchases = new UnityEvent();

    public async Task Initialize()
    {
        _storeController = UnityIAPServices.StoreController();
        _storeController.OnPurchasePending += OnPurchasePending;
        _storeController.OnStoreDisconnected += OnStoreDisconnected;
        _storeController.OnProductsFetched += OnProductsFetched;
        _storeController.OnProductsFetchFailed += OnProductsFetchFailed;
        _storeController.OnStoreConnected += OnStoreConnected;
        _storeController.OnPurchaseFailed += OnPurchaseFailed;
        _storeController.OnPurchaseDeferred += OnPurchaseDeferred;
        _storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;

        await _storeController.Connect();

        ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
        CatalogProvider catalogProvider = CodelessCatalogProvider.PopulateCatalogProvider(catalog);

        catalogProvider.FetchProducts(products =>
        {
            _storeController.FetchProducts(products);
        });

        OnStoreReadyForPurchases.Invoke();
    }

    private void OnPurchaseConfirmed(Order order)
    {
        Debug.Log("Purchase confirmed with product: " + order.Info.PurchasedProductInfo[0].productId);
    }

    private void OnPurchaseDeferred(DeferredOrder order)
    {
        Debug.Log("Purchase deferred with order: " + order.Info.PurchasedProductInfo[0].productId);
    }

    private void OnPurchaseFailed(FailedOrder order)
    {
        Debug.LogWarning("Purchase failed/cancelled with order:" + order.Info.PurchasedProductInfo[0].productId);
    }

    private void OnStoreConnected()
    {
        Debug.Log("IAP store connected");
        _storeConnected = true;
    }

    private void OnProductsFetched(List<Product> products)
    {
        Debug.Log("Fetched " + products.Count + " IAP products");
    }

    private void OnProductsFetchFailed(ProductFetchFailed failure)
    {
        Debug.LogError("Failed to fetch IAP products: " + failure.ToString());
    }

    private void OnStoreDisconnected(StoreConnectionFailureDescription description)
    {
        Debug.LogError("IAP store disconnected: " + description.ToString());
    }

    private void OnPurchasePending(PendingOrder order)
    {
        if (_currentOrderPendingCallback != null && _currentOrderPendingCallback(order))
        {
            _storeController.ConfirmPurchase(order);
        }
        else
        {
            Debug.LogError("Purchase succeeded but giving reward failed, as such, purchase is still pending");
        }
    }

    public void Purchase(string productId, Func<PendingOrder, bool> onPendingCallback)
    {
        if (_storeController == null)
        {
            Debug.LogError("IAP is not initialized");
            return;
        }

        Product product = _storeController.GetProducts().First(product => product.definition.id == productId);

        if (product == null)
        {
            Debug.LogError("Product not found: " + productId);
            return;
        }

        _currentOrderPendingCallback = onPendingCallback;
        Cart cart = new Cart(new CartItem(product));
        _storeController.Purchase(cart);
    }
}