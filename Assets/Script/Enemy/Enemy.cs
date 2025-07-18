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

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 patrolDirection;
    private float patrolTimer;
    private float invincibleTimer;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        patrolTimer = patrolChangeTime;
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

        if (invincibleTimer > 0f)
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

        Vector2 nextPos = (Vector2)transform.position + patrolDirection * patrolSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
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
        Vector2 nextPos = (Vector2)transform.position + dir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
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

        if (collision.CompareTag("Trident"))
        {
            Destroy(collision.gameObject);
            TakeDamage(damageTaken);
            invincibleTimer = invincibleTime;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);

        if (healthBarUI != null)
            healthBarUI.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Spawn coins via CoinManager
        if (CoinManager.Instance != null)
            CoinManager.Instance.SpawnCoinsAt(transform.position);

        // Notify spawner nếu có
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


