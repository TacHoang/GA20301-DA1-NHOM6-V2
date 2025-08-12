using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;
    public Animator anim;

    [Header("Attack Settings")]
    public float attackCooldown = 0.15f;

    private float ngang, doc;
    private bool isAttacking = false;
    private bool canMove = true;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Nhấn chuột trái để tấn công nếu chưa tấn công
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartAttack();
        }

        // Di chuyển nếu được phép
        if (canMove)
        {
            HandleMovement();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("MoveLR", false);
            anim.SetBool("MoveUp", false);
            anim.SetBool("MoveDown", false);
        }
    }

    void HandleMovement()
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
            transform.localScale = new Vector2(-1, 1);
        else if (inputVector.x > 0)
            transform.localScale = new Vector2(1, 1);
    }

    void StartAttack()
    {
        isAttacking = true;
        canMove = false;

        // Quay mặt theo hướng chuột
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPos - transform.position;
        direction.z = 0;

        if (direction.x < 0)
            transform.localScale = new Vector2(-1, 1); // Quay trái
        else if (direction.x > 0)
            transform.localScale = new Vector2(1, 1);  // Quay phải

        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("Attack");

        // Kết thúc tấn công sau cooldown
        Invoke(nameof(EndAttack), attackCooldown);
    }

    void EndAttack()
    {
        isAttacking = false;
        canMove = true;
    }
}
