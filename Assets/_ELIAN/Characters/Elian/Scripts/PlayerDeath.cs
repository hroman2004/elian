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

        // Restamos la vida apenas muere (no despues del delay), asi el
        // HUD se actualiza al instante aunque el reinicio tarde un poco.
        bool hasLivesLeft = LivesManager.LoseLife();

        if (hasLivesLeft)
            Invoke(nameof(RestartScene), restartDelay);
        else
            Invoke(nameof(HandleGameOver), restartDelay);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    private void HandleGameOver()
    {
        // TODO: reemplazar esto cuando tengas armada la pantalla/escena
        // de Game Over (por ejemplo SceneManager.LoadScene("GameOver"),
        // o activar un panel de UI). Por ahora solo lo dejamos loggeado
        // para no bloquearte con el resto del HUD.
        Debug.Log("GAME OVER");
    }
}