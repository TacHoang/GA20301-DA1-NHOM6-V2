using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 6f;
    public GameObject damageZone;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private enum State { Idle, Chase, Attack }
    private State currentState = State.Idle;

    private bool isPlayerInAttackZone = false;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        DisableDamageZone();
    }

    void Update()
    {
        float xDiff = player.position.x - transform.position.x;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Lật hướng toàn bộ Boss (bao gồm các object con)
        // Trước: scale.x = 1 là phải → scale.x = -1 là trái
        if (Mathf.Abs(xDiff) > 0.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = xDiff > 0 ? -1 : 1;  // 🛠️ Đảo ngược lại hướng
            transform.localScale = scale;
        }


        // Xác định trạng thái hiện tại
        if (isPlayerInAttackZone)
        {
            currentState = State.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Idle;
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Idle:
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("isWalking", false);
                DisableDamageZone();
                break;

            case State.Chase:
                if (!isAttacking)
                {
                    Vector2 moveDir = (player.position - transform.position).normalized;
                    rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
                    anim.SetBool("isWalking", true);
                }
                break;

            case State.Attack:
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("isWalking", false);

                if (!isAttacking)
                {
                    isAttacking = true;
                    anim.SetTrigger("AttackTrigger");
                    EnableDamageZone();
                }
                break;
        }
    }

    // Gọi từ animation event ở cuối animation đánh
    public void OnAttackFinished()
    {
        DisableDamageZone();

        if (isPlayerInAttackZone)
        {
            anim.SetTrigger("AttackTrigger");
            EnableDamageZone();
        }
        else
        {
            isAttacking = false;
            currentState = State.Chase;
        }
    }

    // Gọi từ BossAttackSensor (Trigger zone)
    public void SetAttacking(bool value)
    {
        isPlayerInAttackZone = value;

        if (!value)
        {
            isAttacking = false;
            DisableDamageZone();
        }
    }

    public void EnableDamageZone()
    {
        if (damageZone != null)
            damageZone.SetActive(true);
    }

    public void DisableDamageZone()
    {
        if (damageZone != null)
            damageZone.SetActive(false);
    }
}
