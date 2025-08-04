using UnityEngine;

public class Boss : MonoBehaviour
{
    private Boss3 boss;

    void Start()
    {
        boss = GetComponentInParent<Boss3>();

        if (boss == null)
            Debug.LogError("[BossAttackZone] Không tìm thấy Boss3!");
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
