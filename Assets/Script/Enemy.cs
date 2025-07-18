using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Chase Settings")]
    public float speed = 2f;
    public float detectionRange = 5f;

    [Header("Patrol Settings")]
    public float patrolSpeed = 1f;
    public float patrolChangeTime = 2f;

    [Header("Coin Drop Settings")]
    public GameObject coinPrefab;   // Kéo Prefab Coin vào đây
    [Min(0)] public int minCoin = 50;
    [Min(1)] public int maxCoin = 150;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 patrolDirection;
    private float patrolTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        patrolTimer = 0f;
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

    public void Die()
    {
        // Spawn coin ngẫu nhiên
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trident"))
        {
            Destroy(collision.gameObject); // Hủy Trident
            Destroy(gameObject); // Hủy Quai
            Die();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
