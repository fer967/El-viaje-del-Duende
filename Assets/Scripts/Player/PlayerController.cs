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
    public LayerMask hitLayer;
    public int swordDamage = 1;
    public float attackCooldown = 0.4f;
    private bool canAttack = true;

    [Header("Stats")]
    public int maxLives = 4;
    private int currentLives;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controls = new PlayerControls();

        // 🔹 Cargar vidas desde GameManager si existen
        if (GameManager.Instance != null && GameManager.Instance.playerMaxHealth > 0)
        {
            maxLives = GameManager.Instance.playerMaxHealth;
            currentLives = GameManager.Instance.playerCurrentHealth;
        }
        else
        {
            currentLives = maxLives;
            if (GameManager.Instance != null)
                GameManager.Instance.SetPlayerHealth(currentLives, maxLives);
        }
    }

    void Start()
    {
        // 🔹 Cargar posición guardada si existe
        if (PlayerPrefs.HasKey("SpawnX") && PlayerPrefs.HasKey("SpawnY"))
        {
            float x = PlayerPrefs.GetFloat("SpawnX");
            float y = PlayerPrefs.GetFloat("SpawnY");
            transform.position = new Vector2(x, y);
        }

        // 🔹 Actualizar corazones al iniciar
        UIManager.Instance.UpdateHearts(currentLives, maxLives);
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
            lastMoveDir = moveInput.normalized;

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

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, hitLayer);
        foreach (var col in hits)
        {
            var enemy = col.GetComponent<EnemyController>();
            if (enemy != null)
                enemy.TakeDamage(swordDamage);
        }
    }


    private void ResetAttack() => canAttack = true;

    private void OnInteract()
    {
        float interactRadius = 1.0f;
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRadius, LayerMask.GetMask("NPC"));
        if (hit != null)
        {
            var dialog = hit.GetComponent<NPCDialog>();
            // lógica de diálogo si hace falta
        }
    }




    public void TakeDamage(int amount)
    {

        currentLives = Mathf.Max(currentLives - amount, 0);

        // 🔸 Guardar inmediatamente en GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.SetPlayerHealth(currentLives, maxLives);

        // 🔸 Refrescar UI actual
        UIManager.Instance.UpdateHearts(currentLives, maxLives);
              
        animator.SetTrigger("Damage");
        if (currentLives <= 0)
            Die();
    }




    private void Die()
    {
        animator.SetTrigger("Death");
        enabled = false;
        rb.linearVelocity = Vector2.zero;
        GameManager.Instance.TriggerGameOver();
    }


    public void AddLife(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);

        if (GameManager.Instance != null)
            GameManager.Instance.SetPlayerHealth(currentLives, maxLives);

        UIManager.Instance.UpdateHearts(currentLives, maxLives);
    }


    public void Heal(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);

        if (GameManager.Instance != null)
            GameManager.Instance.SetPlayerHealth(currentLives, maxLives);

        UIManager.Instance.UpdateHearts(currentLives, maxLives);
    }


    
    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}








