using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;
    public float ngang, doc;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
        void Update()
        {
            ngang = Input.GetAxisRaw("Horizontal");
            doc = Input.GetAxisRaw("Vertical");

            Vector2 inputVector = new Vector2(ngang, doc);

            // Chuẩn hóa vector nếu đang di chuyển theo hướng chéo
            if (inputVector.magnitude > 1)
            {
                inputVector = inputVector.normalized;
            }

            rb.linearVelocity = inputVector * speed;

            // Xử lý animation
            anim.SetBool("MoveLR", inputVector.x != 0);
            anim.SetBool("MoveUp", inputVector.y > 0);
            anim.SetBool("MoveDown", inputVector.y < 0);

            // Xử lý hướng nhìn nhân vật
            if (inputVector.x < 0)
            {
                rb.transform.localScale = new Vector2(-1, 1);
            }
            else if (inputVector.x > 0)
            {
                rb.transform.localScale = new Vector2(1, 1);
            }
        }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(""))
        {
            Destroy(this.gameObject, 0.5f);
            Time.timeScale = 0f;
        }
    }
}
