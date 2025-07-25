using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("Tên màn chơi muốn chuyển đến sau khi nhấn Play")]
    public string sceneToLoad = "Map1Lv1"; // Đổi tên scene thật sự ở đây

    // Hàm gọi khi ấn nút "Play"
    public void OnPlayButtonClicked()
    {
        Debug.Log("nhấn oke");
        SceneManager.LoadScene(sceneToLoad);
    }

    // Hàm gọi khi ấn nút "Quit"
    public void OnQuitButtonClicked()
    {
          Debug.Log("oke r");
        Application.Quit(); // Thoát game (chỉ hoạt động khi build ra file .exe)
        Debug.Log("Thoát game"); // Hiện log khi test trong Editor
    }
}
