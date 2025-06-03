using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("NPC Settings")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogueLines;
    public bool isBlockingEnabled = true;
    public Button nextButton;

    [Header("Interaction Settings")]
    public InteractPriority interactionPriority = InteractPriority.NPC;

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private PlayerMovement playerMovement;
    
    public InteractPriority Priority => interactionPriority;
    public Vector3 Position => transform.position;

    void Start()
    {
        dialoguePanel.SetActive(false);

        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(DisplayNextLine);
        }
        else
        {
            Debug.LogError("Next Button не назначен в инспекторе!");
        }

        if (dialogueText == null)
        {
            Debug.LogError("Dialogue Text не назначен в инспекторе!");
        }
    }

    public void Interact()
    {
        if (isDialogueActive)
        {
            CloseDialogue();
        }
        else
        {
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;
        dialogueText.text = dialogueLines[currentLineIndex];
        isDialogueActive = true;

        if (isBlockingEnabled && playerMovement != null)
        {
            playerMovement.enabled = false;
            playerMovement.rb.velocity = Vector2.zero;
        }
    }

    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;

        if (isBlockingEnabled && playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        currentLineIndex = 0;
    }

    public void DisplayNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= dialogueLines.Length)
        {
            CloseDialogue();
        }
        else
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Регистрация в системе взаимодействий
            var interactionSystem = other.GetComponent<PlayerInteraction>();
            if (interactionSystem != null)
            {
                interactionSystem.RegisterInteractable(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Удаление из системы взаимодействий
            var interactionSystem = other.GetComponent<PlayerInteraction>();
            if (interactionSystem != null)
            {
                interactionSystem.UnregisterInteractable(this);
            }
            CloseDialogue();
        }
    }
}