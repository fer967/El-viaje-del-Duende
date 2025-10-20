using UnityEngine;

public class WolfmanController : MonoBehaviour
{
    [Header("Configuración de enemigo")]
    public int maxHealth = 3;
    public int damage = 1;
    public int coinReward = 1;      // monedas que otorga al morir
    public float moveSpeed = 2f;
    public float chaseRange = 5f;

    [Header("Ataque")]
    public Transform attackPoint;
    public float attackRange = 0.6f;
    public int attackDamage = 1;
    public LayerMask playerLayer;
    public float attackCooldown = 1f;
    private bool canAttack = true;

    private int currentHealth;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= chaseRange)
        {
            // Perseguir al jugador
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

            if (animator != null)
            {
                animator.SetFloat("MoveX", direction.x);
                animator.SetFloat("MoveY", direction.y);
                animator.SetBool("IsMoving", true);
            }
        }
        else
        {
            if (animator != null)
                animator.SetBool("IsMoving", false);
        }
    }


    void Attack()
    {
        if (!canAttack) return;

        animator.SetTrigger("Attack");
        canAttack = false;

        // Detectar si el Player está dentro del rango de ataque
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            player.GetComponent<PlayerController>()?.TakeDamage(attackDamage);
        }

        Invoke(nameof(ResetAttack), attackCooldown);
    }


    void ResetAttack()
    {
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Death");

        // 🔹 Sumar monedas visuales (y registrar en GameManager si querés)
        if (UIManager.Instance != null)
            UIManager.Instance.AddCoins(coinReward);

        if (GameManager.Instance != null)
            GameManager.Instance.AddCoins(coinReward);

        // Desactivar colisiones y movimiento
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // 🔹 Destruir el enemigo después de un corto delay (para dejar reproducir animación)
        Destroy(gameObject, 0.8f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            var playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
    }
}




//using UnityEngine;

//public class WolfmanController : MonoBehaviour
//{
//    [Header("Componentes")]
//    public Animator animator;
//    public Rigidbody2D rb;
//    public Transform player;

//    [Header("Stats")]
//    public float moveSpeed = 2f;
//    public float detectionRange = 5f;
//    public float attackRange = 1f;
//    public int health = 3;
//    public int coinsDropped = 1;

//    [Header("Estados")]
//    private bool isAttacking = false;
//    private bool isDead = false;

//    private Vector2 moveDir;
//    private Vector2 lastMoveDir;

//    void Start()
//    {
//        if (animator == null) animator = GetComponent<Animator>();
//        if (rb == null) rb = GetComponent<Rigidbody2D>();

//        // Buscar al player si no se asign� manualmente
//        if (player == null)
//            player = GameObject.FindGameObjectWithTag("Player")?.transform;
//    }

//    void Update()
//    {
//        if (isDead || player == null) return;

//        float distance = Vector2.Distance(transform.position, player.position);

//        if (distance <= attackRange)
//        {
//            // Ataca si est� dentro del rango
//            StartAttack();
//        }
//        else if (distance <= detectionRange)
//        {
//            // Persigue al jugador
//            MoveTowardsPlayer();
//        }
//        else
//        {
//            // Idle
//            StopMoving();
//        }

//        UpdateAnimator();
//    }

//    void MoveTowardsPlayer()
//    {
//        if (isAttacking) return;

//        Vector2 direction = (player.position - transform.position).normalized;
//        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
//        moveDir = direction;
//        lastMoveDir = direction;
//    }

//    void StopMoving()
//    {
//        moveDir = Vector2.zero;
//    }

//    void StartAttack()
//    {
//        if (isAttacking) return;
//        isAttacking = true;
//        moveDir = Vector2.zero;
//        animator.SetBool("isAttacking", true);
//        Invoke(nameof(EndAttack), 0.8f); // duraci�n del ataque
//    }

//    void EndAttack()
//    {
//        isAttacking = false;
//        animator.SetBool("isAttacking", false);
//    }

//    void UpdateAnimator()
//    {
//        animator.SetFloat("MoveX", moveDir.x);
//        animator.SetFloat("MoveY", moveDir.y);
//        animator.SetFloat("speed", moveDir.sqrMagnitude);
//    }

//    public void TakeDamage(int damage)
//    {
//        if (isDead) return;

//        health -= damage;

//        if (health <= 0)
//        {
//            Die();
//        }
//    }

//    void Die()
//    {
//        isDead = true;
//        rb.linearVelocity = Vector2.zero;
//        animator.Play("Death"); // si ten�s animaci�n de muerte
//        // Pod�s agregar delay o destruir luego
//        Destroy(gameObject, 1.2f);

//        // Sumar monedas al Player (si ten�s un GameManager)
//        GameManager.Instance.AddCoins(coinsDropped);
//    }

//    private void OnCollisionStay2D(Collision2D collision)
//    {
//        if (!isAttacking || isDead) return;

//        if (collision.gameObject.CompareTag("Player"))
//        {
//            PlayerHealthController playerHealth = collision.gameObject.GetComponent<PlayerHealthController>();
//            if (playerHealth != null)
//            {
//                playerHealth.TakeDamage(1); // Resta 1 coraz�n
//            }
//        }
//    }

//}
