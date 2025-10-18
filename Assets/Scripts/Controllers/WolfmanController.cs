using UnityEngine;

public class WolfmanController : MonoBehaviour
{
    [Header("Componentes")]
    public Animator animator;
    public Rigidbody2D rb;
    public Transform player;

    [Header("Stats")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public int health = 3;
    public int coinsDropped = 1;

    [Header("Estados")]
    private bool isAttacking = false;
    private bool isDead = false;

    private Vector2 moveDir;
    private Vector2 lastMoveDir;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Buscar al player si no se asign� manualmente
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Ataca si est� dentro del rango
            StartAttack();
        }
        else if (distance <= detectionRange)
        {
            // Persigue al jugador
            MoveTowardsPlayer();
        }
        else
        {
            // Idle
            StopMoving();
        }

        UpdateAnimator();
    }

    void MoveTowardsPlayer()
    {
        if (isAttacking) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        moveDir = direction;
        lastMoveDir = direction;
    }

    void StopMoving()
    {
        moveDir = Vector2.zero;
    }

    void StartAttack()
    {
        if (isAttacking) return;
        isAttacking = true;
        moveDir = Vector2.zero;
        animator.SetBool("isAttacking", true);
        Invoke(nameof(EndAttack), 0.8f); // duraci�n del ataque
    }

    void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    void UpdateAnimator()
    {
        animator.SetFloat("MoveX", moveDir.x);
        animator.SetFloat("MoveY", moveDir.y);
        animator.SetFloat("speed", moveDir.sqrMagnitude);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.Play("Death"); // si ten�s animaci�n de muerte
        // Pod�s agregar delay o destruir luego
        Destroy(gameObject, 1.2f);

        // Sumar monedas al Player (si ten�s un GameManager)
        GameManager.Instance.AddCoins(coinsDropped);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isAttacking || isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController playerHealth = collision.gameObject.GetComponent<PlayerHealthController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Resta 1 coraz�n
            }
        }
    }

}
