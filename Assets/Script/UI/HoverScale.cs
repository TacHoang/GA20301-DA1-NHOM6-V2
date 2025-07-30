using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class HoverScaleColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Tween tScale, tColor;
    Vector3 orig;
    Color origColor;
    public Image img;

    void Awake() {
        orig = transform.localScale;
        if (!img) img = GetComponent<Image>();
        origColor = img?.color ?? Color.white;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        tScale?.Kill(); tColor?.Kill();
        tScale = transform.DOScale(orig * 1.1f, 0.2f).SetEase(Ease.OutQuad);
        if (img)
            tColor = img.DOColor(Color.yellow, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData e)
    {
        tScale?.Kill(); tColor?.Kill();
        tScale = transform.DOScale(orig, 0.2f).SetEase(Ease.OutQuad);
        if (img)
            tColor = img.DOColor(origColor, 0.2f).SetEase(Ease.OutQuad);
    }
}
