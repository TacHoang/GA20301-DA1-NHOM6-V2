using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class HoverScaleColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Tween tScale, tColor;
    private Vector3 orig;
    private Color origColor;
    public Image img;

    void Awake()
    {
        // Lấy scale gốc
        orig = transform.localScale;

        // Tự động lấy Image nếu chưa gán
        if (!img) img = GetComponent<Image>();

        if (img == null)
        {
            Debug.LogError("Thiếu component Image trên " + gameObject.name);
            return;
        }

        origColor = img.color;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (img == null) return;

        tScale?.Kill();
        tColor?.Kill();

        // Phóng to + đổi màu khi hover (vẫn chạy kể cả khi game bị pause)
        tScale = transform.DOScale(orig * 1.1f, 0.2f)
                          .SetEase(Ease.OutQuad)
                          .SetUpdate(true); // dùng unscaled time

        tColor = img.DOColor(Color.yellow, 0.2f)
                    .SetEase(Ease.OutQuad)
                    .SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (img == null) return;

        tScale?.Kill();
        tColor?.Kill();

        // Trở về scale và màu gốc
        tScale = transform.DOScale(orig, 0.2f)
                          .SetEase(Ease.OutQuad)
                          .SetUpdate(true);

        tColor = img.DOColor(origColor, 0.2f)
                    .SetEase(Ease.OutQuad)
                    .SetUpdate(true);
    }
}
