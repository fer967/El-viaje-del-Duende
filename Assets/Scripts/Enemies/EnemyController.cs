using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 1;
    private int currentHealth;

    [Header("AI")]
    public float detectRadius = 5f;
    public float attackRadius = 0.9f;
    public float moveSpeed = 2f;
    public float attackCooldown = 1.0f;
    public int coinReward = 1;

    [Header("References")]
    public LayerMask playerLayer;
    public Transform attackPoint;

    // components
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private float lastAttackTime = -999f;
    private Vector2 startPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        startPos = transform.position;
    }

    private void Update()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Duende");
            if (p) player = p.transform;
        }

        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectRadius)
        {
            // perseguir
            Vector2 dir = (player.position - transform.position).normalized;
            if (dist > attackRadius)
            {
                rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
                TryAttack();
            }

            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);
        }
        else
        {
            // volver a posición inicial (opcional)
            Vector2 dir = (startPos - (Vector2)transform.position);
            if (dir.magnitude > 0.1f)
            {
                rb.MovePosition(rb.position + dir.normalized * (moveSpeed * 0.5f) * Time.deltaTime);
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);
        }
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        animator.SetTrigger("Attack");

        // daño al player si corresponde
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, 0.5f, playerLayer);
        if (hit)
        {
            var playerScript = hit.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1); // daño al player
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("Damage");
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        animator.SetTrigger("Death");
        // sumar monedas y desactivar al morir
        UIManager.Instance.AddCoins(coinReward);
        GameManager.Instance.EnemyKilled(); // para contar si hay triggers
        Destroy(gameObject, 0.4f); // dejar tiempo para animación
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, 0.5f);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
