
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Ajustes de ataque")]
    public float attackRange = 0.8f;         
    public int attackDamage = 1;             
    public float attackCooldown = 0.4f;      

    [Header("Referencias")]
    public Transform attackPoint;            
    public LayerMask enemyLayers;            
    public Animator animator;                

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
            EnemyController wolfman = enemy.GetComponent<EnemyController>();
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

    
}





