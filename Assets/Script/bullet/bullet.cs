using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Gây sát thương nếu có hệ thống máu
             other.GetComponent<PlayerHealth>()?.TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
