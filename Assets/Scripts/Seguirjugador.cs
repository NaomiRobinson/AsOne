using UnityEngine;

public class Seguirjugador : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public float distanciaBusquedaX;
    public float distanciaBusquedaY;
    public LayerMask capaJugador;

    public Transform transformJugador;

    public EstadosMovimiento estadoActual;

    public float velocidad;

    public float distanciaMax;
    public Vector3 puntoA;
    public Vector3 puntoB;
    private Vector3 objetivoPatrulla;

    public Vector3 puntoInicial;

    public bool mirandoDer;


    public enum EstadosMovimiento
    {
        Patrullando,
        Siguiendo,
        Volviendo,
    }

    void Start()
    {
        puntoA = transform.position;
        puntoB = puntoA + new Vector3(5f, 0f, 0f); // Distancia de patrullaje configurable
        objetivoPatrulla = puntoB;
        estadoActual = EstadosMovimiento.Patrullando;
    }


    private void Update()
    {
        switch (estadoActual)
        {
            case EstadosMovimiento.Patrullando:
                EstadoPatrullando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }

        // Detectar jugador en cualquier estado
        Collider2D jugadorCollider = Physics2D.OverlapBox(transform.position, new Vector2(distanciaBusquedaX * 2, distanciaBusquedaY * 2), 0f, capaJugador);
        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
            estadoActual = EstadosMovimiento.Siguiendo;
        }
    }

    private void EstadoPatrullando()
    {
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Mover hacia el objetivo actual de patrullaje
        if (transform.position.x < objetivoPatrulla.x)
            rb2D.linearVelocity = new Vector2(velocidad, rb2D.linearVelocity.y);
        else
            rb2D.linearVelocity = new Vector2(-velocidad, rb2D.linearVelocity.y);

        GirarAObjetivo(objetivoPatrulla);

        // Cambiar de dirección si llega al punto
        if (Vector2.Distance(transform.position, objetivoPatrulla) < 0.1f)
        {
            objetivoPatrulla = (objetivoPatrulla == puntoA) ? puntoB : puntoA;
        }
    }
    private void EstadoSiguiendo()
    {
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (transform.position.x < transformJugador.position.x)
            rb2D.linearVelocity = new Vector2(velocidad, rb2D.linearVelocity.y);
        else
            rb2D.linearVelocity = new Vector2(-velocidad, rb2D.linearVelocity.y);

        GirarAObjetivo(transformJugador.position);

        if (Vector2.Distance(transform.position, transformJugador.position) > distanciaMax)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoVolviendo()
    {
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        Vector3 objetivo = (Vector2.Distance(transform.position, puntoA) < Vector2.Distance(transform.position, puntoB)) ? puntoA : puntoB;

        if (transform.position.x < objetivo.x)
            rb2D.linearVelocity = new Vector2(velocidad, rb2D.linearVelocity.y);
        else
            rb2D.linearVelocity = new Vector2(-velocidad, rb2D.linearVelocity.y);

        GirarAObjetivo(objetivo);

        if (Vector2.Distance(transform.position, objetivo) < 0.1f)
        {
            estadoActual = EstadosMovimiento.Patrullando;
        }
    }

    private void GirarAObjetivo(Vector3 objetivo)
    {
        float diferenciaX = objetivo.x - transform.position.x;

        if (Mathf.Abs(diferenciaX) > 0.05f)
        {
            if (diferenciaX > 0 && !mirandoDer)
                Girar();
            else if (diferenciaX < 0 && mirandoDer)
                Girar();
        }
    }

    private void Girar()
    {
        mirandoDer = !mirandoDer;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(distanciaBusquedaX * 2, distanciaBusquedaY * 2, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(puntoA, 0.2f);
        Gizmos.DrawSphere(puntoB, 0.2f);
    }
}
