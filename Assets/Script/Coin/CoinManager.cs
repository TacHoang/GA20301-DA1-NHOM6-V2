using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("Coin Settings")]
    public int coinCount = 0;
    public string coinTextObjectName = "CoinText"; // Tên GameObject chứa Text
    private Text coinText;

    [Header("Coin Prefab")]
    public GameObject coinPrefab;

    [Header("Coin Value Range")]
    public int minCoins = 20;
    public int maxCoins = 99;

    [Header("Drop Settings")]
    [Range(0f, 100f)]
    public float coinDropChance = 70f; // % cơ hội rơi coin (vd: 70%)

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
        coinCount += amount;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = coinCount.ToString("D2"); // Hiển thị: 00, 01, 99...
        }
    }

    public void SpawnCoinsAt(Vector3 position)
    {
        // Kiểm tra % có rơi coin không
        float roll = Random.Range(0f, 100f);
        if (roll <= coinDropChance)
        {
            if (coinPrefab != null)
            {
                Instantiate(coinPrefab, position, Quaternion.identity);
                // Không cộng xu ở đây, chỉ cộng khi player nhặt
            }
        }
    }
}
