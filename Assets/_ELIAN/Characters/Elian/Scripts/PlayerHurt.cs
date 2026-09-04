using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
public class PlayerHurt : MonoBehaviour
{
    [Header("Reaccion al daño")]
    [SerializeField] private float hurtLockDuration = 0.35f;

    private Health health;
    private Animator animator;
    private PlayerController playerController;
    private Rigidbody2D rb;

    private void Awake()
    {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        health.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int current, int max)
    {
        if (current <= 0)
            return;

        animator.SetTrigger("Hurt");

        StopAllCoroutines();
        StartCoroutine(HurtLock());
    }

    private IEnumerator HurtLock()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(hurtLockDuration);

        if (!health.IsDead && playerController != null)
            playerController.enabled = true;
    }
}