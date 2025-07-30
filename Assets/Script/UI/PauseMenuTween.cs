using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseMenuTween : MonoBehaviour
{
    public static PauseMenuTween Instance { get; private set; }

    public RectTransform pauseMenuPanel;
    public float duration = 0.5f;

    private bool isPaused = false;
    private Vector2 hiddenPos, visiblePos;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        DOTween.Init();
        visiblePos = Vector2.zero;
        hiddenPos = new Vector2(-40, 0);
        pauseMenuPanel.anchoredPosition = hiddenPos;
        pauseMenuPanel.gameObject.SetActive(false);

        // Nếu sang scene khác, đảm bảo resume
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void OnResumeClicked() => TogglePause();
    public void OnRestartClicked()
    {
        TogglePause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnQuitClicked()
    {
        TogglePause();
        SceneManager.LoadScene("MainMenu");
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused) PauseGame();
        else ResumeGame();
    }

    private void PauseGame()
    {
        pauseMenuPanel.gameObject.SetActive(true);
        pauseMenuPanel.anchoredPosition = hiddenPos;
        pauseMenuPanel.DOAnchorPos(visiblePos, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);  // chạy menu animation khi paused

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, duration)
               .SetUpdate(true);
        AudioListener.pause = true;
    }

    private void ResumeGame()
    {
        pauseMenuPanel.DOAnchorPos(hiddenPos, duration)
           .SetEase(Ease.InBack)
           .SetUpdate(true)
           .OnComplete(() => pauseMenuPanel.gameObject.SetActive(false));

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, duration)
               .SetEase(Ease.Linear)
               .SetUpdate(true)
               .OnComplete(() => AudioListener.pause = false);
    }

    public bool IsGamePaused() => isPaused;
}
