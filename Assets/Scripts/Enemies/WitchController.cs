using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class WitchController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Movimiento / Patrulla")]
    public float moveSpeed = 1.8f;
    public Transform[] patrolPoints; 
    private int currentPointIndex = 0;

    [Header("Detección y ataque")]
    public float detectRadius = 6f;
    public float meleeRadius = 1.2f;
    public float rangeAttackRadius = 4.5f;
    public float attackCooldown = 1.5f;
    public LayerMask playerLayer;
    public Transform firePoint;
    public GameObject fireballPrefab;

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
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);

            if (dist <= meleeRadius)
            {
                currentMovement = Vector2.zero;
                animator.SetBool("isMoving", false);
                TryAttackMelee();
            }
            else if (dist <= rangeAttackRadius)
            {
                currentMovement = Vector2.zero;
                animator.SetBool("isMoving", false);
                TryAttackRanged(dir);
            }
            else
            {
                currentMovement = dir * moveSpeed;
                animator.SetBool("isMoving", true);
            }
        }
        else
        {
            Patrol();
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
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPointIndex];
        Vector2 dir = (target.position - transform.position);
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);

        if (dir.magnitude < 0.7f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }

        currentMovement = dir.normalized * moveSpeed;
        animator.SetBool("isMoving", true);
    }


    
    public void WitchPatrolSound()
    {
        AudioManager.instance.PlayWitchLaugh();
    }

    private void TryAttackMelee()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack_Melee");

        Collider2D hit = Physics2D.OverlapCircle(transform.position, meleeRadius, playerLayer);
        if (hit)
        {
            var playerScript = hit.GetComponent<PlayerController>();
            if (playerScript != null)
                playerScript.TakeDamage(1);
        }
    }


    private void TryAttackRanged(Vector2 dir)
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack_Distance");

        if (fireballPrefab != null && firePoint != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            Projectile proj = fireball.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.isEnemyProjectile = true;
                proj.targetTag = "Player";
                proj.Launch(dir);
            }
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
        Invoke(nameof(TriggerVictory), 1.5f);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 2f);
    }

    private void TriggerVictory()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowVictory("¡Felicidades! Has derrotado a la Bruja");
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayVictoryMusic();
        }
    }


    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, rangeAttackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
