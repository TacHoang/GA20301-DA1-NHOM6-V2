using UnityEngine;

public class Boss4 : MonoBehaviour
{
    [Header("Tấn công")]
    public GameObject damageZone;

    [Header("Chờ teleport khi mất mục tiêu")]
    public float teleportDelay = 5f;

    private Transform player;
    private Animator anim;

    private enum State { Idle, Attack }
    private State currentState = State.Idle;

    private bool isPlayerInAttackZone = false;
    private bool isAttacking = false;

    private float teleportTimer = 0f;
    private bool waitingToTeleport = false;
    private bool hasTeleportedAndAttacked = false;
    private bool isTeleporting = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        DisableDamageZone();
    }

    void Update()
    {
        if (player == null) return;

        switch (currentState)
        {
            case State.Idle:
                anim.SetBool("isWalking", false);
                break;

            case State.Attack:
                break;
        }

        HandleTeleporting();
    }

    public void SetAttacking(bool value)
    {
        isPlayerInAttackZone = value;

        if (!value)
        {
            isAttacking = false;
            DisableDamageZone();

            // Bắt đầu chờ teleport
            waitingToTeleport = true;
            hasTeleportedAndAttacked = false;
            teleportTimer = 0f;

            currentState = State.Idle;
        }
        else
        {
            // Hủy trạng thái chờ
            waitingToTeleport = false;
            teleportTimer = 0f;
            hasTeleportedAndAttacked = false;
        }
    }

    private void HandleTeleporting()
    {
        if (waitingToTeleport && !hasTeleportedAndAttacked && !isTeleporting)
        {
            teleportTimer += Time.deltaTime;
            if (teleportTimer >= teleportDelay)
            {
                isTeleporting = true;
                anim.SetTrigger("TeleportTrigger"); // ← dùng trigger Animator
            }
        }
    }

    // Gọi từ **Animation Event cuối animation Teleport**
    public void OnTeleportAnimationEnd()
    {
        TeleportAbovePlayer();
        PerformAttack();
        hasTeleportedAndAttacked = true;
        waitingToTeleport = false;
        isTeleporting = false;
    }

    private void TeleportAbovePlayer()
    {
        if (player != null)
        {
            Vector3 newPosition = player.position + new Vector3(0f, 2f, 0f);
            transform.position = newPosition;
            FlipTowardsPlayer();
        }
    }

    private void FlipTowardsPlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;
        if (player.position.x > transform.position.x)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void PerformAttack()
    {
        currentState = State.Attack;
        isAttacking = true;
        anim.SetTrigger("AttackTrigger");
        EnableDamageZone();
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

    // Gọi từ Animation Event khi đòn đánh kết thúc
    public void OnAttackAnimationComplete()
    {
        isAttacking = false;
        DisableDamageZone();

        if (!isPlayerInAttackZone)
        {
            currentState = State.Idle;
        }
        else
        {
            currentState = State.Attack;
        }
    }
}
