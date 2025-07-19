using UnityEngine;

public class DameC5 : MonoBehaviour
{
    public int damageAmount = 0;
    public bool destroyOnHit = true;
    void Start()
    {


    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount); // Gây sát thương

                if (destroyOnHit)
                    Destroy(gameObject); // Hủy vật thể nếu cần
            }
        }
    }


}
