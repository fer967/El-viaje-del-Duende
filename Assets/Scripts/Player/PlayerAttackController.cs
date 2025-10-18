
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Ajustes de ataque")]
    public float attackRange = 0.8f;         // Distancia del golpe
    public int attackDamage = 1;             // Daño al enemigo
    public float attackCooldown = 0.4f;      // Tiempo entre ataques

    [Header("Referencias")]
    public Transform attackPoint;            // Punto desde donde se lanza el golpe
    public LayerMask enemyLayers;            // Capa de enemigos
    public Animator animator;                // Animator del Player

    private bool canAttack = true;           // Control de cooldown

    // 🔹 Este método se ejecuta cuando se activa la acción "Attack" (configurada en PlayerInput)
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Solo atacar cuando la acción se "realiza" (tecla presionada, no soltada)
        if (context.performed && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false;

        // 🔹 Activar animación de ataque
        if (animator != null)
            animator.SetTrigger("Attack");

        // 🔹 Detectar enemigos dentro del rango de ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            WolfmanController wolfman = enemy.GetComponent<WolfmanController>();
            if (wolfman != null)
            {
                wolfman.TakeDamage(attackDamage);
            }
        }

        // 🔹 Reiniciar cooldown
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    // 🔹 Gizmo para visualizar el rango del ataque en la escena
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}




//using UnityEngine;

//public class PlayerAttackController : MonoBehaviour
//{
//    [Header("Ajustes de ataque")]
//    public float attackRange = 0.8f;         // Distancia del golpe
//    public int attackDamage = 1;             // Daño al enemigo
//    public float attackCooldown = 0.4f;      // Tiempo entre ataques

//    [Header("Referencias")]
//    public Transform attackPoint;            // Punto desde donde se lanza el golpe
//    public LayerMask enemyLayers;            // Capa de enemigos
//    public Animator animator;

//    private bool canAttack = true;

//    void Update()
//    {
//        if (Input.GetButtonDown("Fire1") && canAttack)  // Fire1 = click izq o tecla asignada
//        {
//            Attack();
//        }
//    }

//    void Attack()
//    {
//        canAttack = false;

//        // 🔹 Activar animación de ataque
//        animator.SetTrigger("Attack");

//        // 🔹 Detectar enemigos dentro del rango de ataque
//        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

//        foreach (Collider2D enemy in hitEnemies)
//        {
//            WolfmanController wolfman = enemy.GetComponent<WolfmanController>();
//            if (wolfman != null)
//            {
//                wolfman.TakeDamage(attackDamage);
//            }
//        }

//        // 🔹 Reiniciar cooldown
//        Invoke(nameof(ResetAttack), attackCooldown);
//    }

//    void ResetAttack()
//    {
//        canAttack = true;
//    }

//    void OnDrawGizmosSelected()
//    {
//        if (attackPoint == null) return;
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
//    }
//}
