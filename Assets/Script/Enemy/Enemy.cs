using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Chase Settings")]
    public float speed = 2f;
    public float detectionRange = 5f;

    [Header("Patrol Settings")]
    public float patrolSpeed = 1f;
    public float patrolChangeTime = 2f;

    [Header("Health Settings")]
    public int maxHealth = 100;
    [HideInInspector] public int currentHealth;
    public float invincibleTime = 0.5f;

    [Header("Damage Settings")]
    public int damageTaken = 50;

    [Header("Health Bar UI")]
    public EnemyHealthBar healthBarUI;

    [Header("Coin Drop Settings")]
    public GameObject coinPrefab;
    [Min(0)] public int minCoin = 50;
    [Min(1)] public int maxCoin = 150;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 patrolDirection;
    private float patrolTimer;
    private float invincibleTimer = 0f;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        patrolTimer = 0f;
        PickNewPatrolDirection();

        currentHealth = maxHealth;

        if (healthBarUI != null)
        {
            healthBarUI.followTarget = transform;
            healthBarUI.SetHealth(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogWarning($"⚠️ Enemy {name} chưa gán HealthBarUI trong Inspector.");
        }
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= detectionRange)
            ChasePlayer();
        else
            Patrol();

        if (invincibleTimer > 0)
            invincibleTimer -= Time.fixedDeltaTime;
    }

    void ChasePlayer()
    {
        MoveTowards(player.position, speed);
    }

    void Patrol()
    {
        patrolTimer -= Time.fixedDeltaTime;
        if (patrolTimer <= 0f)
            PickNewPatrolDirection();

        Vector2 targetPos = (Vector2)transform.position + patrolDirection * patrolSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
        FlipSprite(patrolDirection.x);
    }

    void PickNewPatrolDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        patrolDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        patrolTimer = patrolChangeTime;
    }

    void MoveTowards(Vector2 target, float moveSpeed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        Vector2 pos = (Vector2)transform.position + dir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
        FlipSprite(dir.x);
    }

    void FlipSprite(float xDir)
    {
        if (xDir < -0.05f) transform.localScale = new Vector3(-1, 1, 1);
        else if (xDir > 0.05f) transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger)
            PickNewPatrolDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead || invincibleTimer > 0f) return;

        Debug.Log($"Enemy va chạm với: {collision.name}, tag: {collision.tag}");

        if (collision.CompareTag("Trident"))
        {
            Debug.Log("Bị Trident tấn công!");
            Destroy(collision.gameObject);
            TakeDamage(damageTaken);
            invincibleTimer = invincibleTime;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);

        Debug.Log($"Enemy bị mất {amount} máu. Còn lại: {currentHealth}");

        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(currentHealth, maxHealth);
            Debug.Log("Cập nhật thanh máu thành công!");
        }
        else
        {
            Debug.LogWarning("⚠️ Chưa gán HealthBar UI trong Inspector!");
        }

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Enemy đã chết!");

        if (coinPrefab != null)
        {
            int amount = Random.Range(minCoin, maxCoin + 1);
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Coin coinScript = coin.GetComponent<Coin>();
            if (coinScript != null)
                coinScript.value = amount;
        }

        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
            spawner.NotifyEnemyDeath(gameObject);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

