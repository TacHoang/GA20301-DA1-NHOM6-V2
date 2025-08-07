using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class gangster : MonoBehaviour
{
    [Header("Chase Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;

    [Header("Patrol Settings")]
    public float patrolSpeed = 1f;
    public float patrolChangeTime = 2f;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 5f;
    public float attackCooldown = 1f;
    public float attackDelay = 0.2f;

    [Header("General Settings")]
    public bool isDead = false;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private float lastAttackTime = 0f;
    private Vector2 patrolDirection;
    private float patrolTimer;
    private bool isAttacking = false;
    private Vector3 originalScale;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        patrolTimer = patrolChangeTime;
        PickNewPatrolDirection();
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
        {
            MoveTowards(player.position, moveSpeed);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                StartCoroutine(RangedAttack());
            }
        }
        else
        {
            Patrol();
        }
    }

    void MoveTowards(Vector2 target, float speed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
        FlipSprite(dir.x);
    }

    void Patrol()
    {
        patrolTimer -= Time.fixedDeltaTime;
        if (patrolTimer <= 0f)
        {
            PickNewPatrolDirection();
        }

        Vector2 nextPos = rb.position + patrolDirection * patrolSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
        FlipSprite(patrolDirection.x);
    }

    void PickNewPatrolDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        patrolDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        patrolTimer = patrolChangeTime;
    }

    IEnumerator RangedAttack()
    {
        isAttacking = true;

        if (animator != null)
            animator.SetTrigger("shoot");

        yield return new WaitForSeconds(attackDelay);

        ShootProjectile();
        lastAttackTime = Time.time;
        isAttacking = false;
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 dir = (player.position - firePoint.position).normalized;

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
            bulletRb.linearVelocity = dir * projectileSpeed;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void FlipSprite(float xDir)
{
    if (xDir < -0.05f)
        transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    else if (xDir > 0.05f)
        transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
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

