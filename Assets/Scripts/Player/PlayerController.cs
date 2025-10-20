using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3f;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.down;

    [Header("Componentes")]
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerControls controls;

    [Header("Ataque")]
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask hitLayer; // layer de enemigos
    public int swordDamage = 1;
    public float attackCooldown = 0.4f;
    private bool canAttack = true;

    [Header("Stats")]
    public int maxLives = 3;
    private int currentLives;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controls = new PlayerControls();
        currentLives = maxLives;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("SpawnX") && PlayerPrefs.HasKey("SpawnY"))
        {
            float x = PlayerPrefs.GetFloat("SpawnX");
            float y = PlayerPrefs.GetFloat("SpawnY");
            transform.position = new Vector2(x, y);
        }
    }


    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Attack.performed += ctx => OnAttack();
        controls.Player.Interact.performed += ctx => OnInteract();
    }

    private void OnDisable()
    {
        controls.Player.Attack.performed -= ctx => OnAttack();
        controls.Player.Interact.performed -= ctx => OnInteract();
        controls.Disable();
    }

    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput.normalized;
        }

        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetBool("IsMoving", moveInput.sqrMagnitude > 0.01f);

        animator.SetFloat("LastMoveX", lastMoveDir.x);
        animator.SetFloat("LastMoveY", lastMoveDir.y);
    }

    private void OnAttack()
    {
        if (!canAttack) return;

        animator.SetTrigger("Attack");
        StopAllCoroutines();
        canAttack = false;
        Invoke(nameof(ResetAttack), attackCooldown);

        // Detectar enemigos en rango
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, hitLayer);
        foreach (var col in hits)
        {
            var enemy = col.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(swordDamage);
            }
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    // Interact�a con NPCs cercanos (usa un OverlapCircle para encontrar un NPC con script NPCDialog)
    private void OnInteract()
    {
        float interactRadius = 1.0f;
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRadius, LayerMask.GetMask("NPC"));
        if (hit != null)
        {
            var dialog = hit.GetComponent<NPCDialog>();
            if (dialog != null) dialog.TriggerDialog();
        }
    }
    //llamada desde otros scripts al recibir da�o
    public void TakeDamage(int amount)
    {
        currentLives -= amount;
        if (currentLives < 0) currentLives = 0;

        // Actualiza UI
        UIManager.Instance.SetLives(currentLives);

        animator.SetTrigger("Damage");

        if (currentLives <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Death");
        // podr�as desactivar controles:
        enabled = false;
        rb.linearVelocity = Vector2.zero;
        // ir al Game Over (esperar animaci�n o llamar directamente)
        GameManager.Instance.TriggerGameOver();
    }
    

    // para visualizar el radio de ataque en scene
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1f); // interact radius
    }

    // m�todos p�blicos para setear spawn inicial (Portales usan PlayerPrefs en tu Portal script)
    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }
}

