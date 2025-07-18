using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;
    public float ngang, doc;
    public float mauhientai;
    public float mautoida = 10;
    public GameObject canvas;
    [SerializeField] public Animator anim;

    void Start()
    {
        mauhientai = mautoida;
    }

    void Update()
    {
        // Nhận input
        ngang = Input.GetAxisRaw("Horizontal");
        doc = Input.GetAxisRaw("Vertical");

        Vector2 inputVector = new Vector2(ngang, doc);

        // Nếu đi chéo thì chuẩn hóa
        if (inputVector.magnitude > 1)
        {
            inputVector = inputVector.normalized;
        }

        // Di chuyển
        rb.linearVelocity = inputVector * speed;

        // Animation
        anim.SetBool("MoveLR", inputVector.x != 0);
        anim.SetBool("MoveUp", inputVector.y > 0);
        anim.SetBool("MoveDown", inputVector.y < 0);

        // Flip hướng
        if (inputVector.x < 0)
            rb.transform.localScale = new Vector2(-1, 1);
        else if (inputVector.x > 0)
            rb.transform.localScale = new Vector2(1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            mauhientai -= 2;

            if (mauhientai <= 0)
            {
                Debug.Log("Player chết");
                Destroy(this.gameObject, 0.2f);
                canvas.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}
