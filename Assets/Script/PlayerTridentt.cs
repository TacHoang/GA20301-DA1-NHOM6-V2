using UnityEngine;

public class PlayerTridentt : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float timeDestroy = 1f;
    [SerializeField] private float damage = 10f;
    public float Damage => damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTrident();
    }
    void MoveTrident()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
}
