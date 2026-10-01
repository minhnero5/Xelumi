using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPButtonView : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI priceText;
    int coin = 500;
    [SerializeField] int inappIndex = 0;
    Button buyBtn;
    private string productId = "";

    public CoinsManager coinsManager;

    private void Awake()
    {
        buyBtn = GetComponentInChildren<Button>();
        buyBtn.onClick.AddListener(OnBuyButtonClicked);
        productId = IAPManager.Instance.GetProductId(inappIndex);
// #if UNITY_EDITOR
//         OnProductFetched(IAPManager.Instance.GetProductById(productId));
// #endif
    }
    void OnEnable()
    {
        StartCoroutine(LoadProduct());
    }
    private void OnBuyButtonClicked()
    {
        Debug.Log($"[IAP] Attempting to purchase product: {productId}");
        IAPManager.Instance.BuyProduct(this.productId, (product) =>
        {
            OnPurchaseSuccess();
        });
    }

    public void OnProductFetched(Product product)
    {
        if (product == null)
        {
            Debug.LogError($"[IAP] Product with ID {productId} not found.");
            gameObject.SetActive(false);
            return;
        }
        if (product.availableToPurchase)
        {
            string tile = product.metadata.localizedTitle;
            coin = int.Parse(tile.Split(' ')[0]);
            coinText.text = coin.ToString() + " Coins";
            priceText.text = product.metadata.localizedPriceString;
        }
    }
    IEnumerator LoadProduct()
   {
        while (!IAPManager.Instance.isProductsFetched)
        {
            yield return null;
        }
        OnProductFetched(IAPManager.Instance.GetProductById(productId));
    }
    
    public void OnPurchaseSuccess()
    {
        coinsManager.AddCoins(coin);
        ToastPopup.Instance.Show("Purchased Success");
       ///NotificationPopup.instance.AddNotification("Purchased Successfully");
    }
}
