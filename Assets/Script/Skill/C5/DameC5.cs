using UnityEngine;

public class DameC5 : MonoBehaviour
{
    public int damageAmount = 0;
    public bool destroyOnHit = true;
    public GameObject soundEffectPrefab; // Prefab chứa AudioSource
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Tìm component EnemyHealth thay vì Enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount); // Gây sát thương

                // 🔊 Tạo âm thanh va chạm tại điểm hiện tại
                if (soundEffectPrefab != null)
                {
                    Instantiate(soundEffectPrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject); // Hủy vật thể nếu cần
            }
        }
    }
}
