using UnityEngine;
using TMPro;

public class LivesCountUI : MonoBehaviour
{
    [SerializeField] private TMP_Text livesText;

    private void OnEnable()
    {
        LivesManager.LivesChanged += UpdateText;
    }

    private void OnDisable()
    {
        LivesManager.LivesChanged -= UpdateText;
    }

    private void Start()
    {
        // Pintamos el valor actual apenas arranca, ya que LivesChanged
        // solo se dispara cuando el numero cambia, no al inicio.
        UpdateText(LivesManager.CurrentLives);
    }

    private void UpdateText(int lives)
    {
        livesText.text = "x" + lives;
    }
}
