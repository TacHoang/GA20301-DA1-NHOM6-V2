using UnityEngine;
using System.Collections;



public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.2f;

    private Vector3 originalPos;

    void Awake()
    {
        instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = new Vector3(originalPos.x + randomOffset.x, originalPos.y + randomOffset.y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }


}
