using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("Coin Drop Settings")]
    [Range(0f, 100f)]
    public float dropChancePercent = 100f;  // 100% nếu bạn muốn drop luôn khi enemy chết
    public int minDrop = 50;
    public int maxDrop = 150;
    public GameObject coinPrefab;

    [Header("UI")]
    public Text coinText;
    [HideInInspector] public int totalCoins = 0;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        UpdateCoinUI();
    }

    public void SpawnCoinsAt(Vector2 position)
    {
        float roll = Random.Range(0f, 100f);
        if (roll > dropChancePercent || coinPrefab == null)
            return;

        int amount = Random.Range(minDrop, maxDrop + 1);
        // Tốt nhất spawn nhiều xu nhỏ tổng gần bằng amount,
        // hoặc spawn 1 coin mang toàn bộ ─ tùy ý.
        GameObject c = Instantiate(coinPrefab, position, Quaternion.identity);
        var coinScript = c.GetComponent<Coin>();
        if (coinScript != null)
            coinScript.value = amount;
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = totalCoins.ToString();
    }
}


