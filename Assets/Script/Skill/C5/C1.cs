using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class C1 : MonoBehaviour
{
    [Header("Thiết lập kỹ năng C1")]
    public GameObject wavePrefab;       // Prefab của sóng siêu âm
    public float waveSpeed = 10f;       // Tốc độ bay
    public float cooldownTime = 5f;     // Thời gian hồi chiêu
    public float waveLifetime = 1.5f;   // Thời gian tồn tại

    [Header("UI hồi chiêu")]
    public Text cooldownText;           // Text hiển thị cooldown
    public Image skillIcon;             // Icon kỹ năng

    [Header("Âm thanh kỹ năng")]
    public AudioClip c1Sound;           // Âm thanh khi kích hoạt
    private AudioSource audioSource;

    private float cooldownTimer = 0f;
    public bool canUseSkill = true;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
            skillIcon.color = new Color(1f, 1f, 1f, 0.4f);
        }
        else
        {
            cooldownText.text = "C1";
            skillIcon.color = new Color(1f, 1f, 1f, 1f);

            if (canUseSkill && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)))
            {
                FireWaves();
                cooldownTimer = cooldownTime;
            }
        }
    }

    void FireWaves()
    {
        // 🔊 Phát âm thanh kích hoạt chiêu đúng 1 lần
        if (audioSource != null && c1Sound != null)
        {
            audioSource.PlayOneShot(c1Sound);
        }
        var impulse = GetComponent<CinemachineImpulseSource>();
        impulse?.GenerateImpulse();
        Vector2 origin = transform.position;

        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (Vector2 dir in directions)
        {
            GameObject wave = Instantiate(wavePrefab, origin, Quaternion.identity);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            wave.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Rigidbody2D rb = wave.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = dir * waveSpeed;
            }

            Destroy(wave, waveLifetime);
        }

        Debug.Log("✅ C1 kích hoạt: Bắn 4 xung siêu âm");
    }
}