using UnityEngine;
using UnityEngine.UI;
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
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager chưa tồn tại!");
            return;
        }

        if (GameManager.Instance.playerHealth <= 0 || GameManager.Instance.playerHealth > maxHealth)
        {
            GameManager.Instance.playerHealth = maxHealth;
        }

        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManagerLv2>();
        }

        currentHealth = GameManager.Instance.playerHealth;
        UpdateHealthUI(true);
    }

    void Update()
    {
        if (gameManager != null && gameManager.IsGameOver()) return;
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
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        GameManager.Instance.playerHealth = currentHealth;
        UpdateHealthUI();
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
                healthFillImage.DOFillAmount(targetFill, 0.4f).SetEase(Ease.OutCubic);
            }
        }
        else
        {
            Debug.LogWarning("healthFillImage chưa được gán trong PlayerHealth!");
        }
    }

    void Die()
    {
        Debug.Log("Player đã chết.");
        if (gameManager != null)
            gameManager.GameOver();
    }
}
