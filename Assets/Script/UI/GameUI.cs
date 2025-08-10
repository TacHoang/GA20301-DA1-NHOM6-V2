using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("Tên scene đầu tiên khi chơi mới")]
    public string firstSceneName = "Map1Lv1";

    public void OnPlayNewGameClicked()
    {
        // Xóa dữ liệu cũ
        PlayerPrefs.DeleteAll();

        // Đặt cờ New Game
        GameManager.Instance.isNewGame = true;
        GameManager.Instance.ResetData();

        // Lưu scene đầu tiên để sau Continue vẫn dùng được
        PlayerPrefs.SetString("CurrentScene", firstSceneName);
        PlayerPrefs.Save();

        LoadScene(firstSceneName);
    }

    public void OnContinueClicked()
    {
        if (PlayerPrefs.HasKey("CurrentScene"))
        {
            string savedScene = PlayerPrefs.GetString("CurrentScene");
            GameManager.Instance.isNewGame = false;
            LoadScene(savedScene);
        }
        else
        {
            Debug.Log("Không có dữ liệu để chơi tiếp");
        }
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    private void LoadScene(string sceneName)
    {
        GameManager.sceneToLoad = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}
