using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private int value;

    void Start()
    {
        // Lấy số coin ngẫu nhiên khi coin vừa spawn
        value = Random.Range(CoinManager.Instance.minCoins, CoinManager.Instance.maxCoins + 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoins(value);
            Destroy(gameObject); // Biến mất khi nhặt
        }
    }
}
