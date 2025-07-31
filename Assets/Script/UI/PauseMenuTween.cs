using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseMenuTween : MonoBehaviour
{
    public static PauseMenuTween Instance { get; private set; }

    [Header("Cài đặt UI")]
    public RectTransform pauseMenuPanel;
    public float duration = 0.5f;

    private bool isPaused = false;
    private Vector2 hiddenPos, visiblePos;
    private CanvasGroup canvasGroup;
    private SkillManager skillManager;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        DOTween.Init();

        visiblePos = Vector2.zero;
        hiddenPos = new Vector2(-Screen.width, 0);

        canvasGroup = pauseMenuPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = pauseMenuPanel.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        pauseMenuPanel.gameObject.SetActive(true);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        skillManager = FindObjectOfType<SkillManager>(); 
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

        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    private void PauseGame()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        pauseMenuPanel.anchoredPosition = hiddenPos;
        pauseMenuPanel.DOAnchorPos(visiblePos, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        canvasGroup.DOFade(1f, duration)
            .SetUpdate(true);

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, duration)
               .SetUpdate(true);

        AudioListener.pause = true;

        skillManager?.LockAllSkills(); 
    }

    private void ResumeGame()
    {
        pauseMenuPanel.DOAnchorPos(hiddenPos, duration)
        .SetEase(Ease.InBack)
        .SetUpdate(true);

        canvasGroup.DOFade(0f, duration)
            .SetUpdate(true);

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, duration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() => AudioListener.pause = false);

        skillManager?.UnlockAllSkills();
    }

    public bool IsGamePaused() => isPaused;
}