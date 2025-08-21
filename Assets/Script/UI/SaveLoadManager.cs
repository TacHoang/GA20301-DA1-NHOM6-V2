using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    private string saveFilePath;
    private GameManager gm;

    [System.Serializable]
    public class SaveData
    {
        public int playerHP;
        public int playerGold;
        public string currentScene;
    }

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        gm = GameManager.Instance;
    }

    // 🔹 Lưu dữ liệu game
    public void SaveAllData()
    {
        if (gm == null) gm = GameManager.Instance;
        if (gm == null) return;

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Menu") return; // Không lưu Menu

        SaveData data = new SaveData
        {
            playerHP = gm.playerHealth,
            playerGold = gm.playerGold,
            currentScene = currentScene
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("💾 Game Saved! (HP + Gold + Scene)");
    }

    // 🔹 Load dữ liệu game
    public void LoadAllData()
    {
        if (gm == null) gm = GameManager.Instance;
        if (gm == null) return;

        if (!File.Exists(saveFilePath))
        {
            Debug.Log("⚠️ No save file found, starting fresh.");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        gm.playerHealth = data.playerHP;
        gm.playerGold = data.playerGold;

        Debug.Log("📂 Game Loaded! (HP + Gold + Scene)");
    }

    // 🔹 Lấy scene đã lưu
    public string GetSavedScene()
    {
        if (!File.Exists(saveFilePath))
            return null;

        string json = File.ReadAllText(saveFilePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return data.currentScene;
    }

    // 🔹 Xóa save (nếu cần)
    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("🗑 Save deleted.");
        }
    }
}
