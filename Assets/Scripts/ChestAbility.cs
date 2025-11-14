using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ChestAbility : MonoBehaviour
{
    [Header("Animación")]
    public Animator animator;

    [Header("Estado")]
    private bool isOpened = false;

    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpened) return;

        if (collision.CompareTag("Player"))
        {
            OpenChest();
        }
    }


    private void OpenChest()
    {
        isOpened = true;

        if (animator != null)
            animator.SetTrigger("Open");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnlockTitanPunch();
            Debug.Log("Cofre activado y habilidad desbloqueada");
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró GameManager en la escena.");
        }

        GetComponent<Collider2D>().enabled = false;
    }
}








// 🔹 (Opcional) Sonido o partículas
// AudioManager.Instance.Play("ChestOpen");
// Instantiate(vfxPrefab, transform.position, Quaternion.identity);
