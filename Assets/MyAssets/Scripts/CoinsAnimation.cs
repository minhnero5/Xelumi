using DG.Tweening;
using UnityEngine;

public class CoinsAnimation : MonoBehaviour
{
    [SerializeField] private GameObject pileOfCoins;
    private Vector2[] initialPos;
    private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount;
    private Vector2 targetPos;
    private CoinsManager coinsManager;

    void Start()
    {             
        initialPos = new Vector2[coinsAmount];
        initialRotation = new Quaternion[coinsAmount];
        
        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            RectTransform rectTransform = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>();

            initialPos[i] = rectTransform.anchoredPosition;
            initialRotation[i] = rectTransform.rotation;
        }

        coinsManager = FindObjectOfType<CoinsManager>();
        targetPos = coinsManager.transform.position;

        CountCoins();
    }

    public void CountCoins()
    {
        pileOfCoins.SetActive(true);
        var delay = 0f;

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            Transform coin = pileOfCoins.transform.GetChild(i);

            coin.DOScale(.8f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            coin.DOMove(targetPos, 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack);
            coin.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
            coin.DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }

        Invoke(nameof(PlayCoinSound), 1);
        InvokeRepeating(nameof(GiveCoins), .1f, .1f);
    }


    private void PlayCoinSound()
    {
        AudioManager.Instance.Play("Coins");
    }

    int count = 0;
    void GiveCoins()
    {
        count++;

        if (count == 25)
            CancelInvoke(nameof(GiveCoins));

        coinsManager.AddCoins(2);
    }
}
