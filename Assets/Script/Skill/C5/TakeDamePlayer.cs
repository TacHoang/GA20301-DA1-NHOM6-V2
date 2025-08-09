using UnityEngine;

public class TakeDamePlayer : MonoBehaviour
{
    [SerializeField] private float damageAmount = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage((int)damageAmount);
                Debug.Log("Người chơi đã bị bẫy gây sát thương: " + damageAmount);
            }
        }
    }


}
