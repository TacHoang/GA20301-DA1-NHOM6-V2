using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Spawn Settings")]
    public int maxEnemiesPerPoint = 10;    // Số lượng enemy mỗi điểm spawn
    public int maxTotalEnemies = 100;      // Giới hạn tổng nếu muốn

    [Header("Spawn Randomness")]
    public float offsetRange = 1f;
    public float checkRadius = 0.5f;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        // Tự động thêm các con làm điểm spawn nếu chưa gán
        if (spawnPoints.Count == 0)
        {
            foreach (Transform child in transform)
            {
                spawnPoints.Add(child);
            }
        }
    }

    void Update()
    {
        // Xoá các enemy đã chết
        activeEnemies.RemoveAll(enemy => enemy == null);

        foreach (Transform spawnPoint in spawnPoints)
        {
            int count = CountEnemiesNear(spawnPoint.position);

            while (count < maxEnemiesPerPoint && activeEnemies.Count < maxTotalEnemies)
            {
                TrySpawnEnemyAt(spawnPoint);
                count++;
            }
        }
    }

    void TrySpawnEnemyAt(Transform spawnPoint)
    {
        for (int i = 0; i < 5; i++) // thử 5 lần
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Vector2 offset = new Vector2(
                Random.Range(-offsetRange, offsetRange),
                Random.Range(-offsetRange, offsetRange)
            );
            Vector2 spawnPos = (Vector2)spawnPoint.position + offset;

            Collider2D hit = Physics2D.OverlapCircle(spawnPos, checkRadius);
            if (hit == null)
            {
                GameObject newEnemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                activeEnemies.Add(newEnemy);
                break;
            }
        }
    }

    int CountEnemiesNear(Vector2 position)
    {
        int count = 0;
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null && Vector2.Distance(enemy.transform.position, position) < offsetRange * 2)
            {
                count++;
            }
        }
        return count;
    }

    // Gọi từ enemy khi nó chết
    public void NotifyEnemyDeath(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
}

