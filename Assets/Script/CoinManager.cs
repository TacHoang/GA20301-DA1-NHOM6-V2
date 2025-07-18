// CoinManager.cs
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public int totalCoins = 0;
    public Text coinText;  // kéo UI Text vào đây

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        if (coinText != null)
            coinText.text = totalCoins.ToString();
    }
}

