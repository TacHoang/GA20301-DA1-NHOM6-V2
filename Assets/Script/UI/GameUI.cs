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

        GameManager.Instance.isNewGame = true;
        GameManager.Instance.ResetData();

        // Lưu scene đầu tiên
        PlayerPrefs.SetString("CurrentScene", firstSceneName);
        PlayerPrefs.Save();

        GameManager.sceneToLoad = firstSceneName;
        SceneManager.LoadScene("LoadingScene");
    }

public void OnContinueClicked()
{
    string savedScene = SaveLoadManager.Instance.GetSavedScene();

    if (!string.IsNullOrEmpty(savedScene))
    {
        // Load dữ liệu HP + Gold trước
        SaveLoadManager.Instance.LoadAllData();

        // Set scene cần load
        GameManager.sceneToLoad = savedScene;

        // Load scene qua LoadingScene
        SceneManager.LoadScene("LoadingScene");
    }
    else
    {
        Debug.Log("Không có save để tiếp tục, vào Menu.");
        GameManager.sceneToLoad = "Menu";
        SceneManager.LoadScene("LoadingScene");
    }
}


    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
