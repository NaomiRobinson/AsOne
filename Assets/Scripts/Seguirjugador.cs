using UnityEngine;

public class Seguirjugador : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public float radioBusqueda;
    public LayerMask capaJugador;

    public Transform transformJugador;

    public EstadosMovimiento estadoActual;

    public float velocidad;

    public float distanciaMax;

    public Vector3 puntoInicial;

    public bool mirandoDer;


    public enum EstadosMovimiento
    {
        Esperando,
        Siguiendo,
        Volviendo,
    }

    void Start()
    {
        puntoInicial = transform.position;
    }


    private void Update()
    {
        switch (estadoActual)
        {
            case EstadosMovimiento.Esperando:
                EstadoEsperando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }

    }

    private void EstadoEsperando()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capaJugador);

        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
            estadoActual = EstadosMovimiento.Siguiendo;
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

        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMax ||
            Vector2.Distance(transform.position, transformJugador.position) > distanciaMax)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoVolviendo()
    {
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (transform.position.x < puntoInicial.x)
            rb2D.linearVelocity = new Vector2(velocidad, rb2D.linearVelocity.y);
        else
            rb2D.linearVelocity = new Vector2(-velocidad, rb2D.linearVelocity.y);

        GirarAObjetivo(puntoInicial);

        if (Mathf.Abs(transform.position.x - puntoInicial.x) < 0.1f)
        {
            rb2D.linearVelocity = Vector2.zero;
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            estadoActual = EstadosMovimiento.Esperando;
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
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMax);
    }
}
