using UnityEngine;

public class FreezableObject : MonoBehaviour
{
    public float freezeDistance = 20f;
    private Transform player;
    private Rigidbody2D rb;
    private MonoBehaviour[] behaviours;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        behaviours = GetComponents<MonoBehaviour>();
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > freezeDistance)
        {
            // Freeze scripts
            foreach (var b in behaviours)
                if (b != this) b.enabled = false;

            // Optional: freeze Rigidbody
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true;
            }
        }
        else
        {
            foreach (var b in behaviours)
                if (b != this) b.enabled = true;

            if (rb != null)
                rb.isKinematic = false;
        }
    }
}

