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

    // (Tùy chọn) nếu bạn gán trong Inspector thì sẽ ưu tiên cái này
    public SaveLoadManager saveManager;  // Gán trong Inspector nếu muốn

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

    // ▶ Resume: tiếp tục game
    public void OnResumeClicked()
    {
        TogglePause();
    }

    // 🔄 Restart → load lại scene hiện tại qua LoadingScene
    public void OnRestartClicked()
    {
        TogglePause();

        (saveManager != null ? saveManager : SaveLoadManager.Instance)?.SaveAllData();

        GameManager.Instance?.ResetData();      
        CoinManager.Instance?.ResetCoin();      

        GameManager.sceneToLoad = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene");
    }

    // ⏪ Back → thoát về Menu, lưu progress
public void OnBackClicked()
{
    Debug.Log("▶ OnBackClicked được gọi!");

    isPaused = false;

    try
    {
        SaveLoadManager.Instance?.SaveAllData();
        PlayerPrefs.Save();
    }
    catch (System.Exception e)
    {
        Debug.LogError("❌ Lỗi khi save: " + e.Message);
    }

    try
    {
        DOTween.Kill(pauseMenuPanel);
    }
    catch { }

    Time.timeScale = 1f;
    AudioListener.pause = false;

    try
    {
        skillManager?.UnlockAllSkills();
    }
    catch { }

    GameManager.sceneToLoad = "Menu";
    Debug.Log("▶ sceneToLoad set = " + GameManager.sceneToLoad);

    SceneManager.LoadScene("LoadingScene");
}



    // ❌ Thoát hẳn game
    public void OnQuitClicked()
    {
        (saveManager != null ? saveManager : SaveLoadManager.Instance)?.SaveAllData();
        PlayerPrefs.Save();
        Application.Quit();
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

        canvasGroup.DOFade(1f, duration).SetUpdate(true);

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

        canvasGroup.DOFade(0f, duration).SetUpdate(true);

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, duration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() => AudioListener.pause = false);

        skillManager?.UnlockAllSkills();
    }

    public bool IsGamePaused() => isPaused;
}
