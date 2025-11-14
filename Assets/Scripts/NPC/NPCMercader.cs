using UnityEngine;

public class NPCMercader : NPCDialog
{
    [Header("Configuración del Mercader")]
    public int vidasACurar = 2;
    public string mensajeCompra = "¡Has comprado +2 vidas!";

    private bool yaCompro = false;

    public override void ActivarInteraccion()
    {
        // Evitar que compre infinitas veces (opcional)
        if (!yaCompro)
        {
            // Buscar controlador de vidas
            PlayerHealthController health = FindFirstObjectByType<PlayerHealthController>();
            if (health != null)
            {
                health.Heal(vidasACurar);   // usa tu sistema actual
            }

            // Guardar en GameManager también
            if (GameManager.Instance != null)
                GameManager.Instance.playerCurrentHealth = health.currentHealth;

            // Reemplazar líneas del diálogo para esta interacción
            dialogueLines = new string[] { mensajeCompra };

            yaCompro = true;
        }
        else
        {
            // Mensaje si ya compró
            dialogueLines = new string[] { "Gracias por tu compra, viajero." };
        }

        // Llamamos al DialogueManager como siempre
        var manager = FindFirstObjectByType<DialogueManager>();
        if (manager != null)
        {
            manager.StartDialogue(dialogueLines);
        }
    }
}
