using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 100;
    private bool collected = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !collected)
        {
            collected = true;
            CoinManager.Instance.AddCoins(value);
            Destroy(gameObject);
        }
    }
}
