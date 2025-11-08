
// version corregida
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
        // Asegura que el collider sea trigger
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

        // 🎬 Reproduce animación
        if (animator != null)
            animator.SetTrigger("Open");

        // 🟢 Desbloquea habilidad (ya muestra mensaje internamente)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnlockTitanPunch();
            Debug.Log("Cofre activado y habilidad desbloqueada");
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró GameManager en la escena.");
        }

        // 🚫 Evita reactivarse
        GetComponent<Collider2D>().enabled = false;
    }
}





//using UnityEngine;

//[RequireComponent(typeof(Collider2D))]
//public class ChestAbility : MonoBehaviour
//{
//    [Header("Animación")]
//    public Animator animator;

//    [Header("Estado")]
//    private bool isOpened = false;

//    private void Awake()
//    {
//        // Asegura que el collider sea trigger
//        var col = GetComponent<Collider2D>();
//        col.isTrigger = true;
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (isOpened) return;

//        if (collision.CompareTag("Player"))
//        {
//            OpenChest();
//        }

//        Debug.Log($"Entró al trigger con: {collision.name}");
//    }

//    private void OpenChest()
//    {
//        isOpened = true;

//        if (animator != null)
//            animator.SetTrigger("Open");

//        // 🔹 Desbloquea habilidad en GameManager
//        if (GameManager.Instance != null)
//            GameManager.Instance.UnlockTitanPunch();

//        // 🔹 Mensaje visual
//        if (UIManager.Instance != null)
//            UIManager.Instance.ShowMessage("¡Has obtenido la habilidad Puño Titánico!");

//        Debug.Log("Cofre activado");



//        // 🔹 Evita reactivarse
//        GetComponent<Collider2D>().enabled = false;
//    }
//}


// 🔹 (Opcional) Sonido o partículas
// AudioManager.Instance.Play("ChestOpen");
// Instantiate(vfxPrefab, transform.position, Quaternion.identity);
