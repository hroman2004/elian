using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public enum Speaker
    {
        Eco,
        Elian,
        Sophia
    }

    [Serializable]
    public class DialogueLine
    {
        public Speaker speaker;

        [TextArea(2, 4)]
        public string text;
    }

    [Header("Interfaz")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private Image portraitImage;

    [Header("Retratos")]
    [SerializeField] private Sprite elianPortrait;
    [SerializeField] private Sprite sophiaPortrait;
    [SerializeField] private Sprite ecoPortrait;

    private DialogueLine[] currentLines;
    private int currentLine;
    private bool dialogueActive;

    private Action onDialogueFinished;

    private PlayerController playerController;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
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

    public void StartDialogue(
        DialogueLine[] lines,
        Action onFinished = null)
    {
        if (lines == null || lines.Length == 0)
            return;

        currentLines = lines;
        currentLine = 0;
        dialogueActive = true;
        onDialogueFinished = onFinished;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerRb = player.GetComponent<Rigidbody2D>();
            playerAnimator = player.GetComponent<Animator>();

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
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowLine();
    }

    private void ShowLine()
    {
        DialogueLine line = currentLines[currentLine];

        if (dialogueText != null)
            dialogueText.text = line.text;

        UpdateSpeaker(line.speaker);
    }

    private void UpdateSpeaker(Speaker speaker)
    {
        string speakerName = "";
        Sprite portrait = null;

        switch (speaker)
        {
            case Speaker.Eco:
                speakerName = "ECO ESTUDIANTE";
                portrait = ecoPortrait;
                break;

            case Speaker.Elian:
                speakerName = "ELIAN";
                portrait = elianPortrait;
                break;

            case Speaker.Sophia:
                speakerName = "SOPHIA";
                portrait = sophiaPortrait;
                break;
        }

        if (speakerNameText != null)
            speakerNameText.text = speakerName;

        if (portraitImage != null)
        {
            portraitImage.sprite = portrait;
            portraitImage.gameObject.SetActive(portrait != null);
        }
    }

    private void NextLine()
    {
        currentLine++;

        if (currentLine >= currentLines.Length)
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

        onDialogueFinished?.Invoke();
        onDialogueFinished = null;
    }
}
