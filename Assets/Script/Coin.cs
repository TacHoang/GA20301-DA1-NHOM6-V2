// Coin.cs
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 100;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoins(value);
            Destroy(gameObject);
        }
    }
}
