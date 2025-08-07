using UnityEngine;

public class PlayerTridentt : MonoBehaviour
{
    [Header("Thiết lập đạn")]
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float timeDestroy = 1f;
    [SerializeField] private float damage = 10f;

    [Header("Âm thanh khi trúng")]
    [SerializeField] private GameObject hitSoundPrefab;
    private bool hasHit;
    void Start()
    {
        Destroy(gameObject, timeDestroy); // Tự hủy nếu không trúng
    }

    void Update()
    {
        if (!hasHit)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
            }

            if (hitSoundPrefab != null)
            {
                Instantiate(hitSoundPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); // Biến mất ngay lập tức
        }
    }
}