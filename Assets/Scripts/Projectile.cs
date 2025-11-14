using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Propiedades del proyectil")]
    public float speed = 6f;
    public int damage = 1;
    public float lifetime = 3f;
    public bool isEnemyProjectile = false;
    public string targetTag = "Enemy";          // o "Player" si es del enemigo

    private Rigidbody2D rb;
    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;

        if (rb != null)
        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if (isEnemyProjectile)
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                if (player != null)
                    player.TakeDamage(damage);
            }
            else
            {
                EnemyController enemy = collision.GetComponent<EnemyController>();
                if (enemy != null)
                    enemy.TakeDamage(damage);

                OgreController ogre = collision.GetComponent<OgreController>();
                if (ogre != null)
                    ogre.TakeDamage(damage);

                // agrego para ver daño en otros
                RangedEnemyStationary enemyStationary = collision.GetComponent<RangedEnemyStationary>();
                if (enemyStationary != null)
                    enemyStationary.TakeDamage(damage);

                WitchController witch = collision.GetComponent<WitchController>();
                if (witch != null)
                    witch.TakeDamage(damage);

            }

            Destroy(gameObject);
        }
        //else if (collision.CompareTag("Wall"))
        //{
        //    // Destruir al chocar con paredes
        //    Destroy(gameObject);
        //}
    }
}










