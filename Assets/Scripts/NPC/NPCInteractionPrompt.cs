using UnityEngine;
using TMPro;
using System.Collections;

public class NPCInteractionPrompt : MonoBehaviour
{
    [Header("UI del mensaje")]
    public GameObject promptPanel;     // El panel o texto que dice "Presiona E para hablar"
    public TMP_Text promptText;

    [Header("Configuración")]
    public string mensaje = "Presiona E para hablar";
    public float tiempoVisible = 3f;   // ⏱ tiempo antes de desaparecer el mensaje

    private Coroutine ocultarCoroutine;

    void Start()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MostrarMensaje();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OcultarMensaje();
        }
    }

    void MostrarMensaje()
    {
        if (promptPanel == null) return;

        promptText.text = mensaje;
        promptPanel.SetActive(true);

        // Si ya había una coroutine activa, la reiniciamos
        if (ocultarCoroutine != null)
            StopCoroutine(ocultarCoroutine);

        ocultarCoroutine = StartCoroutine(OcultarTrasTiempo());
    }

    IEnumerator OcultarTrasTiempo()
    {
        yield return new WaitForSeconds(tiempoVisible);
        OcultarMensaje();
    }

    void OcultarMensaje()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);

        if (ocultarCoroutine != null)
        {
            StopCoroutine(ocultarCoroutine);
            ocultarCoroutine = null;
        }
    }
}
