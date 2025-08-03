using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("Chuyển động & Tấn công")]
    public float moveSpeed = 2f;
    public float detectionRange = 6f;
    public GameObject damageZone;

    [Header("UI")]
    public GameObject BosshealthBarCanvas; // Gán Canvas chứa Slider thanh máu (World Space)

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

        // Lật hướng Boss
        if (Mathf.Abs(xDiff) > 0.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = xDiff > 0 ? -1 : 1;
            transform.localScale = scale;
        }

        // Xác định trạng thái
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

        // Hiện/ẩn thanh máu tùy theo khoảng cách
        if (BosshealthBarCanvas != null)
        {
            if (distanceToPlayer <= detectionRange)
            {
                if (!BosshealthBarCanvas.activeSelf)
                    BosshealthBarCanvas.SetActive(true);
            }
            else
            {
                if (BosshealthBarCanvas.activeSelf)
                    BosshealthBarCanvas.SetActive(false);
            }
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

    // Gọi từ Animation Event
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

    // Gọi từ BossAttackSensor (Trigger)
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
