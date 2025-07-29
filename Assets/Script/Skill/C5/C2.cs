using UnityEngine;
using UnityEngine.UI;

public class C2 : MonoBehaviour
{
    [Header("Thiết lập kỹ năng C2")]
    public GameObject circlePrefab;
    public float cooldownTime = 6f;

    [Header("UI hồi chiêu")]
    public Text cooldownText;
    public Image skillIcon;

    private float cooldownTimer = 0f;
    public bool canUseSkill = true;
    void Update()
    {
        // Hiển thị hồi chiêu
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
            cooldownText.text = "C2";
            Color full = skillIcon.color;
            full.a = 1f;
            skillIcon.color = full;
        }

        // Kích hoạt kỹ năng bằng phím 2
        if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && cooldownTimer <= 0f && canUseSkill)
        {
            ActivateC2();
            cooldownTimer = cooldownTime;
            cooldownText.text = cooldownTime.ToString("F0");
        }
    }

    void ActivateC2()
    {
        if (circlePrefab != null)
        {
            GameObject circle = Instantiate(circlePrefab, transform.position, Quaternion.identity);
            circle.transform.SetParent(transform);
            Debug.Log("✅ Chiêu C2 đã được kích hoạt");

            // Phát âm thanh nếu có AudioSource
            AudioSource audio = circle.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play(); // Bật âm thanh khi vòng được tạo
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Chưa gán prefab vòng tròn C2!");
        }
    }
}