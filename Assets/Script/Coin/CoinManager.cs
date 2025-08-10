using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
        StartCoroutine(FindUIAfterDelay());
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
        StartCoroutine(FindUIAfterDelay());
    }

    private IEnumerator FindUIAfterDelay()
    {
        yield return null; // đợi 1 frame
        GameObject found = GameObject.Find(coinTextObjectName);
        if (found != null)
        {
            coinText = found.GetComponent<Text>();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy UI CoinText trong scene: " + SceneManager.GetActiveScene().name);
        }
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerGold += amount;
            UpdateCoinUI();
        }
    }

    public void ResetCoin()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerGold = 0;
            UpdateCoinUI();
        }
    }

    public void UpdateCoinUI()
    {
        if (coinText != null && GameManager.Instance != null)
        {
            coinText.text = GameManager.Instance.playerGold.ToString("D2");
        }
    }

    public void SpawnCoinsAt(Vector3 position)
    {
        if (Random.Range(0f, 100f) <= coinDropChance && coinPrefab != null)
        {
            Instantiate(coinPrefab, position, Quaternion.identity);
        }
    }
}
