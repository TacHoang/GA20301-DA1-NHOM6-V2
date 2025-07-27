using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Máu")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Thanh máu")]
    public EnemyHealthBar healthBar;

    private Animator animator;

    [Header("Âm thanh chết")]
    public AudioClip dieSound;
    [Range(0f, 1f)] public float dieVolume = 1f;
    private AudioSource audioSource;


    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
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
        // Gọi anim Die
        if (animator != null)
            animator.SetTrigger("Die");
        
        // 2. Gọi âm thanh chết
        if (dieSound != null && audioSource != null)
            {
                audioSource.clip = dieSound;
                audioSource.volume = dieVolume;
                audioSource.Play();
            }


        // Gắn cờ chết cho script Enemy
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
            enemy.isDead = true;

        // Gọi CoinManager
        if (CoinManager.Instance != null)
            CoinManager.Instance.SpawnCoinsAt(transform.position);

        // Vô hiệu hóa collider + vật lý
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Hủy sau thời gian anim
        Destroy(gameObject, 0.8f); // 1 giây = thời lượng anim chết
    }
}

