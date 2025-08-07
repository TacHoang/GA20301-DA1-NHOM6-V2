using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("UI Settings")]
    public string coinTextObjectName = "CoinText";
    private Text coinText;

    [Header("Coin Prefab")]
    public GameObject coinPrefab;

    [Header("Coin Value Range")]
    public int minCoins = 20;
    public int maxCoins = 99;

    [Header("Drop Settings")]
    [Range(0f, 100f)]
    public float coinDropChance = 70f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindCoinTextAndUpdateUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCoinTextAndUpdateUI();
    }

    private void FindCoinTextAndUpdateUI()
    {
        GameObject found = GameObject.Find(coinTextObjectName);
        if (found != null)
        {
            coinText = found.GetComponent<Text>();
        }

        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        GameManager.Instance.playerGold += amount;
        UpdateCoinUI();
    }

    public void ResetCoin()
    {
        GameManager.Instance.playerGold = 0;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = GameManager.Instance.playerGold.ToString("D2");
        }
    }

    public void SpawnCoinsAt(Vector3 position)
    {
        float roll = Random.Range(0f, 100f);
        if (roll <= coinDropChance && coinPrefab != null)
        {
            Instantiate(coinPrefab, position, Quaternion.identity);
        }
    }
}
