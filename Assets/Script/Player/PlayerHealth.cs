using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI Image Fill")]
    public Image healthFillImage;

    [SerializeField] private GameManagerLv2 gameManager;

    void Start()
    {
        if (GameManager.Instance.playerHealth <= 0 || GameManager.Instance.playerHealth > maxHealth)
        {
            GameManager.Instance.playerHealth = maxHealth;
        }

        gameManager = FindAnyObjectByType<GameManagerLv2>();

        currentHealth = GameManager.Instance.playerHealth;
        UpdateHealthUI(true); // Gán fill ban đầu không tween
    }

    void Update()
    {
        if (gameManager.IsGameOver()) return;
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        GameManager.Instance.playerHealth = currentHealth;
        UpdateHealthUI();
        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        int oldHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        GameManager.Instance.playerHealth = currentHealth;
        UpdateHealthUI(); // sẽ tween trong này
    }

    void UpdateHealthUI(bool instant = false)
    {
        if (healthFillImage != null)
        {
            float targetFill = (float)currentHealth / maxHealth;

            if (instant)
            {
                healthFillImage.fillAmount = targetFill;
            }
            else
            {
                // Tween từ fill hiện tại đến fill mới
                healthFillImage.DOFillAmount(targetFill, 0.4f).SetEase(Ease.OutCubic);
            }
        }
    }

    void Die()
    {
        Debug.Log("Player đã chết.");
        gameManager.GameOver();
    }
}
