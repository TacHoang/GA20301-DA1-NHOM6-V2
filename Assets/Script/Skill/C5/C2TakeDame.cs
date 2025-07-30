using UnityEngine;
using System.Collections.Generic;

public class C2TakeDame : MonoBehaviour
{
    public float damage = 10f;
    public float duration = 4f;
    public float damageInterval = 0.5f;
    private AudioSource audio;
    private Transform player;

    private Dictionary<Collider2D, float> nextDamageTimes = new Dictionary<Collider2D, float>();
    private List<Collider2D> enemiesInside = new List<Collider2D>();

    void Start()
    {
        player = transform.parent;
        Destroy(gameObject, duration);
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // theo dõi vị trí player
        if (player != null)
            transform.position = player.position;

        // xử lý damage với vòng for ngược (từ cuối về đầu)
        for (int i = enemiesInside.Count - 1; i >= 0; i--)
        {
            var col = enemiesInside[i];
            if (col == null)
            {
                // nếu collider null (đã bị destroy), xóa khỏi list
                enemiesInside.RemoveAt(i);
                nextDamageTimes.Remove(col);
                continue;
            }

            float nextTime = nextDamageTimes.TryGetValue(col, out float nt) ? nt : 0f;
            if (Time.time >= nextTime)
            {
                EnemyHealth enemy = col.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)damage);
                    nextDamageTimes[col] = Time.time + damageInterval;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !enemiesInside.Contains(other))
        {
            enemiesInside.Add(other);
            nextDamageTimes[other] = 0f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (enemiesInside.Contains(other))
            enemiesInside.Remove(other);
        if (nextDamageTimes.ContainsKey(other))
            nextDamageTimes.Remove(other);
    }

    void OnDestroy()
    {
        if (audio != null && audio.isPlaying)
            audio.Stop();
    }
}
