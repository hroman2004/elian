using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class EcoEncounterTrigger : MonoBehaviour
{
    [Header("Interfaz")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject tutorialText;

    [Header("Dialogo")]
    [TextArea(2, 4)]
    [SerializeField] private string[] lines;

    [Header("Encuentro")]
    [SerializeField] private GameObject firstWave;

    private bool triggered;
    private bool dialogueActive;
    private int currentLine;

    private PlayerController playerController;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (tutorialText != null)
            tutorialText.SetActive(false);

        if (firstWave != null)
            firstWave.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueActive)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player"))
            return;

        triggered = true;

        playerController = other.GetComponent<PlayerController>();
        playerRb = other.GetComponent<Rigidbody2D>();
        playerAnimator = other.GetComponent<Animator>();

        if (playerController != null)
            playerController.enabled = false;

        if (playerRb != null)
            playerRb.linearVelocity = Vector2.zero;

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
            playerAnimator.SetBool("IsGrounded", true);
            playerAnimator.SetBool("IsCrouching", false);
            playerAnimator.SetBool("AimUp", false);

            playerAnimator.ResetTrigger("Attack");
            playerAnimator.ResetTrigger("Hurt");

            playerAnimator.Play("elian_idle", 0, 0f);
        }

        currentLine = 0;
        dialogueActive = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowLine();
    }

    private void ShowLine()
    {
        if (lines == null || lines.Length == 0)
        {
            EndDialogue();
            return;
        }

        if (dialogueText != null)
            dialogueText.text = lines[currentLine];
    }

    private void NextLine()
    {
        currentLine++;

        if (currentLine >= lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    private void EndDialogue()
    {
        dialogueActive = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        if (firstWave != null)
            firstWave.SetActive(true);

        if (tutorialText != null)
            tutorialText.SetActive(true);

        gameObject.SetActive(false);
    }
}