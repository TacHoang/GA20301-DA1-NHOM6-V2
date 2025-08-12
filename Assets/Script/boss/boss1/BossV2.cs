using UnityEngine;

public class BossV2 : MonoBehaviour
{
    [Header("Máu")]
    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;

    [Header("Âm thanh chết")]
    public AudioClip dieSound;
    [Range(0f, 1f)] public float dieVolume = 1f;
    private AudioSource audioSource;

    [Header("UI Boss")]
    public GameObject BosshealthBarCanvas;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Xóa thanh máu boss hoàn toàn
        if (BosshealthBarCanvas != null)
        {
            Destroy(BosshealthBarCanvas);
            Debug.Log("🧨 Đã xóa bossHealthBarCanvas: " + BosshealthBarCanvas.name);
        }
        else
        {
            Debug.LogWarning("⚠️ bossHealthBarCanvas chưa được gán!");
        }

        // Gọi anim Die
        if (animator != null)
            animator.SetTrigger("Die");

        // Gọi âm thanh chết
        if (dieSound != null && audioSource != null)
        {
            audioSource.clip = dieSound;
            audioSource.volume = dieVolume;
            audioSource.Play();
        }

        // Cập nhật nhiệm vụ
        QuestManager qm = FindObjectOfType<QuestManager>();
        if (qm != null)
            qm.AddKill();

        // Gắn cờ chết
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
            enemy.isDead = true;

        // Spawn máu
        HealthManager.Instance?.SpawnHealthAt(transform.position);

        // Spawn vàng
        CoinManager.Instance?.SpawnCoinsAt(transform.position);

        // Vô hiệu hóa va chạm & vật lý
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Hủy boss sau 1 giây
        Destroy(gameObject, 1f);
    }


}
