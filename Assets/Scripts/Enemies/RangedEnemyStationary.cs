using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RangedEnemyStationary : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 4;
    private int currentHealth;

    [Header("Detección y ataque")]
    public float detectRadius = 6f;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    public Transform shootPoint;
    public GameObject projectilePrefab; // Flecha o bola de fuego
    public bool isEnemyProjectile = true;

    private Transform player;
    private Animator animator;
    private float lastAttackTime = -999f;
    private Vector2 lastDir = Vector2.down; // Para saber hacia dónde mirar

    private void Awake()
    {
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
            lastDir = dir;

            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);

            TryAttack(dir);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void TryAttack(Vector2 dir)
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        animator.SetTrigger("Attack");

        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Projectile p = proj.GetComponent<Projectile>();

            if (p != null)
            {
                p.isEnemyProjectile = isEnemyProjectile;
                p.targetTag = "Player";
                p.Launch(dir);
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
        Destroy(gameObject, 0.6f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        if (shootPoint)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(shootPoint.position, 0.2f);
        }
    }
}
