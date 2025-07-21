using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Thanh máu")]
    public Image healthFillImage;

    [Header("Theo dõi đối tượng")]
    public Transform followTarget;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (followTarget == null) return;

        // Vị trí thanh máu theo enemy
        transform.position = followTarget.position + offset;

        // Giữ không bị lật khi enemy quay mặt
        transform.rotation = cam.transform.rotation;

        // Đảm bảo scale dương để không bị lật
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void SetHealth(float current, float max)
    {
        float percent = Mathf.Clamp01(current / max);
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = percent;
        }
        else
        {
            Debug.LogWarning("⚠️ Thiếu gán healthFillImage trong EnemyHealthBar");
        }
    }
}
