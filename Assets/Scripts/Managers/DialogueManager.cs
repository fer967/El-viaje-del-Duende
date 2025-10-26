using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject dialogPanel;
    public TMP_Text dialogText;

    private string[] lines;
    private int currentLine = 0;
    private bool isActive = false;
    private bool isTyping = false;
    private bool skipTyping = false;  

    private PlayerControls controls;

    [Header("Configuración")]
    public float tiempoAutoCierre = 2f;
    public float velocidadEscritura = 0.03f;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Interact.performed -= OnInteract;
        controls.Player.Disable();
    }

    public void StartDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        currentLine = 0;
        dialogPanel.SetActive(true);
        isActive = true;

        StartCoroutine(EnableInputNextFrame());
        StartCoroutine(EscribirLinea(lines[currentLine]));
        
    }

    private IEnumerator EnableInputNextFrame()
    {
        yield return null;
        controls.Player.Interact.performed += OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!isActive) return;

        
        if (isTyping)
        {
            skipTyping = true;
            return;
        }
      
        currentLine++;

        if (currentLine < lines.Length)
        {
            StartCoroutine(EscribirLinea(lines[currentLine]));
        }
        else
        {
            StartCoroutine(CerrarDespuesDeTiempo());
        }
    }


    private IEnumerator EscribirLinea(string linea)
    {
        isTyping = true;
        skipTyping = false;
        dialogText.text = "";

        foreach (char letra in linea.ToCharArray())
        {
            // Si el jugador presiona para saltar, muestra toda la línea instantáneamente
            if (skipTyping)
            {
                dialogText.text = linea;
                break;
            }

            dialogText.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        isTyping = false;
    }


    private IEnumerator CerrarDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoAutoCierre);
        EndDialogue();
    }


    void EndDialogue()
    {
        isActive = false;
        dialogPanel.SetActive(false);
        controls.Player.Interact.performed -= OnInteract;
    }
}













