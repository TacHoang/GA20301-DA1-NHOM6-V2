using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class Portal : MonoBehaviour
{
    public string targetScene = "Map1Lv2"; // Tên scene cần chuyển đến
    public GameObject loadingCanvas; // Gắn loadingCanvas có CanvasGroup vào đây

    private bool isLoading = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            StartCoroutine(StartLoadingScene());
        }
    }

    IEnumerator StartLoadingScene()
    {
        loadingCanvas.SetActive(true); // Bật canvas nếu đang tắt

        // Lấy CanvasGroup để làm hiệu ứng mờ
        CanvasGroup cg = loadingCanvas.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0f; // Bắt đầu từ mờ hoàn toàn
            cg.DOFade(1f, 0.5f); // Hiện lên trong 0.5s
            yield return new WaitForSeconds(0.5f); // Chờ hiệu ứng xong
        }

        // Lưu scene cần load (nếu có dùng GameManager)
        GameManager.sceneToLoad = targetScene;

        // Chuyển sang scene loading hoặc scene thật
        SceneManager.LoadScene("LoadingScene"); // Hoặc: SceneManager.LoadScene(targetScene);
    }
}
