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

        // Gọi âm thanh chết từ vị trí hiện tại
        if (dieSound != null)
            AudioSource.PlayClipAtPoint(dieSound, transform.position, 2.5f); // trên mức 1
        QuestManager qm = FindObjectOfType<QuestManager>();
        if (qm != null)
        {
            qm.AddKill(); // Tăng 1 kill duy nhất
        }


        // Gắn cờ chết
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
            enemy.isDead = true;

        // CoinManager
        if (CoinManager.Instance != null)
            CoinManager.Instance.SpawnCoinsAt(transform.position);

        // Vô hiệu hóa va chạm & vật lý
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Hủy sau 1 giây (hoặc thời lượng anim chết)
        Destroy(gameObject, 0.5f);
    }
}

