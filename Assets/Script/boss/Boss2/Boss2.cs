using UnityEngine;
using System.Collections;

public class Boss2 : MonoBehaviour
{
    public float detectionRange = 6f;
    public float teleportOffsetX = 1.5f;
    public float teleportDelay = 0.5f;

    public GameObject damageZone;
    public GameObject bossHealthBarCanvas;

    public GameObject teleportZone;
    public GameObject attackZone;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;

    enum State { Idle, Chase, Attack }
    private State currState = State.Idle;

    private bool inTeleportZone = false;
    private bool inAttackZone = false;
    private bool isTeleporting = false;
    private bool isAttacking = false;
    private bool hasTeleported = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();

        if (damageZone) damageZone.SetActive(false);
        if (bossHealthBarCanvas) bossHealthBarCanvas.SetActive(false);

        if (player == null)
            Debug.LogError("[Boss2] Không tìm thấy player!");
    }

    void Update()
    {
        float dist = player == null ? Mathf.Infinity : Vector2.Distance(transform.position, player.position);

        bossHealthBarCanvas?.SetActive(dist <= detectionRange);

        if (player != null)
        {
            bool faceLeft = player.position.x < transform.position.x;
            spr.flipX = faceLeft;

            // flip tất cả object con theo boss
            foreach (Transform child in transform)
            {
                Vector3 scale = child.localScale;
                scale.x = Mathf.Abs(scale.x) * (faceLeft ? -1 : 1);
                child.localScale = scale;
            }
        }

        if (inTeleportZone)
            currState = State.Attack;
        else if (dist <= detectionRange)
            currState = State.Chase;
        else
            currState = State.Idle;
    }

    void FixedUpdate()
    {
        switch (currState)
        {
            case State.Idle:
                SetVelocity(Vector2.zero);
                anim.SetBool("isWalking", false);
                isTeleporting = isAttacking = false;
                hasTeleported = false;
                damageZone?.SetActive(false);
                break;

            case State.Chase:
                if (!isTeleporting && !hasTeleported && !inTeleportZone)
                {
                    anim.SetBool("isWalking", true);
                    Vector2 dir = (player.position - transform.position).normalized;
                    SetVelocity(dir * 2f);
                }
                else
                    anim.SetBool("isWalking", false);
                break;

            case State.Attack:
                SetVelocity(Vector2.zero);
                anim.SetBool("isWalking", false);

                if (!isTeleporting && inTeleportZone && !hasTeleported)
                {
                    isTeleporting = true;
                    anim.SetTrigger("TeleportTrigger");
                    StartCoroutine(TeleportCoroutine());
                }
                else if (!isAttacking && hasTeleported && inAttackZone)
                {
                    isAttacking = true;
                    anim.SetTrigger("AttackTrigger");
                    damageZone?.SetActive(true);
                }
                break;
        }
    }

    IEnumerator TeleportCoroutine()
    {
        yield return new WaitForSeconds(teleportDelay);
        float dir = (transform.position.x < player.position.x) ? 1 : -1;
        transform.position = player.position + Vector3.right * teleportOffsetX * dir;
        hasTeleported = true;
        isTeleporting = false;
    }

    public void OnAttackFinished()
    {
        damageZone?.SetActive(false);
        isAttacking = false;
        hasTeleported = false;
    }

    public void SetTeleportZone(bool flag) => inTeleportZone = flag;
    public void SetAttackZone(bool flag)
    {
        inAttackZone = flag;
        if (!flag) damageZone?.SetActive(false);
    }

    void SetVelocity(Vector2 v)
    {
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = v;
#else
        rb.velocity = v;
#endif
    }

    public void EnableDamageZone() => damageZone?.SetActive(true);
    public void DisableDamageZone() => damageZone?.SetActive(false);
}
