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
            OpenChest(collision);
        }
    }

    private void OpenChest(Collider2D collision)
    {
        isOpened = true;
        if (animator != null)
            animator.SetTrigger("Open");
         PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.UnlockPunchAbility();   
        }
        GetComponent<Collider2D>().enabled = false;
    }
}
















