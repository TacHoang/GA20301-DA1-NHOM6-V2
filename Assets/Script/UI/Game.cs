using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject gameUi;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;
    private bool isPaused = false;
    void Start()
    {
        MainMenu();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGameMenu();
        }
    }
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);        // Hiện Main Menu
        gameOverMenu.SetActive(false);  // Ẩn Game Over Menu
        pauseMenu.SetActive(false);     // Ẩn Pause Menu
        Time.timeScale = 0f;
        
    }

    // Hàm hiển thị Game Over Menu, ẩn các menu khác
    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);   // Hiện Game Over Menu
        mainMenu.SetActive(false);      // Ẩn Main Menu
        pauseMenu.SetActive(false);     // Ẩn Pause Menu
        Time.timeScale = 0f;
    }

    // Hàm hiển thị Pause Menu, ẩn các menu khác
    public void PauseGameMenu()
    {
        pauseMenu.SetActive(true);      // Hiện Pause Menu
        mainMenu.SetActive(false);      // Ẩn Main Menu
        gameOverMenu.SetActive(false);  // Ẩn Game Over Menu
        Time.timeScale = 0f;
        isPaused = true;
    }

            public void StartGame()
        {
            mainMenu.SetActive(false);        // Ẩn menu chính
            pauseMenu.SetActive(false);       // Ẩn menu tạm dừng
            gameOverMenu.SetActive(false);    // Ẩn màn hình thua
            Time.timeScale = 1f;              // Chạy game bình thường (tốc độ 1x)
        }

        // Hàm tiếp tục chơi sau khi pause
        public void ResumeGame()
        {
            mainMenu.SetActive(false);        // Ẩn menu chính (phòng khi người chơi quay lại menu)
            pauseMenu.SetActive(false);       // Ẩn menu pause
            gameOverMenu.SetActive(false);    // Ẩn game over menu
            Time.timeScale = 1f;              // Tiếp tục thời gian
            isPaused = false;
        }
        
}
