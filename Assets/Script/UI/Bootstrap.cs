using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "Menu";

    void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
        }

        if (SaveLoadManager.Instance == null)
        {
            GameObject sm = new GameObject("SaveLoadManager");
            sm.AddComponent<SaveLoadManager>();
        }
    }

   void Start()
{
    // Chỉ load Menu nếu chưa có scene nào được set
    if (string.IsNullOrEmpty(GameManager.sceneToLoad))
    {
        GameManager.sceneToLoad = firstSceneName;
        SceneManager.LoadScene(firstSceneName);
    }
}

}
