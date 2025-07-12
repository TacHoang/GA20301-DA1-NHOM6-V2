using UnityEngine;
using System.Collections;

public class ExplodingEnemy : MonoBehaviour
{
    public float speed = 2f;
    public float explodeRange = 1.5f;
    public float explodeDelay = 0.5f;
    public GameObject explosionEffect;

    private Transform player;
    private Rigidbody2D rb;
    private bool isExploding = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player != null && !isExploding)
        {
            // Di chuyển về phía player
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 targetPosition = (Vector2)transform.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // Quay mặt theo hướng player
            float xDiff = player.position.x - transform.position.x;
            if (xDiff < -0.05f)
                transform.localScale = new Vector3(-1f, 1f, 1f); // trái
            else if (xDiff > 0.05f)
                transform.localScale = new Vector3(1f, 1f, 1f);  // phải

            // Nếu đủ gần thì bắt đầu phát nổ
            if (Vector2.Distance(transform.position, player.position) <= explodeRange)
            {
                StartCoroutine(ExplodeAfterDelay());
            }
        }
    }

    IEnumerator ExplodeAfterDelay()
    {
        isExploding = true;

        yield return new WaitForSeconds(explodeDelay);

        // Gây sát thương nếu trong vùng nổ
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explodeRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // Gây sát thương ở đây nếu có hệ thống máu
                // hit.GetComponent<PlayerHealth>()?.TakeDamage(1);
            }
        }

        // Hiệu ứng nổ
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Die(); // Tự hủy sau khi nổ
    }

    // Enemy chết do bị bắn
    public void Die()
    {
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.NotifyEnemyDeath(gameObject);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Die(); // Bị bắn thì chết ngay, không cần phát nổ
        }
    }

    // Vẽ vùng nổ khi chọn enemy trong Scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRange);
    }
}
