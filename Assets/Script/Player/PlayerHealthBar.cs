using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}
