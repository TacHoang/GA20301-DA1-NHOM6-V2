using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        // Nếu GameManager chưa có máu => gán max
        if (GameManager.Instance.playerHealth <= 0 || GameManager.Instance.playerHealth > maxHealth)
        {
            GameManager.Instance.playerHealth = maxHealth;
        }
        gameManager = FindAnyObjectByType<GameManagerLv2>();
        // Lấy máu từ GameManager
        currentHealth = GameManager.Instance.playerHealth;
        UpdateHealthUI();
    }

    public void Update()
    {
        if (gameManager.IsGameOver()) return;
    }
    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);

        // Cập nhật máu vào GameManager
        GameManager.Instance.playerHealth = currentHealth;

        UpdateHealthUI();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthFillImage.fillAmount = fillAmount;
        }
    }

    void Die()
    {
        Debug.Log("Player đã chết.");
        gameManager.GameOver();
    }
}
