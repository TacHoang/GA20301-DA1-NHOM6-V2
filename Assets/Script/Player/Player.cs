using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;
    public float ngang, doc;
    [SerializeField] public Animator anim;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        ngang = Input.GetAxisRaw("Horizontal");
        doc = Input.GetAxisRaw("Vertical");

        Vector2 inputVector = new Vector2(ngang, doc);

        if (inputVector.magnitude > 1)
            inputVector = inputVector.normalized;

        rb.linearVelocity = inputVector * speed;

        anim.SetBool("MoveLR", inputVector.x != 0);
        anim.SetBool("MoveUp", inputVector.y > 0);
        anim.SetBool("MoveDown", inputVector.y < 0);

        if (inputVector.x < 0)
            rb.transform.localScale = new Vector2(-1, 1);
        else if (inputVector.x > 0)
            rb.transform.localScale = new Vector2(1, 1);
    }
}
