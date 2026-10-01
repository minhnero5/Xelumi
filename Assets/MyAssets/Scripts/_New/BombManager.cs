using UnityEngine;
using TMPro;
using System.Collections;

public class BombManager : MonoBehaviour
{
    public static BombManager Instance;

    [Header("UI")]
    public GameObject instructionText;
    public TextMeshProUGUI bombCountText;

    public bool IsUsingBomb { get; private set; }

    private GameManager gameManager;
    private Coroutine checkWinRoutine;


    private void Awake()
    {
        Debug.Log("alo1");
        // Singleton đơn giản
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gameManager = FindFirstObjectByType<GameManager>();

        IsUsingBomb = false;
        if (instructionText != null) instructionText.SetActive(false);

        UpdateBombUI();
    }

    // Gắn vào OnClick của nút Bom (toggle)
    public void ActivateBomb()
    {
        // Nhấn lần nữa để tắt
        if (IsUsingBomb)
        {
            DeactivateBomb();
            return;
        }

        // Nếu ItemStorage chưa có thì không làm gì
        if (ItemStorage.Instance == null) return;

        // Hết bom thì không bật
        if (ItemStorage.Instance.Get(ItemType.Bomb) <= 0) return;

        IsUsingBomb = true;
        if (instructionText != null) instructionText.SetActive(true);
    }

    // Gọi khi bạn đã nổ 1 tile thành công
    public void UseBomb()
    {
        if (ItemStorage.Instance == null) return;

        // Trừ 1 bomb trong kho, nếu không đủ thì thôi
        if (!ItemStorage.Instance.UseOne(ItemType.Bomb))
            return;

        UpdateBombUI();
    }

    public void DeactivateBomb()
    {
        IsUsingBomb = false;

        if (instructionText != null) instructionText.SetActive(false);

        // Tránh gọi chồng coroutine check win
        if (checkWinRoutine != null) StopCoroutine(checkWinRoutine);
        checkWinRoutine = StartCoroutine(CheckWin());
    }

    public void UpdateBombUI()
    {
        if (bombCountText == null) return;

        int count = ItemStorage.Instance.Get(ItemType.Bomb);

        bombCountText.text = count.ToString();
    }

    private IEnumerator CheckWin()
    {
        yield return new WaitForSeconds(1f);

        if (FindFirstObjectByType<MovableItem>() != null)
            yield break;

        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
            gameManager.Win();
    }
}