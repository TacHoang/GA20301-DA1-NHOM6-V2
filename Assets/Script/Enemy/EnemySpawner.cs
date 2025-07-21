using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpawnPointData
{
    public Transform point;
    public int maxEnemies = 10;
    public List<GameObject> enemies = new List<GameObject>();
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Points & Settings")]
    public List<SpawnPointData> spawnPoints = new List<SpawnPointData>();

    [Header("Spawn Randomness")]
    public float offsetRange = 1f;
    public float checkRadius = 0.5f;

    void Start()
    {
        // Nếu danh sách spawn trống thì tự động lấy các con của object này
        if (spawnPoints.Count == 0)
        {
            foreach (Transform child in transform)
            {
                SpawnPointData data = new SpawnPointData();
                data.point = child;
                data.maxEnemies = 10;
                spawnPoints.Add(data);
            }
        }

        // Bắt đầu quy trình spawn nhẹ nhàng
        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            foreach (var spawnData in spawnPoints)
            {
                // Xoá quái đã chết khỏi danh sách
                spawnData.enemies.RemoveAll(enemy => enemy == null);

                // Nếu còn thiếu thì spawn thêm 1 con
                if (spawnData.enemies.Count < spawnData.maxEnemies)
                {
                    TrySpawnEnemyAt(spawnData);
                }

                // Chờ chút trước khi chuyển sang điểm tiếp theo (giảm lag)
                yield return new WaitForSeconds(0.1f);
            }

            // Lặp lại mỗi giây
            yield return new WaitForSeconds(1f);
        }
    }

    void TrySpawnEnemyAt(SpawnPointData spawnData)
    {
        for (int i = 0; i < 5; i++) // thử 5 vị trí ngẫu nhiên
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            Vector2 offset = new Vector2(
                Random.Range(-offsetRange, offsetRange),
                Random.Range(-offsetRange, offsetRange)
            );

            Vector2 spawnPos = (Vector2)spawnData.point.position + offset;

            Collider2D hit = Physics2D.OverlapCircle(spawnPos, checkRadius);
            if (hit == null)
            {
                GameObject newEnemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                spawnData.enemies.Add(newEnemy);
                break;
            }
        }
    }

    // Gọi từ enemy khi bị tiêu diệt
    public void NotifyEnemyDeath(GameObject enemy)
    {
        foreach (var data in spawnPoints)
        {
            if (data.enemies.Contains(enemy))
            {
                data.enemies.Remove(enemy);
                break;
            }
        }
    }
}
