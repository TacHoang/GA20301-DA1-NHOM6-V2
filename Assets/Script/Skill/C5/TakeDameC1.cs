using UnityEngine;

public class TakeDameC1 : MonoBehaviour
{
    public int damage = 50;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"🔥 Xung siêu âm gây {damage} dame cho {other.name}");
            }
        }
    }


}
