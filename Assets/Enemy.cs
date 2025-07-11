using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Tìm player và các component cần thiết
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Tính hướng và di chuyển về phía player
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 targetPosition = (Vector2)transform.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // Quay mặt về phía player bằng localScale
            float xDiff = player.position.x - transform.position.x;

            if (xDiff < -0.05f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f); // Quay mặt trái
            }
            else if (xDiff > 0.05f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // Quay mặt phải
            }
        }
    }

    // Gọi khi enemy bị tiêu diệt (vd: trúng đạn)
    public void Die()
    {
        // Thông báo cho EnemySpawner (nếu có)
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.NotifyEnemyDeath(gameObject);
        }

        Destroy(gameObject);
    }

    // Trigger khi trúng đạn
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Die();
        }
    }
}
