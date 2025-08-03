using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Thanh máu")]
    public Image healthFillImage;

    [Header("Theo dõi đối tượng")]
    public Transform followTarget;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Camera cam;
    private Tween currentTween;

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
        float targetPercent = Mathf.Clamp01(current / max);

        if (healthFillImage == null)
        {
            Debug.LogWarning("⚠️ Thiếu gán healthFillImage trong EnemyHealthBar");
            return;
        }

        // Nếu đang tween thì hủy để tween mới
        if (currentTween != null && currentTween.IsActive()) currentTween.Kill();

        // Tween mượt về giá trị mới
        currentTween = healthFillImage.DOFillAmount(targetPercent, 0.3f).SetEase(Ease.OutQuad);
    }
}
