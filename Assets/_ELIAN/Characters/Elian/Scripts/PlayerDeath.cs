using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private float restartDelay = 1.5f;

    private Health health;
    private Animator animator;

    private void Awake()
    {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health.Died += OnDeath;
    }

    private void OnDisable()
    {
        health.Died -= OnDeath;
    }

    private void OnDeath()
    {
        animator.ResetTrigger("Hurt");
        animator.SetTrigger("Die");

        PlayerHurt ph = GetComponent<PlayerHurt>();
        if (ph != null) ph.enabled = false;

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Invoke(nameof(RestartScene), restartDelay);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }
}
