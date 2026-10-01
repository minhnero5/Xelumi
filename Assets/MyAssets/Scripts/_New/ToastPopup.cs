using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ToastPopup : MonoBehaviour
{
    public static ToastPopup Instance { get; private set; }

     [Header("UI")]
    public GameObject root;          // root là GameObject có Image
    public TextMeshProUGUI message;  // optional

    [Header("Timing")]
    public float fadeInTime = 0.15f;
    public float showSeconds = 0.2f;
    public float fadeOutTime = 0.2f;

    private Image rootImage;
    private Sequence seq;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (root == null) root = gameObject;

        rootImage = root.GetComponent<Image>();
        if (rootImage == null)
        {
            Debug.LogError("ToastPopup: root phải có Image component.");
            return;
        }

        // Ẩn ban đầu
        root.SetActive(false);
        SetAlpha(0f);
    }

    public void Show(string text)
    {
        if (rootImage == null) return;

        if (message != null) message.text = text;

        root.SetActive(true);
        SetAlpha(0f);

        seq?.Kill();

        seq = DOTween.Sequence()
            .SetUpdate(true) // unscaled time
            .Append(rootImage.DOFade(1f, fadeInTime))
            .Join(message != null ? message.DOFade(1f, fadeInTime) : null)
            .AppendInterval(showSeconds)
            .Append(rootImage.DOFade(0f, fadeOutTime))
            .Join(message != null ? message.DOFade(0f, fadeOutTime) : null)
            .OnComplete(() =>
            {
                root.SetActive(false);
            });
    }

    public void HideNow()
    {
        seq?.Kill();
        seq = null;

        SetAlpha(0f);
        if (root != null) root.SetActive(false);
    }

    private void SetAlpha(float a)
    {
        if (rootImage != null)
        {
            var c = rootImage.color;
            c.a = a;
            rootImage.color = c;
        }

        if (message != null)
        {
            var c2 = message.color;
            c2.a = a;
            message.color = c2;
        }
    }

    private void OnDestroy()
    {
        seq?.Kill();
    }
}