using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    [Header("Líneas del diálogo")]
    public string[] dialogueLines;

    private bool isPlayerNear = false;

    [Header("Referencia al PlayerInput del jugador")]
    public PlayerInput playerInput;
    private InputAction interactAction;

    [Header("UI: Indicador de interacción")]
    public GameObject interactHint;   // Asigna el texto “Presiona E para hablar” desde el Canvas

    void Start()
    {
        if (playerInput != null)
        {
            interactAction = playerInput.actions["Interact"];
            interactAction.performed += OnInteract;
        }

        if (interactHint != null)
            interactHint.SetActive(false);
    }

    void OnDisable()
    {
        if (interactAction != null)
            interactAction.performed -= OnInteract;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (interactHint != null)
                interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (interactHint != null)
                interactHint.SetActive(false);
        }
    }


    protected virtual void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerNear)
            ActivarInteraccion();
    }


    public virtual void ActivarInteraccion()
    {
        var dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager != null)
            dialogueManager.StartDialogue(dialogueLines);
    }


    //private void OnInteract(InputAction.CallbackContext context)
    //{
    //    if (isPlayerNear)
    //    {
    //        var dialogueManager = FindFirstObjectByType<DialogueManager>();
    //        if (dialogueManager != null)
    //        {
    //            Debug.Log("🗨️ Iniciando diálogo con NPC...");
    //            dialogueManager.StartDialogue(dialogueLines);
    //        }
    //    }
    //}
}






