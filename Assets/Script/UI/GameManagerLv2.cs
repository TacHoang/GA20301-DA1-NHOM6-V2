
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerLv2 : MonoBehaviour
{

    [SerializeField] private GameObject gameOverUi;
    private bool isGameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
        gameOverUi.SetActive(false);
    }


   
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        gameOverUi.SetActive(true);
    }
    public void RestarGameMap1Lv1()
    {
        isGameOver = false;
     
        Time.timeScale = 1;
        SceneManager.LoadScene("Map1Lv1.");
    }
    public void RestarGameMap1Lv2()
    {
        isGameOver = false;

        Time.timeScale = 1;
        SceneManager.LoadScene("Map1Lv2");
    }
    public void RestarGameMap1Lv3()
    {
        isGameOver = false;

        Time.timeScale = 1;
        SceneManager.LoadScene("Map1Lv3");
    }
    public void RestarGameMap2Lv1()
    {
        isGameOver = false;

        Time.timeScale = 1;
        SceneManager.LoadScene("Map2Lv1");
    }
    public void RestarGameMap2Lv2()
    {
        isGameOver = false;

        Time.timeScale = 1;
        SceneManager.LoadScene("Map2Lv2");
    }
    public void RestarGameMap2Lv3()
    {
        isGameOver = false;

        Time.timeScale = 1;
        SceneManager.LoadScene("Map2Lv3");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }
}
