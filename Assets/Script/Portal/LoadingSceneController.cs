using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    public Slider loadingBar;
    [Header("Thời gian chờ trước khi chuyển scene (giây)")]
    public float delayTime = 0.5f;

    void Start()
{
    if (loadingBar == null)
    {
        Debug.LogError("Loading bar chưa được gán trong Inspector hoặc không tìm thấy trong scene!");
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

        // Load scene cho tới khi progress đạt 0.9 (Unity giữ ở đây tới khi cho phép kích hoạt)
        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }

        // Tăng từ 0.9 → 1.0 trong delayTime
        float elapsed = 0f;
        while (elapsed < delayTime)
        {
            elapsed += Time.deltaTime;
            loadingBar.value = Mathf.Lerp(0f, 1f, elapsed / delayTime);
            yield return null;
        }

        loadingBar.value = 1f;
        operation.allowSceneActivation = true;
    }
}
