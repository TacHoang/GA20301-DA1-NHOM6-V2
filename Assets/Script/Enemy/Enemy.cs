using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Chase Settings")]
    public float speed = 2f;
    public float detectionRange = 5f;

    [Header("Patrol Settings")]
    public float patrolSpeed = 1f;
    public float patrolChangeTime = 2f;

    [Header("Damage Settings")]
    public int damageTaken = 50;
    public float attackRange = 1f;
    public float attackCooldown = 1f;

    // 👇 Các biến sức khỏe đã xóa:
    // public int maxHealth;
    // public int currentHealth;
    // public float invincibleTime;
    // public EnemyHealthBar healthBarUI;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 patrolDirection;
    private float patrolTimer;
    private float lastAttackTime = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        patrolTimer = patrolChangeTime;
        PickNewPatrolDirection();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRange)
            ChasePlayer();
        else
            Patrol();

        TryAttackPlayer();
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

    void TryAttackPlayer()
    {
        if (player == null || Time.time - lastAttackTime < attackCooldown) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageTaken);
                lastAttackTime = Time.time;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger)
            PickNewPatrolDirection();
    }

    // 👇 Các hàm liên quan đến máu đã bị xóa:
    // public void TakeDamage(int amount) { ... }
    // void Die() { ... }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
