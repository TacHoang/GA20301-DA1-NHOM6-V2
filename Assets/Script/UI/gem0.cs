using UnityEngine;
using UnityEngine.UI;

public class gem0 : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);        // Hiện Main Menu

        Time.timeScale = 0f;

    }
    
    public void StartGame()
        {
            mainMenu.SetActive(false);        // Ẩn menu chính
            
            Time.timeScale = 1f;              // Chạy game bình thường (tốc độ 1x)
        }
}
