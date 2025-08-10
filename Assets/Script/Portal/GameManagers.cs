using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Dữ liệu game
    public int playerHealth = 100;
    public int playerGold = 0;

    // Cờ báo New Game hay Continue
    public bool isNewGame = false;

    public static string sceneToLoad;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject GetPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            Debug.LogWarning("Không tìm thấy Player trong scene!");
        return player;
    }

    public void ResetData()
    {
        playerHealth = 100;
        playerGold = 0;
    }
}
