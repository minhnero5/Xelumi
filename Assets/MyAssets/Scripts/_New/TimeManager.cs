using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Timer")]
    public float startSeconds = 180f;     // 3 phút
    public Text timerText;                // optional

    [Header("Time Booster Item")]
    public float addSecondsPerUse = 10f;  // mỗi lần dùng +10s
    public TextMeshProUGUI timeBoosterCountText;     // optional: hiển thị số lượng item

    private float remaining;
    private bool started;
    private bool running;

    private GameManager gm;

    private void Awake()
    {
        gm = FindFirstObjectByType<GameManager>();
        remaining = startSeconds;
        UpdateUI();
    }

    private void Update()
    {
        if (gm == null) return;

        // Đợi game bắt đầu (StartPanel tắt)
        if (!started)
        {
            if (gm.startPanel == null || gm.startPanel.activeSelf == false)
            {
                started = true;
                running = true;

                remaining = startSeconds;
                UpdateUI();
            }
            else
            {
                return;
            }
        }

        // Nếu win/lose thì dừng timer
        if (IsGameEnded())
        {
            running = false;
            return;
        }

        if (!running) return;

        remaining -= Time.deltaTime;
        if (remaining <= 0f)
        {
            remaining = 0f;
            UpdateUI();

            running = false;
            gm.Retry(); // hết giờ -> retry
            return;
        }

        UpdateUI();
    }

    // ✅ Nút "Use +10s" gọi hàm này
    public void UseTimeBooster()
    {
        if (!running) return;

        // Trừ 1 item trong kho
        if (!ItemStorage.Instance.UseOne(ItemType.TimeBooster))
        {
            // hết item -> có thể hiện popup/âm thanh
            UpdateUI();
            return;
        }

        remaining += addSecondsPerUse;
        UpdateUI();
    }

    private bool IsGameEnded()
    {
        if (gm.winPanel != null && gm.winPanel.activeSelf) return true;
        if (gm.commingSoonPanel != null && gm.commingSoonPanel.activeSelf) return true;
        if (gm.retryPanel != null && gm.retryPanel.activeSelf) return true;
        return false;
    }

    private void UpdateUI()
    {
        // timer
        if (timerText != null)
        {
            int total = Mathf.CeilToInt(Mathf.Max(0f, remaining));
            int min = total / 60;
            int sec = total % 60;
            timerText.text = $"{min:00}:{sec:00}";
        }

        // item count từ storage
        if (timeBoosterCountText != null)
        {
            timeBoosterCountText.text = ItemStorage.Instance.Get(ItemType.TimeBooster).ToString();
        }
    }
}