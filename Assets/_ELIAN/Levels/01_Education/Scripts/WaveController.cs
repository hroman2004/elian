using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Header("Siguiente oleada")]
    [SerializeField] private GameObject nextWave;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialText;
    [SerializeField] private bool hideTutorialWhenComplete;

    [Header("Desbloqueo")]
    [SerializeField] private GameObject deactivateOnComplete;

    private bool completed;

    private void Update()
    {
        if (completed)
            return;

        if (transform.childCount > 0)
            return;

        completed = true;

        if (nextWave != null)
            nextWave.SetActive(true);

        if (hideTutorialWhenComplete && tutorialText != null)
            tutorialText.SetActive(false);

        if (deactivateOnComplete != null)
            deactivateOnComplete.SetActive(false);
    }
}