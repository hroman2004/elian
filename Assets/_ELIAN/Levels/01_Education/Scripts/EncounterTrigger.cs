using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    [SerializeField] private GameObject firstWave;

    private bool triggered;

    private void Start()
    {
        if (firstWave != null)
            firstWave.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player"))
            return;

        triggered = true;

        if (firstWave != null)
            firstWave.SetActive(true);

        gameObject.SetActive(false);
    }
}