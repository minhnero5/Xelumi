using System;
using UnityEngine;

public enum ItemType
{
    TimeBooster,
    Bomb
}

public class ItemStorage : MonoBehaviour
{
    public static ItemStorage Instance { get; private set; }

    // Event để UI/Shop cập nhật số lượng
    public event Action<ItemType, int> OnItemChanged;

    private const string INIT_KEY = "ITEM_STORAGE_INIT";

    [Header("First Time Gift (only once)")]
    public int firstTimeTimeBooster = 3;
    public int firstTimeBomb = 3;

    private void Awake()
    {
            Debug.Log("alo2");
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        InitFirstTime();
    }

    private string Key(ItemType type) => "ITEM_" + type.ToString();

    // ✅ Chạy 1 lần duy nhất cho người chơi mới
    private void InitFirstTime()
    {
        if (PlayerPrefs.GetInt(INIT_KEY, 0) == 1) return;

        Set(ItemType.TimeBooster, firstTimeTimeBooster);
        Set(ItemType.Bomb, firstTimeBomb);

        PlayerPrefs.SetInt(INIT_KEY, 1);
        PlayerPrefs.Save();
    }

    public int Get(ItemType type)
    {
        return PlayerPrefs.GetInt(Key(type), 0);
    }

    public void Set(ItemType type, int value)
    {
        int safe = Mathf.Max(0, value);
        PlayerPrefs.SetInt(Key(type), safe);
        PlayerPrefs.Save();
        OnItemChanged?.Invoke(type, safe);
    }

    public void Add(ItemType type, int amount)
    {
        if (amount <= 0) return;
        Set(type, Get(type) + amount);
    }

    // Trừ 1 item, trả true nếu trừ được
    public bool UseOne(ItemType type)
    {
        int current = Get(type);
        if (current <= 0) return false;

        Set(type, current - 1);
        return true;
    }
}