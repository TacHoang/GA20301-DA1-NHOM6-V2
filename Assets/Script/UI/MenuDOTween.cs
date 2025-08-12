using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    public Image blackScreen;
    public CanvasGroup logoGroup;
    public RectTransform[] buttons;
    public Vector3[] buttonOffsets;

    private Vector3[] buttonsOriginalPos;

    void Start()
    {
        blackScreen.color = Color.black;
        blackScreen.raycastTarget = true;

        logoGroup.alpha = 0;
        logoGroup.blocksRaycasts = true;

        int n = buttons.Length;
        buttonsOriginalPos = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
            buttonsOriginalPos[i] = buttons[i].localPosition;
            buttons[i].gameObject.GetComponent<Button>().interactable = false;

            SetAlphaToOne(buttons[i]); // Đảm bảo alpha luôn = 1
        }

        SetHoverScaleColorActive(false);

        SlideButtonsOut();

        PlaySequence();
    }

    void SlideButtonsOut()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].DOLocalMove(buttonsOriginalPos[i] + buttonOffsets[i], 0.5f)
                .SetEase(Ease.InBack)
                .SetUpdate(UpdateType.Normal, true);
        }
    }

    void PlaySequence()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(logoGroup.DOFade(1, 2f));
        seq.Append(logoGroup.DOFade(0, 2f));
        seq.AppendCallback(() => logoGroup.blocksRaycasts = false);

        seq.Append(blackScreen.DOFade(0, 1f));
        seq.AppendCallback(() => blackScreen.raycastTarget = false);

        seq.AppendInterval(1f);

        seq.OnComplete(() =>
        {
            SlideButtonsIndividually();
        });
    }

    void SlideButtonsIndividually()
    {
        float delayBetweenButtons = 0.15f;
        int buttonsCount = buttons.Length;
        int completedCount = 0;

        for (int i = 0; i < buttonsCount; i++)
        {
            int index = i;

            SetAlphaToOne(buttons[index]);

            buttons[index].DOLocalMove(buttonsOriginalPos[index], 0.5f)
                .SetEase(Ease.OutBack)
                .SetDelay(delayBetweenButtons * index)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() =>
                {
                    SetAlphaToOne(buttons[index]);
                    completedCount++;

                    if (completedCount >= buttonsCount)
                    {
                        EnableButtonsInteractable();
                        SetHoverScaleColorActive(true);
                    }
                });
        }
    }

    void EnableButtonsInteractable()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.GetComponent<Button>().interactable = true;
            SetAlphaToOne(buttons[i]);
        }
    }

    void SetAlphaToOne(RectTransform btn)
    {
        var imgs = btn.GetComponentsInChildren<Image>();
        foreach (var img in imgs)
        {
            Color c = img.color;
            c.a = 1f;
            img.color = c;
        }
    }

    void SetHoverScaleColorActive(bool active)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var hover = buttons[i].GetComponent<HoverScaleColor>();
            if (hover != null)
            {
                hover.enabled = active;
            }
        }
    }
}
