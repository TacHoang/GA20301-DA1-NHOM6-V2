using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class C3 : MonoBehaviour
{
    [Header("Thiết lập kỹ năng C3")]
    public GameObject laserPrefab;
    public float cooldownTime = 8f;
    public AudioSource fireSound;

    [Header("UI hồi chiêu")]
    public Text cooldownText;
    public Image laserIcon;

    private float cooldownTimer = 0f;
    private bool canCast = true;

    void Update()
    {
        if (!canCast)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 0f; // ✅ Giữ timer ở mức 0
                canCast = true;
                cooldownText.text = "C3";
                laserIcon.color = new Color(laserIcon.color.r, laserIcon.color.g, laserIcon.color.b, 1f);
            }
            else
            {
                cooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
                laserIcon.color = new Color(laserIcon.color.r, laserIcon.color.g, laserIcon.color.b, 0.4f);
            }


        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && canCast)
        {
            ActivateLaser(); // không gán cooldown ở đây nữa
        }
    }





    void ActivateLaser()
    {
        if (fireSound != null) fireSound.Play();
        var impulse = GetComponent<CinemachineImpulseSource>();
        impulse?.GenerateImpulse();





        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mousePos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        float spawnOffset = 0f; // Khoảng cách trước mặt
        Vector2 spawnPos = (Vector2)transform.position + shootDir * spawnOffset;

        GameObject laser = Instantiate(laserPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
        var takeDame = laser.GetComponent<TakeDameC3>();

        if (takeDame != null)
        {
            takeDame.Begin();

            // Tránh va chạm với chính player
            Collider2D playerCol = GetComponent<Collider2D>();
            Collider2D laserCol = laser.GetComponent<Collider2D>();
            if (playerCol != null && laserCol != null)
            {
                Physics2D.IgnoreCollision(playerCol, laserCol);
            }
        }
        else
        {
            Debug.LogError("⚠️ Prefab laser chưa có TakeDameC3.cs");
        }

        canCast = false;
        cooldownTimer = cooldownTime;
        cooldownText.text = cooldownTime.ToString("0s");
    }








}
