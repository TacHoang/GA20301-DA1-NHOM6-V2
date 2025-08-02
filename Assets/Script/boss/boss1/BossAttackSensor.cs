using UnityEngine;

public class BossAttackZone : MonoBehaviour
{
    private BossController boss;

    void Start()
    {
        boss = GetComponentInParent<BossController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetAttacking(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetAttacking(false);
        }
    }
}
