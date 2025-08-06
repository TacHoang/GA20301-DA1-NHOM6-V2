using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using Unity.Cinemachine;

public class TakeDameC3 : MonoBehaviour
{
    [Header("Cấu hình sát thương")]
    public int damageAmount = 50;
      public float laserLifeTime = 0.5f;
    public float fadeDuration = 0.5f;

    private SpriteRenderer sr;
    private Collider2D col;
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    private bool isFading = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (col != null)
        {
            col.enabled = true;
        }

        // Bắt đầu fade sau laserLifeTime
        Invoke(nameof(BeginFadeOut), laserLifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy")))
        {
            Debug.Log("Laser trúng: " + other.name);

            // Gây dame
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }

            hitEnemies.Add(other.gameObject);

             // 🎥 Rung màn hình
        }
    }
    public void Begin()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
        Invoke(nameof(BeginFadeOut), laserLifeTime);
    }



    void BeginFadeOut()
    {
        if (!isFading)
        {
            isFading = true;
            col.enabled = false;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        Color originalColor = sr.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}