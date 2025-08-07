using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField] private int amountToSpawn = 1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SpawnHealthAt(Vector3 position)
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 spawnPos = position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f), 0f);
            Instantiate(healthPickupPrefab, spawnPos, Quaternion.identity);
        }
    }
}
