using UnityEngine;

public class BuyItem : MonoBehaviour
{
    [Header("Prices")]
    public int bombPrice = 50;
    public int timePrice = 30;

    [Header("Amounts")]
    public int bombAmount = 1;
    public int timeAmount = 1;

    public CoinsManager coinsManager;
    public BombManager bomManager;


    public void BuyBomb()
    {
        // 1) Trừ coin luôn (SỬA dòng này cho đúng CoinsManager của bạn)
        // CoinsManager.Instance.coins -= bombPrice;
        // CoinsManager.Instance.UpdateUI();
        if(coinsManager.collectedCoins < bombPrice)
        {
            ToastPopup.Instance.Show("Not enough coins!");
            return;
        }
        else
        {
            coinsManager.LessCoins(bombPrice);
        // 2) Cộng item vào kho
            ItemStorage.Instance.Add(ItemType.Bomb, bombAmount);

             ToastPopup.Instance.Show("Buy 1 Bomb!");
             bomManager.UpdateBombUI();
        }
    }

    public void BuyTimeBooster()
    {
        // 1) Trừ coin luôn (SỬA dòng này cho đúng CoinsManager của bạn)
        // CoinsManager.Instance.coins -= timePrice;
        // CoinsManager.Instance.UpdateUI();
        if(coinsManager.collectedCoins < timePrice)
        {
            ToastPopup.Instance.Show("Not enough coins!");
            return;
        }
        else
        {
            coinsManager.LessCoins(timePrice);
        // 2) Cộng item vào kho
            ItemStorage.Instance.Add(ItemType.TimeBooster, timeAmount);
            ToastPopup.Instance.Show("Buy 1 TimeBooster!");
        // ToastPopupDOTween_RootOnly.Instance.Show("Đã mua Time!");
        }
    }
}