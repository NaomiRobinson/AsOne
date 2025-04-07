using UnityEngine;

public class MovimientoOpcional : MonoBehaviour
{
    public GameObject jugador;
    public GameObject jugadorEspejado;
    private Rigidbody2D rb;
    private Rigidbody2D rbEspejado;

    private Vector2 direccionMovimiento;
    [SerializeField] private float velocidadMovimiento = 5f;

    private bool estaMoviendose = false;
    private bool esperandoColision = false;
    private bool esatColisionando = false;

    private Vector2 direccionBloqueada = Vector2.zero;

    [SerializeField] private float tiempoEsperaColision = 0.5f;
    private float tiempoRestanteColision = 0f;

    private readonly KeyCode[] teclasMovimiento = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
    private readonly Vector2[] direcciones = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Start()
    {
        rb = jugador.GetComponent<Rigidbody2D>();
        rbEspejado = jugadorEspejado.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Bloquea la entrada si está esperando colisiones o está contra una pared
        if (esperandoColision || estaMoviendose || esatColisionando)
        {
            if (esperandoColision)
            {
                tiempoRestanteColision -= Time.deltaTime;
                if (tiempoRestanteColision <= 0f)
                {
                    esperandoColision = false;
                    estaMoviendose = false;
                    direccionBloqueada = Vector2.zero;
                    Debug.Log("Tiempo de espera de colisión ha terminado.");
                }
            }
            return;
        }

        for (int i = 0; i < teclasMovimiento.Length; i++)
        {
            if (Input.GetKeyDown(teclasMovimiento[i]))
            {
                Debug.Log("Tecla presionada: " + teclasMovimiento[i]);
                Moverse(direcciones[i]);
                break;
            }
        }
    }

    void Moverse(Vector2 direccion)
    {
        if (!estaMoviendose && !esatColisionando)
        {
            direccionMovimiento = direccion;
            estaMoviendose = true;

            // Movimiento horizontal igual en ambos jugadores
            if (direccionMovimiento.x != 0)
            {
                rb.linearVelocity = direccionMovimiento * velocidadMovimiento;
                rbEspejado.linearVelocity = -direccionMovimiento * velocidadMovimiento;
            }
            // Movimiento vertical espejado
            else if (direccionMovimiento.y != 0)
            {
                rb.linearVelocity = direccionMovimiento * velocidadMovimiento;
                rbEspejado.linearVelocity = direccionMovimiento * velocidadMovimiento;
            }

            esperandoColision = true;
            tiempoRestanteColision = tiempoEsperaColision;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == jugador || collision.gameObject == jugadorEspejado)
            return;

        Debug.Log("Colisión detectada con una pared.");
        direccionBloqueada = direccionMovimiento; // Bloquea la dirección en la que se estaba moviendo

        DetenerMovimiento();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == jugador || collision.gameObject == jugadorEspejado)
            return;

        Debug.Log("Se alejó de la pared.");
        direccionBloqueada = Vector2.zero;
    }
    void DetenerMovimiento()
    {
        Debug.Log("Deteniendo movimiento.");
        estaMoviendose = false;
        rb.linearVelocity = Vector2.zero;
        rbEspejado.linearVelocity = Vector2.zero;

        esperandoColision = true;
        tiempoRestanteColision = tiempoEsperaColision;
    }}

   