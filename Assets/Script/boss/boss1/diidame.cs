using UnityEngine;

public class DamageZoneMotion : MonoBehaviour
{
    private Vector2 originalPos;
    public float jiggleAmount = 0.01f; // chuyển động rất nhẹ
    public float jiggleSpeed = 10f;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        // Dao động nhẹ theo chiều ngang
        float offset = Mathf.Sin(Time.time * jiggleSpeed) * jiggleAmount;
        transform.localPosition = originalPos + new Vector2(offset, 0);
    }

    void OnDisable()
    {
        transform.localPosition = originalPos; // Reset lại khi tắt
    }
}
