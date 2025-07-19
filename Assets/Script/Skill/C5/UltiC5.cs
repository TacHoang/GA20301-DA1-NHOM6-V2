using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UltiC5 : MonoBehaviour
{
    [Header("Prefab kỹ năng")]
    public GameObject meteorPrefab;
    public GameObject orbPrefab;

    [Header("Thông số kỹ năng")]
    public float meteorDelay = 0.4f;
    public float orbDelay = 0.6f;
    public float meteorSpeed = 5f;
    public float orbSpeed = 3f;
    public float meteorLifetime = 5f;
    public float orbLifetime = 8f;

    [Header("Hồi chiêu kỹ năng")]
    public float cooldownTime = 30f;
    private float cooldownTimer = 0f;
    private bool isSkillActive = false;

    [Header("Giao diện UI")]
    public Button skillButton;
    public Text cooldownText;

    private GameObject orbInstance;

    void Start()
    {
        // Gán hàm gọi kỹ năng khi nút được bấm
        skillButton.onClick.AddListener(OnSkillButtonPressed);
    }

    void Update()
    {
        // Kích hoạt kỹ năng bằng phím "1"
        if (Input.GetKeyDown(KeyCode.Alpha5) && CanActivateSkill())
        {
            StartCoroutine(ActivateSkill());
        }

        // Cập nhật thời gian hồi chiêu
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownText.text = $"{Mathf.CeilToInt(cooldownTimer)}s";
            skillButton.interactable = false;
        }
        else
        {
            cooldownText.text = "C5";
            skillButton.interactable = true;
        }
    }

    public void OnSkillButtonPressed()
    {
        if (CanActivateSkill())
        {
            StartCoroutine(ActivateSkill());
        }
    }

    private bool CanActivateSkill()
    {
        return !isSkillActive && cooldownTimer <= 0f;
    }

    IEnumerator ActivateSkill()
    {
        isSkillActive = true;
        cooldownTimer = cooldownTime;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 spawnOffsetY = new Vector2(0, 10f);
        Vector2 orbSpawnPos = mousePos + spawnOffsetY;

        // 🌀 Tạo quả cầu đứng yên phía trên chuột
        orbInstance = Instantiate(orbPrefab, orbSpawnPos, Quaternion.identity);
        Destroy(orbInstance, orbLifetime);

        DameC5 orbDmg = orbInstance.GetComponent<DameC5>();
        if (orbDmg != null)
        {
            orbDmg.damageAmount = 200;
            orbDmg.destroyOnHit = false;
        }

        // ☄️ Tạo 20 thiên thạch rơi phía trên chuột
        for (int i = 0; i < 20; i++)
        {
            float offsetX = Random.Range(-6f, 6f);
            Vector2 meteorSpawn = new Vector2(mousePos.x + offsetX, mousePos.y + 10f);
            GameObject meteor = Instantiate(meteorPrefab, meteorSpawn, Quaternion.identity);
            Destroy(meteor, meteorLifetime);

            Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0;
                rb.linearVelocity = Vector2.down * meteorSpeed;
            }

            DameC5 dmg = meteor.GetComponent<DameC5>();
            if (dmg != null)
            {
                dmg.damageAmount = 15;
                dmg.destroyOnHit = true;
            }

            yield return new WaitForSeconds(meteorDelay);
        }

        // ⏳ Chờ xong thiên thạch rồi mới cho quả cầu rơi
        yield return new WaitForSeconds(orbDelay);

        Rigidbody2D orbRb = orbInstance.AddComponent<Rigidbody2D>();
        orbRb.gravityScale = 0;
        orbRb.linearVelocity = Vector2.down * orbSpeed;

        yield return new WaitForSeconds(2f);
        isSkillActive = false;
    }
}
