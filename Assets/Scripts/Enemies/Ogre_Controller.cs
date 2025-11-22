using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class OgreController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 6;
    private int currentHealth;

    [Header("Movimiento / Patrulla")]
    public float moveSpeed = 1.5f;
    public Transform pointA;
    public Transform pointB;
    private Transform currentTarget;

    [Header("Detección y ataque")]
    public float detectRadius = 5f;
    public float attackRadius = 1.2f;
    public float attackCooldown = 1.0f;
    public LayerMask playerLayer;
    public Transform attackPoint;
                  
    [Header("Drop al morir")]
    public GameObject bowDropPrefab;   
    public GameObject arrowDropPrefab; 

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private float lastAttackTime = -999f;
    private Vector2 currentMovement = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        if (pointA != null && pointB != null)
            currentTarget = pointB; 
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
            Patrol();
        }
                   
        if (attackPoint != null)
        {
            Vector2 offset = Vector2.zero;
            Vector2 facingDir = new Vector2(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
            if (facingDir.sqrMagnitude > 0.01f)
            {
                offset = facingDir.normalized * dist;
                attackPoint.localPosition = offset;
            }
        }

    }


    private void FixedUpdate()
    {
        if (currentMovement != Vector2.zero)
        {
            rb.MovePosition(rb.position + currentMovement * Time.fixedDeltaTime);
        }
    }

    private void Patrol()
    {
        if (pointA == null || pointB == null) return;
        Vector2 dir = (currentTarget.position - transform.position);
        if (dir.magnitude < 0.1f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
        currentMovement = dir.normalized * moveSpeed;
        animator.SetBool("IsMoving", true);
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);
    }


    private void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, 1f, playerLayer);
        if (hit)
        {
            var playerScript = hit.GetComponent<PlayerController>();
            if (playerScript != null)
                playerScript.TakeDamage(1);
        }
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("Damage");
        if (currentHealth <= 0)
            Die();
    }


    private void Die()
    {
        animator.SetTrigger("Death");
        if (bowDropPrefab != null)
            Instantiate(bowDropPrefab, transform.position, Quaternion.identity);
        if (arrowDropPrefab != null)
            Instantiate(arrowDropPrefab, transform.position + new Vector3(0.5f, 0f, 0f), Quaternion.identity);
        Destroy(gameObject, 0.6f);
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, 1f);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
