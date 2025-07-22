using UnityEngine;

public class DameC5 : MonoBehaviour
{
    public int damageAmount = 0;
    public bool destroyOnHit = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Tìm component EnemyHealth thay vì Enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount); // Gây sát thương

                if (destroyOnHit)
                    Destroy(gameObject); // Hủy vật thể nếu cần
            }
        }
    }
}
