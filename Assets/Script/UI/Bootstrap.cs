using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "Menu"; // Scene Menu

    void Awake()
    {
        // Tạo GameManager nếu chưa có
        if (GameManager.Instance == null)
        {
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
        }

        // Tạo SaveLoadManager nếu chưa có
        if (SaveLoadManager.Instance == null)
        {
            GameObject sm = new GameObject("SaveLoadManager");
            sm.AddComponent<SaveLoadManager>();
        }
    }

    void Start()
    {
        // Load menu
        SceneManager.LoadScene(firstSceneName);
    }
}
