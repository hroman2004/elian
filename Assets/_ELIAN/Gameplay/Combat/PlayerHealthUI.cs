using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Health health;
    [SerializeField] private Image[] pips; // Pip_1 a Pip_6, en orden de izquierda a derecha

    private void OnEnable()
    {
        if (health != null)
            health.HealthChanged += UpdateBar;
    }

    private void OnDisable()
    {
        if (health != null)
            health.HealthChanged -= UpdateBar;
    }

    private void Start()
    {
        // Health.Awake ya corrio antes que este Start, asi que CurrentHealth
        // esta listo. Pintamos el estado inicial una sola vez, ya que
        // HealthChanged solo se dispara en TakeDamage/RestoreFullHealth.
        if (health != null)
            UpdateBar(health.CurrentHealth, health.MaxHealth);
    }

    private void UpdateBar(int current, int max)
    {
        int pipsToShow = Mathf.CeilToInt((float)current / max * pips.Length);

        for (int i = 0; i < pips.Length; i++)
        {
            pips[i].enabled = i < pipsToShow;
        }
    }
}