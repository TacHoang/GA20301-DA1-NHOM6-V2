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
    private float timeInRange = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player != null && !isExploding)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            // Tăng thời gian nếu đứng gần player
            if (distance <= explodeRange)
            {
                timeInRange += Time.fixedDeltaTime;

                if (timeInRange >= 1f)
                {
                    StartCoroutine(ExplodeAfterDelay());
                }
            }
            else
            {
                timeInRange = 0f; // reset nếu player đi xa
            }

            // Di chuyển về phía player
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 targetPosition = (Vector2)transform.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // Lật mặt theo hướng player
            float xDiff = player.position.x - transform.position.x;
            if (xDiff < -0.05f)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (xDiff > 0.05f)
                transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    IEnumerator ExplodeAfterDelay()
    {
        isExploding = true;

        yield return new WaitForSeconds(explodeDelay);

        // Gây sát thương nếu player trong vùng nổ
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explodeRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // Gây sát thương ở đây nếu có hệ thống máu
                // hit.GetComponent<PlayerHealth>()?.TakeDamage(1);
            }
        }

        // Tạo hiệu ứng nổ và hủy nó sau 1 giây
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // tự hủy hiệu ứng sau 1 giây
        }

        Die(); // Xóa enemy sau khi nổ
    }

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
            Die(); // Bị bắn thì chết ngay, không nổ
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRange);
    }
}

