using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Máu")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Thanh máu")]
    public EnemyHealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
        else
            Debug.LogWarning("⚠️ Thiếu gán 'healthBar' cho Enemy");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Gọi CoinManager để sinh đồng xu
        if (CoinManager.Instance != null)
            CoinManager.Instance.SpawnCoinsAt(transform.position);

        Destroy(gameObject);
    }
}
