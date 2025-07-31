using UnityEngine;
using UnityEngine.UI;

public class C4 : MonoBehaviour
{
    [Header("Thiết lập kỹ năng C4")]
    public GameObject sharpStonePrefab;
    public float stoneSpeed = 10f;
    public float cooldownTime = 5f;

    [Header("UI Hồi chiêu")]
    public Text cooldownText; // Text hiển thị số giây
    public Image skillIcon;

    private float cooldownTimer = 0f;
    public bool canUseSkill = true;

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownText.text = Mathf.Ceil(cooldownTimer).ToString();

            Color faded = skillIcon.color;
            faded.a = 0.4f;
            skillIcon.color = faded;
        }
        else
        {
            cooldownText.text = "C4";

            Color full = skillIcon.color;
            full.a = 1f;
            skillIcon.color = full;
        }



        // Kích hoạt kỹ năng bằng phím 4
        if (Input.GetKeyDown(KeyCode.Alpha4) && cooldownTimer <= 0f && canUseSkill)
        {
            ActivateC4();
            cooldownTimer = cooldownTime;
            cooldownText.text = cooldownTime.ToString("F0");
        }
    }

    void ActivateC4()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 basePos = new Vector2(transform.position.x, transform.position.y + 0f);
        Vector2 shootDir = (mousePos - basePos).normalized;
        Vector2 sideDir = new Vector2(-shootDir.y, shootDir.x);

        Vector2[] spawnOffsets = new Vector2[]
        {
            sideDir * -1f + shootDir * -0.5f,
            shootDir * -0.5f,
            sideDir * 1f + shootDir * -0.5f
        };

        foreach (Vector2 offset in spawnOffsets)
        {
            Vector2 spawnPos = basePos + offset;
            GameObject stone = Instantiate(sharpStonePrefab, spawnPos, Quaternion.identity);

            Destroy(stone, 2.5f);

            Rigidbody2D rb = stone.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = shootDir * stoneSpeed;

            float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            stone.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}