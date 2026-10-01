using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObject retryPanel, winPanel, startPanel, commingSoonPanel;
    public Text levelText, moveLimitText;
    private bool isWin, isRetry;
    public int levelNo;
    public int skipLevelCoinAmount;
    public Animator toastAnim;
    public Gradient red, pink, blue, green, yellow;
    public int moveLimitValue;
    [HideInInspector] public bool isDragging;
    public GameObject moveLimitTextObj;

    void Awake()
    {
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
       // PlayerPrefs.SetInt("Level", 225);
        levelNo = PlayerPrefs.GetInt("Level", 1);
        levelText.text = "LEVEL " + levelNo;

        if (levelNo < 11)
            moveLimitTextObj.SetActive(false);
    }

    private void Start()
    {
        FindFirstObjectByType<LevelLoader>().LoadLevel(levelNo);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Next();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerPrefs.SetInt("Level", levelNo - 1);
            SceneManager.LoadScene(0);
        }
    }

    public void Retry()
    {
        if (isWin)
            return;

        isRetry = true;
        Invoke(nameof(InvokeRetryGame), 1.5f);
    }

    private void InvokeRetryGame()
    {
        AudioManager.Instance.Play("Retry");
        retryPanel.SetActive(true);
        retryPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);
    }

    public void Reload()
    {
        PlayClickSound();
        //AdsManager.Instance.ShowInterstitialAd();
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        PlayClickSound();
        startPanel.SetActive(false);
    }

    public void Win()
    {
        if (isRetry)
            return;

        isWin = true;
        Invoke(nameof(InvokeWinGame), 1);
    }

    private void InvokeWinGame()
    {
        AudioManager.Instance.Play("Win");

        if (SceneManager.GetActiveScene().buildIndex < 225)
        {
            winPanel.SetActive(true);
            winPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);
        }
        else
        {
            commingSoonPanel.SetActive(true);
            commingSoonPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);
        }
    }

    public void Next()
    {
        PlayClickSound();
        //AdsManager.Instance.ShowInterstitialAd();

        PlayerPrefs.SetInt("Level", levelNo + 1);
        SceneManager.LoadScene(0);
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void ShowRewardVideo()
    {
        //AdsManager.Instance.ShowRewardedAd();
    }

    public void SkipLevel()
    {
        PlayClickSound();
        //AdsManager.Instance.ShowInterstitialAd();

        CoinsManager coinsManager = FindFirstObjectByType<CoinsManager>();

        if (coinsManager.collectedCoins >= skipLevelCoinAmount)
        {
            coinsManager.LessCoins(skipLevelCoinAmount);
            Next();
            AudioManager.Instance.Play("Reward");
        }
        else
        {
            toastAnim.SetTrigger("Toast");
        }
    }

    public void DecreseMoveLimitText()
    {
        if(moveLimitValue == 0)
            return;

        moveLimitValue--;
        moveLimitText.text = "Moves " + moveLimitValue;

        if (moveLimitValue == 0)
        {
            if(FindObjectsByType<MovableItem>(FindObjectsSortMode.None).Length == 1)
                Invoke(nameof(Retry), 2);
            else
            {
                Retry();
            }
        }
    }

    public void SetMoveLimit(int moveLimit)
    {
        moveLimitValue = moveLimit;
        moveLimitText.text = "Moves " + moveLimit;
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.Play("Click");
    }
}
