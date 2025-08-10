using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    private GameManager gm;

    public Vector3 defaultPosition = Vector3.zero;
    public float autoSaveInterval = 300f;

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

    void Start()
    {
        StartCoroutine(LoadAfterFrame());
        StartCoroutine(AutoSaveRoutine());
    }

    IEnumerator LoadAfterFrame()
    {
        yield return null;
        gm = GameManager.Instance;

        if (gm == null)
        {
            Debug.LogError("GameManager.Instance NULL!");
            yield break;
        }

        // Chỉ load dữ liệu nếu KHÔNG phải New Game
        if (!gm.isNewGame)
            LoadAllData();
        else
            gm.isNewGame = false;
    }

    IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(autoSaveInterval);
            SaveAllData();
        }
    }

    public void SaveAllData()
    {
        if (gm == null) gm = GameManager.Instance;
        if (gm == null) return;

        GameObject player = gm.GetPlayer();
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            PlayerPrefs.SetFloat("PlayerX", pos.x);
            PlayerPrefs.SetFloat("PlayerY", pos.y);
            PlayerPrefs.SetFloat("PlayerZ", pos.z);
        }

        PlayerPrefs.SetInt("PlayerHP", gm.playerHealth);
        PlayerPrefs.SetInt("PlayerGold", gm.playerGold);

        PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);

        PlayerPrefs.Save();
        Debug.Log("Game Saved");
    }

    public void LoadAllData()
    {
        if (gm == null) gm = GameManager.Instance;
        if (gm == null) return;

        GameObject player = gm.GetPlayer();
        if (player != null)
        {
            float x = PlayerPrefs.GetFloat("PlayerX", defaultPosition.x);
            float y = PlayerPrefs.GetFloat("PlayerY", defaultPosition.y);
            float z = PlayerPrefs.GetFloat("PlayerZ", defaultPosition.z);
            player.transform.position = new Vector3(x, y, z);
        }

        gm.playerHealth = PlayerPrefs.GetInt("PlayerHP", gm.playerHealth);
        gm.playerGold = PlayerPrefs.GetInt("PlayerGold", gm.playerGold);

        Debug.Log("Game Loaded");
    }
}
