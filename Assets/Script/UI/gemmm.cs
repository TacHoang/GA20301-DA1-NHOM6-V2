using UnityEngine;
using UnityEngine.UI;

public class gemmm : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
        {
            
            gameOverMenu.SetActive(false);    // Ẩn màn hình thua
            Time.timeScale = 1f;              // Chạy game bình thường (tốc độ 1x)
        }
}
