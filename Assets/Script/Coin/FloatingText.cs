using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float moveSpeed = 50f;
    public float duration = 1f;

    public void Setup(string message, Color color)
    {
        text.text = message;
        text.color = color;
        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }
}
