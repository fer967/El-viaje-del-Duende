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
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectRadius)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            if (dist > attackRadius)
            {
                currentMovement = dir * moveSpeed;
                animator.SetBool("IsMoving", true);
            }
            else
            {
                currentMovement = Vector2.zero;
                animator.SetBool("IsMoving", false);
                TryAttack();
            }

            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);
        }
        else
        {
            Vector2 dir = (startPos - (Vector2)transform.position);
            if (dir.magnitude > 0.1f)
            {
                currentMovement = dir.normalized * (moveSpeed * 0.5f);
                animator.SetBool("IsMoving", true);
            }
            else
            {
                currentMovement = Vector2.zero;
                animator.SetBool("IsMoving", false);
            }
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);
        }
    }


    private Vector2 currentMovement = Vector2.zero;


    private void FixedUpdate()
    {
        if (currentMovement != Vector2.zero)
        {
            rb.MovePosition(rb.position + currentMovement * Time.fixedDeltaTime);
        }
    }
     

    private void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        animator.SetTrigger("Attack");

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, 1f, playerLayer);
        if (hit)
        {
            Debug.Log("✅ Golpeó al jugador!");
            var playerScript = hit.GetComponent<PlayerController>();
            if (playerScript != null)
                playerScript.TakeDamage(1);
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
        UIManager.Instance.AddCoins(coinReward);
        GameManager.Instance.EnemyKilled(); 
        Destroy(gameObject, 0.4f); 
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, 0.5f);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
