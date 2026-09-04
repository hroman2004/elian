using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [Header("Deteccion")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 1.5f;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField, Min(1)] private int attackDamage = 1;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private Health playerHealth;

    private bool facingRight = true;
    private float lastAttackTime = -99f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerHealth = p.GetComponent<Health>();
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(
            transform.position, player.position);

        if (distance <= attackRange)
        {
            animator.SetFloat("Speed", 0f);
            LookAtPlayer();

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;

                if (playerHealth != null && !playerHealth.IsDead)
                    playerHealth.TakeDamage(attackDamage);
            }
        }
        else if (distance <= detectionRange)
        {
            animator.SetFloat("Speed", 1f);
            LookAtPlayer();
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(
            transform.position, player.position);

        if (distance <= detectionRange && distance > attackRange)
        {
            float dir = Mathf.Sign(
                player.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(
                dir * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    private void LookAtPlayer()
    {
        bool playerIsRight = player.position.x > transform.position.x;

        if (playerIsRight != facingRight)
        {
            facingRight = playerIsRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}