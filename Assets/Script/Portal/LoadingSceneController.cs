using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;
    public float delayTime = 0.5f;

    void Start()
    {
        if (string.IsNullOrEmpty(GameManager.sceneToLoad))
        {
            Debug.LogError("GameManager.sceneToLoad chưa gán!");
            return;
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
            if (loadingBar != null) loadingBar.value = progress;
            if (loadingText != null) loadingText.text = $"Loading... {(int)(progress * 100)}%";
            yield return null;
        }

        float elapsed = 0f;
        while (elapsed < delayTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Lerp(0f, 1f, elapsed / delayTime);
            if (loadingBar != null) loadingBar.value = progress;
            if (loadingText != null) loadingText.text = $"Loading {(int)(progress * 100)}%";
            yield return null;
        }

        if (loadingBar != null) loadingBar.value = 1f;
        if (loadingText != null) loadingText.text = "Loading 100%";

        operation.allowSceneActivation = true;
    }
}
