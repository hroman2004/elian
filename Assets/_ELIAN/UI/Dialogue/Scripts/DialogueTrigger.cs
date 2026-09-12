using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Sistema de dialogo")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Lineas")]
    [SerializeField] private DialogueManager.DialogueLine[] lines;

    [Header("Activar al terminar")]
    [SerializeField] private GameObject[] activateOnEnd;

    private bool triggered;

    private void Start()
    {
        if (activateOnEnd == null)
            return;

        foreach (GameObject obj in activateOnEnd)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player"))
            return;

        if (dialogueManager == null)
            return;

        triggered = true;

        dialogueManager.StartDialogue(
            lines,
            FinishDialogue
        );
    }

    private void FinishDialogue()
    {
        if (activateOnEnd != null)
        {
            foreach (GameObject obj in activateOnEnd)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }

        gameObject.SetActive(false);
    }
}
