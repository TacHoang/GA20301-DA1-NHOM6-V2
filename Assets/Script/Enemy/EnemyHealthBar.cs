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
        if (followTarget != null)
        {
            transform.position = followTarget.position + offset;

            if (cam != null)
                transform.rotation = cam.transform.rotation;
        }
    }

    public void SetHealth(float current, float max)
    {
        float percent = Mathf.Clamp01(current / max);
        if (healthFillImage != null)
            healthFillImage.fillAmount = percent;
        else
            Debug.LogWarning("⚠️ Thiếu gán healthFillImage trong EnemyHealthBar");
    }
}

