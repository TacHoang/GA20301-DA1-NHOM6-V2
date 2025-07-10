using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;     // Prefab của enemy
    public Transform player;           // Transform của Player
    public int maxEnemies = 100;       // Tổng số enemy spawn tối đa
    public int maxActiveEnemies = 10;  // Số enemy đang tồn tại cùng lúc
    public float spawnRadius = 20f;    // Khoảng cách spawn quanh player

    private int totalSpawned = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Update()
    {
        // Kiểm tra player có di chuyển không
        if (player != null && player.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 0.1f)
        {
            while (activeEnemies.Count < maxActiveEnemies && totalSpawned < maxEnemies)
            {
                SpawnEnemy();
            }
        }

        // Loại bỏ các enemy đã chết (null)
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    void SpawnEnemy()
    {
        float minDistanceFromPlayer = 2f;

        Vector2 randomOffset = new Vector2(
            Random.Range(-spawnRadius, spawnRadius),
            Random.Range(-spawnRadius, spawnRadius)
        );

        // Tránh spawn quá gần player
        if (randomOffset.magnitude < minDistanceFromPlayer)
        {
            randomOffset = randomOffset.normalized * Random.Range(minDistanceFromPlayer, spawnRadius);
        }

        Vector2 spawnPos = (Vector2)player.position + randomOffset;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        activeEnemies.Add(newEnemy);
        totalSpawned++;
    }

    // Gọi khi enemy chết để cập nhật danh sách
    public void NotifyEnemyDeath(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
}

