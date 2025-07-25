using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    public Slider loadingBar; // Kéo Slider "LoadingBar" vào đây trong Inspector
    [Header("Thời gian chờ trước khi chuyển scene (giây)")]
    public float delayTime = 0.5f;

    void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(GameManager.sceneToLoad);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }

        // Bắt đầu tăng từ 0.9 → 1.0 trong delayTime
        float elapsed = 0f;
        while (elapsed < delayTime)
        {
            elapsed += Time.deltaTime;
            loadingBar.value = Mathf.Lerp(0.9f, 1f, elapsed / delayTime);
            yield return null;
        }

        loadingBar.value = 1f;
        operation.allowSceneActivation = true;
    }
}

