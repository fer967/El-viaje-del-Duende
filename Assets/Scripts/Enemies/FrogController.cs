using UnityEngine;
public class SapoController : MonoBehaviour
{
    public Animator animator;
    public float rangoDeteccion = 2f;        // Distancia m�xima a la que detecta
    public float anguloVision = 60f;         // Campo de visi�n frontal (en grados)
    public Vector2 direccionFrontal = Vector2.down; // Direcci�n en la que mira el sapo (por defecto hacia abajo)

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

        // Producto punto para saber el �ngulo entre el frente del sapo y el jugador
        float dot = Vector2.Dot(direccionFrontal.normalized, direccionAlJugador);
        float angulo = Mathf.Acos(dot) * Mathf.Rad2Deg;

        bool enRango = distancia < rangoDeteccion;
        bool enVision = angulo < anguloVision / 2f;

        animator.SetBool("Attack", enRango && enVision);
    }

#if UNITY_EDITOR
    // Gizmo para ver el rango y �ngulo en el editor
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

