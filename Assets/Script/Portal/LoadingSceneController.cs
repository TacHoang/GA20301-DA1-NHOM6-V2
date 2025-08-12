using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;  // Nhớ import thư viện TextMeshPro

public class LoadingSceneController : MonoBehaviour
{
    public Slider loadingBar;

    [Header("Thời gian chờ trước khi chuyển scene (giây)")]
    public float delayTime = 0.5f;

    public TextMeshProUGUI loadingText; // Thêm biến TextMeshPro

    void Start()
    {
        if (loadingBar == null)
        {
            Debug.LogError("Loading bar chưa được gán trong Inspector hoặc không tìm thấy trong scene!");
        }

        if (loadingText == null)
        {
            Debug.LogWarning("Loading text chưa được gán trong Inspector!");
        }

        if (string.IsNullOrEmpty(GameManager.sceneToLoad))
        {
            Debug.LogError("GameManager.sceneToLoad chưa được gán!");
        }

        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(GameManager.sceneToLoad);
        operation.allowSceneActivation = false;

        while (operation.progress < 0f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0f);
            loadingBar.value = progress;

            if (loadingText != null)
            {
                loadingText.text = $"Loading... {(int)(progress * 100)}%";
            }

            yield return null;
        }

        float elapsed = 0f;
        while (elapsed < delayTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Lerp(0f, 1f, elapsed / delayTime);
            loadingBar.value = progress;

            if (loadingText != null)
            {
                loadingText.text = $"Loading {(int)(progress * 100)}%";
            }

            yield return null;
        }

        loadingBar.value = 1f;

        if (loadingText != null)
        {
            loadingText.text = "Loading 100%";
        }

        operation.allowSceneActivation = true;
    }
}
