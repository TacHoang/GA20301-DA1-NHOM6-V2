using UnityEngine;
using DG.Tweening;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 5;
    public AudioClip healSound;
    public float attractRange = 3f;       // Phạm vi hút máu
    public float moveSpeed = 5f;          // Tốc độ bay về Player

    private Transform player;
    private bool isAttracted = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Nếu trong phạm vi thì hút về
        if (distance <= attractRange)
        {
            isAttracted = true;
        }

        // Nếu đang bị hút thì di chuyển dần về phía Player
        if (isAttracted)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);

                if (healSound != null)
                    AudioSource.PlayClipAtPoint(healSound, transform.position);

                // Tween hiệu ứng biến mất
                transform.DOScale(1.5f, 0.15f).SetEase(Ease.OutBack);
                Destroy(gameObject, 0.2f);
            }
        }
    }
}
