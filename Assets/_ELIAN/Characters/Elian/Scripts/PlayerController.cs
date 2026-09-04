using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Deteccion de suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Disparo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointCrouch;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerControls controls;

    private Vector2 moveInput = Vector2.zero;

    private bool isGrounded = false;
    private bool isCrouching = false;
    private bool isAimingUp = false;
    private bool facingRight = true;

    private bool jumpRequested = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        // El input se lee cada frame para que responda inmediatamente.
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        isCrouching = moveInput.y < 0f && isGrounded;
        isAimingUp = moveInput.y > 0f;

        float horizontal = isCrouching ? 0f : moveInput.x;

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetBool("AimUp", isAimingUp);

        if (horizontal > 0.01f && !facingRight)
            Flip();
        else if (horizontal < -0.01f && facingRight)
            Flip();

        // Registramos el salto inmediatamente.
        if (controls.Player.Jump.WasPressedThisFrame() && isGrounded)
        {
            jumpRequested = true;
        }

        // El disparo no necesita esperar al ciclo de fisica.
        if (controls.Player.Fire.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            jumpRequested = false;
        }

        float horizontal = isCrouching ? 0f : moveInput.x;

        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void Shoot()
    {
        animator.SetTrigger("Attack");

        Transform spawn = firePoint;

        if (isAimingUp)
            spawn = firePointUp;
        else if (isCrouching)
            spawn = firePointCrouch;

        Instantiate(
            bulletPrefab,
            spawn.position,
            spawn.rotation
        );
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}
