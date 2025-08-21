using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    [Header("UI Loading")]
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;

    [Header("Thời gian fake load (giây)")]
    public float fakeLoadTime = 3f; // bạn chỉnh để bar chạy nhanh hay chậm

    void Start()
    {
        if (string.IsNullOrEmpty(GameManager.sceneToLoad))
        {
            Debug.LogError("⚠ GameManager.sceneToLoad chưa được gán!");
            return;
        }

        StartCoroutine(FakeLoadingThenSwitch());
    }

    IEnumerator FakeLoadingThenSwitch()
    {
        float elapsed = 0f;

        // chạy thanh từ 0 → 100% trong "fakeLoadTime" giây
        while (elapsed < fakeLoadTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / fakeLoadTime);

            if (loadingBar != null) loadingBar.value = progress;
            if (loadingText != null) loadingText.text = $"Loading {(int)(progress * 100)}%";

            yield return null;
        }

        // đảm bảo full 100%
        if (loadingBar != null) loadingBar.value = 1f;
        if (loadingText != null) loadingText.text = "Loading 100%";

        // chờ 0.5s cho đẹp
        yield return new WaitForSeconds(0.5f);

        // chuyển scene thật
        SceneManager.LoadScene(GameManager.sceneToLoad);
    }
}
