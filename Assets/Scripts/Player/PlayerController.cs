using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [Header("Ataque con espada")]
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask hitLayer;
    public int swordDamage = 1;
    public float attackCooldown = 0.4f;
    private bool canAttack = true;

    [Header("Habilidad Puño Titánico")]
    public Transform punchPoint;
    public float punchRange = 0.7f;
    public int punchDamage = 3;
    public float punchCooldown = 0.6f;
    private bool canPunch = true;
    private bool hasPunchAbility = false;

    [Header("Stats")]
    public int maxLives = 4;
    private int currentLives;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controls = new PlayerControls();

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
        if (PlayerPrefs.HasKey("SpawnX") && PlayerPrefs.HasKey("SpawnY"))
        {
            float x = PlayerPrefs.GetFloat("SpawnX");
            float y = PlayerPrefs.GetFloat("SpawnY");
            transform.position = new Vector2(x, y);
        }

        UIManager.Instance.UpdateHearts(currentLives, maxLives);

        if (SceneManager.GetActiveScene().name == "Bosque6" && PlayerPrefs.GetInt("HasPunchAbility", 0) == 1)
        {
            hasPunchAbility = true;

            // Mostrar mensaje si todavía no se mostró
            if (GameManager.Instance != null && GameManager.Instance.hasTitanPunch && !GameManager.Instance.punchMessageShown)
            {
                GameManager.Instance.punchMessageShown = true;
                if (UIManager.Instance != null)
                    UIManager.Instance.ShowMessage("¡Has obtenido la habilidad Puño Titánico!");
            }
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
            lastMoveDir = moveInput.normalized;

        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetBool("IsMoving", moveInput.sqrMagnitude > 0.01f);
        animator.SetFloat("LastMoveX", lastMoveDir.x);
        animator.SetFloat("LastMoveY", lastMoveDir.y);

        // Actualizar posición del punto de ataque y puño
        if (attackPoint != null)
            attackPoint.localPosition = lastMoveDir.normalized * 0.8f;

        if (punchPoint != null)
            punchPoint.localPosition = lastMoveDir.normalized * 0.8f;
    }

    // ⚔️ Ataque con espada (default)
    private void OnAttack()
    {
        // Si tiene habilidad Puño y estamos en Bosque6, usa el golpe en vez de la espada
        if (hasPunchAbility && SceneManager.GetActiveScene().name == "Bosque6")
        {
            OnPunchAttack();
            return;
        }

        if (!canAttack) return;

        animator.SetTrigger("Attack");
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

    // 👊 Ataque con Puño Titánico
    private void OnPunchAttack()
    {
        if (!canPunch) return;

        animator.SetTrigger("Punch");
        canPunch = false;
        Invoke(nameof(ResetPunch), punchCooldown);

        Collider2D[] hits = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, hitLayer);
        foreach (var col in hits)
        {
            // Golpea al OgreController
            var ogre = col.GetComponent<OgreController>();
            if (ogre != null)
                ogre.TakeDamage(punchDamage);
        }
    }

    private void ResetPunch() => canPunch = true;

    public void UnlockPunchAbility()
    {
        hasPunchAbility = true;
        PlayerPrefs.SetInt("HasPunchAbility", 1);
        PlayerPrefs.Save();
        Debug.Log("✅ Habilidad Puño Titánico desbloqueada!");
    }


    
    private void OnInteract()
    {
        float interactRadius = 1.0f;
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRadius, LayerMask.GetMask("NPC"));
        if (hit != null)
        {
            var dialog = hit.GetComponent<NPCDialog>();
        }

    }

    public void TakeDamage(int amount)
    {
        currentLives = Mathf.Max(currentLives - amount, 0);

        if (GameManager.Instance != null)
            GameManager.Instance.SetPlayerHealth(currentLives, maxLives);

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

        if (punchPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(punchPoint.position, punchRange);
        }
    }
}














