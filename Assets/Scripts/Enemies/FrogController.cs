using UnityEngine;
public class SapoController : MonoBehaviour
{
    public Animator animator;
    public float rangoDeteccion = 2f;        
    public float anguloVision = 60f;         
    public Vector2 direccionFrontal = Vector2.down; 

    private Transform jugador;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (jugador == null) return;

        Vector2 direccionAlJugador = (jugador.position - transform.position).normalized;
        float distancia = Vector2.Distance(transform.position, jugador.position);

        float dot = Vector2.Dot(direccionFrontal.normalized, direccionAlJugador);
        float angulo = Mathf.Acos(dot) * Mathf.Rad2Deg;

        bool enRango = distancia < rangoDeteccion;
        bool enVision = angulo < anguloVision / 2f;

        animator.SetBool("Attack", enRango && enVision);
    }

#if UNITY_EDITOR
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);

        Vector3 frente = new Vector3(direccionFrontal.x, direccionFrontal.y, 0);
        Quaternion rot1 = Quaternion.Euler(0, 0, anguloVision / 2f);
        Quaternion rot2 = Quaternion.Euler(0, 0, -anguloVision / 2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + rot1 * frente * rangoDeteccion);
        Gizmos.DrawLine(transform.position, transform.position + rot2 * frente * rangoDeteccion);
    }
#endif
}

