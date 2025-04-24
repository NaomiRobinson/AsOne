using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public GameObject jugador;
    public GameObject jugadorEspejado;
    private Rigidbody2D rb;
    private Rigidbody2D rbEspejado;
    float velocidadMaximaY = 10f;

    [SerializeField] private float velocidadX = 5f;
    [SerializeField] private ParticleSystem particulasIzq;
    [SerializeField] private ParticleSystem particulasDer;

    private bool gravedadInvertida = false;

    private Animator animatorJugador;
    private Animator animatorEspejado;

    void Start()
    {
        rb = jugador.GetComponent<Rigidbody2D>();
        rbEspejado = jugadorEspejado.GetComponent<Rigidbody2D>();

        animatorJugador = jugador.GetComponent<Animator>();
        animatorEspejado = jugadorEspejado.GetComponent<Animator>();

        rb.gravityScale = 1f;
        rbEspejado.gravityScale = 1f;

    }

    void Update()
    {
        float movimientoHorizontal = 0f;

        // Movimiento en X (libre y espejado)
        if (Input.GetKey(KeyCode.A))
        {
            movimientoHorizontal = -1f;
            rb.linearVelocity = new Vector2(movimientoHorizontal * velocidadX, rb.linearVelocity.y);
            rbEspejado.linearVelocity = new Vector2(-movimientoHorizontal * velocidadX, rbEspejado.linearVelocity.y);

            SetFlipX(jugador, 1);
            SetFlipX(jugadorEspejado, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movimientoHorizontal = 1f;
            rb.linearVelocity = new Vector2(movimientoHorizontal * velocidadX, rb.linearVelocity.y);
            rbEspejado.linearVelocity = new Vector2(-movimientoHorizontal * velocidadX, rbEspejado.linearVelocity.y);

            SetFlipX(jugador, -1);
            SetFlipX(jugadorEspejado, -1);
        }
        else
        {
            // Frena en X si no está moviéndose
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            rbEspejado.linearVelocity = new Vector2(0, rbEspejado.linearVelocity.y);
        }

        // Invertir gravedad con W, restaurar con S
        if (Input.GetKeyDown(KeyCode.W))
        {
            particulasIzq.Play();
            particulasDer.Play();
            InvertirGravedad(true);
            SetFlipY(jugador, -1);
            SetFlipY(jugadorEspejado, -1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            particulasIzq.Play();
            particulasDer.Play();
            InvertirGravedad(false);
            SetFlipY(jugador, 1);
            SetFlipY(jugadorEspejado, 1);
        }


        animatorJugador.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        animatorEspejado.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
    }
    void FixedUpdate()
    {
        // Limitar velocidad en Y
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -velocidadMaximaY, velocidadMaximaY));
        rbEspejado.linearVelocity = new Vector2(rbEspejado.linearVelocity.x, Mathf.Clamp(rbEspejado.linearVelocity.y, -velocidadMaximaY, velocidadMaximaY));
    }

    void InvertirGravedad(bool invertir)
    {
        gravedadInvertida = invertir;
        float gravedad = invertir ? -1f : 1f;
        rb.gravityScale = gravedad;
        rbEspejado.gravityScale = gravedad;
    }

    void SetFlipX(GameObject obj, int direccion)
    {
        Vector3 scale = obj.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direccion;
        obj.transform.localScale = scale;

    }

    void SetFlipY(GameObject obj, int direccion)
    {
        Vector3 scale = obj.transform.localScale;
        scale.y = Mathf.Abs(scale.y) * direccion;
        obj.transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contacto in collision.contacts)
        {
            Vector2 normal = contacto.normal;

            if (Vector2.Angle(normal, Vector2.right) < 10f || Vector2.Angle(normal, Vector2.left) < 10f)
            {
                // Paredes laterales: frenar en X
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                rbEspejado.linearVelocity = new Vector2(0, rbEspejado.linearVelocity.y);
            }
            // No hace falta frenar en Y, lo hace la gravedad + colisión
        }
    }


}