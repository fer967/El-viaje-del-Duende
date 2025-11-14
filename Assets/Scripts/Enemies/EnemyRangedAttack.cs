using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [Header("Ataque a distancia")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float attackRange = 6f;
    public float attackCooldown = 2f;
    public int projectileDamage = 1;
    public float projectileSpeed = 5f;

    private Animator animator;
    private Transform player;
    private bool canShoot = true;
    private Vector2 shootDir = Vector2.right;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= attackRange)
        {
            shootDir = direction.normalized;
            UpdateLookDirection(shootDir);

            if (canShoot)
                Shoot();
        }
    }


    private void Shoot()
    {
        canShoot = false;
        Invoke(nameof(ResetShoot), attackCooldown);

        animator.SetTrigger("Attack");

        if (projectilePrefab == null || shootPoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.isEnemyProjectile = true;
            p.targetTag = "Player";
            p.damage = projectileDamage;
            p.speed = projectileSpeed;
            p.Launch(shootDir);
        }
    }

    private void ResetShoot() => canShoot = true;

    
    private void UpdateLookDirection(Vector2 dir)
    {
        
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                shootDir = Vector2.right;
            }
            else
            {
                shootDir = Vector2.left;
            }
        }
        else
        {
            if (dir.y > 0)
            {
                shootDir = Vector2.up;
            }
            else
            {
                shootDir = Vector2.down;
            }
        }

        animator.SetFloat("MoveX", shootDir.x);
        animator.SetFloat("MoveY", shootDir.y);

        if (shootPoint != null)
        {
            float offset = 0.5f; 
            shootPoint.localPosition = shootDir * offset;
        }
    }
}





