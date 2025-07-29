using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Chase Settings")]
    public float speed = 2f;
    public float detectionRange = 5f;

    [Header("Patrol Settings")]
    public float patrolSpeed = 1f;
    public float patrolChangeTime = 2f;

    [Header("Damage Settings")]
    public float attackCooldown = 1f;
    public float attackDuration = 0.5f;

    [HideInInspector]
    public bool isDead = false;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 patrolDirection;
    private float patrolTimer;
    private float lastAttackTime = 0f;

    private Animator animator;
    public GameObject damageZone;
    private bool isAttacking = false;
    private bool playerInAttackZone = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        patrolTimer = patrolChangeTime;
        PickNewPatrolDirection();

        if (damageZone != null)
            damageZone.SetActive(false);
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        if (isAttacking)
        {
            FlipSprite(player.position.x - transform.position.x);
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRange)
            ChasePlayer();
        else
            Patrol();

        if (playerInAttackZone && Time.time - lastAttackTime >= attackCooldown)
        {
            StartCoroutine(Attack());
        }
    }

    void ChasePlayer()
    {
        MoveTowards(player.position, speed);
    }

    void Patrol()
    {
        patrolTimer -= Time.fixedDeltaTime;
        if (patrolTimer <= 0f)
            PickNewPatrolDirection();

        Vector2 nextPos = (Vector2)transform.position + patrolDirection * patrolSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
        FlipSprite(patrolDirection.x);
    }

    void MoveTowards(Vector2 target, float moveSpeed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        Vector2 nextPos = (Vector2)transform.position + dir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
        FlipSprite(dir.x);
    }

    void PickNewPatrolDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        patrolDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        patrolTimer = patrolChangeTime;
    }

    void FlipSprite(float xDir)
    {
        if (xDir < -0.05f) transform.localScale = new Vector3(-1, 1, 1);
        else if (xDir > 0.05f) transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        EnableDamageZone();

        yield return new WaitForSeconds(attackDuration);

        DisableDamageZone();
        lastAttackTime = Time.time;
        isAttacking = false;
    }

    public void SetPlayerInAttackZone(bool value)
    {
        playerInAttackZone = value;
    }

    void EnableDamageZone()
    {
        if (damageZone != null)
            damageZone.SetActive(true);
    }

    void DisableDamageZone()
    {
        if (damageZone != null)
            damageZone.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger)
            PickNewPatrolDirection();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
