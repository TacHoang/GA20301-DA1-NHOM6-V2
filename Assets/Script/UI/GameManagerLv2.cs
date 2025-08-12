using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;



public class GameManagerLv2 : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject gameOverUi;
    private bool isGameOver = false;
    [SerializeField] private TMP_Text deathQuoteText;

    [SerializeField]
    private string[] deathQuotes = {
        "Phản xạ trong thế giới này rất quan trọng......",
        "Hãy chỉ chuột và nhắm bắt thật chuẩn xác....",
        "Không nên chủ quan khi thấy mình đang mạnh lên....",
        "Hãy sử dụng kỹ năng hợp lý để giúp sống xót",
        "Kỹ năng rất quan trọng nhưng trình độ mới là cái nhất...",
        "Những quái vật di chuyển rất nhanh hãy tránh né......"

    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        gameOverUi.SetActive(false);
    }


   
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        if (playerObject != null)
        {
            Destroy(playerObject); // Xóa người chơi
        }

        if (deathQuoteText != null && deathQuotes.Length > 0)
        {
            int randomIndex = Random.Range(0, deathQuotes.Length);
            deathQuoteText.text = deathQuotes[randomIndex]; // Hiển thị câu nói ngẫu nhiên
        }
        GameManager.Instance?.ResetData();         // ← reset máu và vàng
    CoinManager.Instance?.ResetCoin();         // ← reset UI hiển thị vàng
        gameOverUi.SetActive(true);
    }
    public void RestarGameMap1Lv1()
    {
        isGameOver = false;
        Time.timeScale = 1;
        
    GameManager.Instance?.ResetData();         // ← reset máu và vàng
    CoinManager.Instance?.ResetCoin();         // ← reset UI hiển thị vàng
        SceneManager.LoadScene("Map1Lv1.");

    }
    public void RestarGameMap1Lv2()
    {
        isGameOver = false;
        Time.timeScale = 1;
        
        GameManager.Instance?.ResetData();         // ← reset máu và vàng
        CoinManager.Instance?.ResetCoin();         // ← reset UI hiển thị vàng
        SceneManager.LoadScene("Map1Lv2");
    }
    public void RestarGameMap1Lv3()
    {
        isGameOver = false;
        Time.timeScale = 1;
        
    GameManager.Instance?.ResetData();         // ← reset máu và vàng
    CoinManager.Instance?.ResetCoin();         // ← reset UI hiển thị vàng
        SceneManager.LoadScene("Map1Lv3");
    }
    public void RestarGameMap2Lv1()
    {
        isGameOver = false;

        Time.timeScale = 1;
        
    GameManager.Instance?.ResetData();         // ← reset máu và vàng
    CoinManager.Instance?.ResetCoin();         // ← reset UI hiển thị vàng
        SceneManager.LoadScene("Map2Lv1");
    }
    public void RestarGameMap2Lv2()
    {
        isGameOver = false;

        Time.timeScale = 1;
        
    GameManager.Instance?.ResetData();         // ← reset máu và vàng
    CoinManager.Instance?.ResetCoin();         // ← reset UI hiển thị vàng
        SceneManager.LoadScene("Map2Lv2");
    }
    public void RestarGameMap2Lv3()
    {
        isGameOver = false;

        Time.timeScale = 1;
        
    GameManager.Instance?.ResetData();         // ← reset máu và vàng
    CoinManager.Instance?.ResetCoin();         // ← reset UI hiển thị vàng
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
