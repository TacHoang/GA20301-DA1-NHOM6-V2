using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening; // Nếu bạn dùng DOTween

public class GameUI : MonoBehaviour
{
    [Header("Tên màn chơi muốn chuyển đến sau khi nhấn Play")]
    public string sceneToLoad = "Map1Lv1"; // Màn chơi chính sau khi load xong

    [Header("Canvas loading có hiệu ứng fade")]
    public GameObject loadingCanvas;

    // Hàm gọi khi ấn nút "Play"
    public void OnPlayButtonClicked()
    {
        StartCoroutine(LoadWithFade());
    }

    IEnumerator LoadWithFade()
    {
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(true);

            // Làm mờ canvas dần lên
            CanvasGroup cg = loadingCanvas.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
                cg.DOFade(1f, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                // Nếu không có CanvasGroup thì vẫn đợi 0.5s
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Gán scene cần load để LoadingScene biết
        GameManager.sceneToLoad = sceneToLoad;

        // Chuyển sang LoadingScene
        SceneManager.LoadScene("LoadingScene");
    }

    // Hàm gọi khi ấn nút "Quit"
    public void OnQuitButtonClicked()
    {
        Debug.Log("oke r");
        Application.Quit();
        Debug.Log("Thoát game");
    }

    // Hàm gọi khi ấn nút "Main Menu"
    public void OnMainMenuButtonClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
