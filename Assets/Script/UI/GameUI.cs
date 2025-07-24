using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public void StartGame()
    {
        // Nếu không cần làm gì, có thể load scene luôn:
        SceneManager.LoadScene("TênScene"); // hoặc load scene kế tiếp
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
