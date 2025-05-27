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

    [SerializeField] private ParticleSystem indicadorJugador;

    private bool puedeInvertirJugador = true;
    private bool puedeInvertirEspejado = true;

    public bool gravedadInvertida = false;

    private bool estaIndicadorJugador = true;

    public BloquearInvertirGravedad bloqueoJugador;
    public BloquearInvertirGravedad bloqueoEspejado;

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
        indicadorJugador.Play();

    }

    void Update()
    {
        float movimientoHorizontal = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            movimientoHorizontal = -1f;
            MoverHorizontal(movimientoHorizontal);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movimientoHorizontal = 1f;
            MoverHorizontal(movimientoHorizontal);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            rbEspejado.linearVelocity = new Vector2(0, rbEspejado.linearVelocity.y);
        }

        // Actualizar animaciones correctamente
        animatorJugador.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        animatorEspejado.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));

        // Invertir gravedad con W, restaurar con S
        if (Input.GetKeyDown(KeyCode.W))
        {
            CambiarGravedad(true);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CambiarGravedad(false);
        }
    }

    void FixedUpdate()
    {
        // Limitar velocidad en Y
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -velocidadMaximaY, velocidadMaximaY));
        rbEspejado.linearVelocity = new Vector2(rbEspejado.linearVelocity.x, Mathf.Clamp(rbEspejado.linearVelocity.y, -velocidadMaximaY, velocidadMaximaY));
    }

    void MoverHorizontal(float direccion)
    {
        rb.linearVelocity = new Vector2(direccion * velocidadX, rb.linearVelocity.y);
        rbEspejado.linearVelocity = new Vector2(-direccion * velocidadX, rbEspejado.linearVelocity.y);

        int flipX = direccion < 0 ? 1 : -1;
        SetFlipX(jugador, flipX);
        SetFlipX(jugadorEspejado, flipX);

        desactivarIndicador();
    }

    void CambiarGravedad(bool invertir)
    {
        gravedadInvertida = invertir;
        float gravedad = invertir ? -1f : 1f;
        int flipY = invertir ? -1 : 1;

        if (puedeInvertirJugador)
        {
            rb.gravityScale = gravedad;
            SetFlipY(jugador, flipY);
        }
        if (puedeInvertirEspejado)
        {
            rbEspejado.gravityScale = gravedad;
            SetFlipY(jugadorEspejado, flipY);
        }

        if (puedeInvertirJugador || puedeInvertirEspejado)
        {
            particulasIzq.Play();
            particulasDer.Play();
            desactivarIndicador();
            Debug.Log($"[MovimientoJugador] Gravedad {(invertir ? "invertida: ARRIBA" : "restaurada: ABAJO")}");
        }
        else
        {
            Debug.Log("[MovimientoJugador] No se puede cambiar la gravedad: ambos jugadores están bloqueados.");
        }
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

                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                rbEspejado.linearVelocity = new Vector2(0, rbEspejado.linearVelocity.y);
            }

        }
    }

    void desactivarIndicador()
    {

        if (estaIndicadorJugador)
        {
            estaIndicadorJugador = false;
            indicadorJugador.Stop();
        }

    }

    public void PuedeInvertir(BloquearInvertirGravedad quien, bool estado)
    {
        if (quien == bloqueoJugador)
        {
            puedeInvertirJugador = estado;
            Debug.Log($"[MovimientoJugador] bloqueoJugador puedeInvertirJugador = {estado}");
        }
        else if (quien == bloqueoEspejado)
        {
            puedeInvertirEspejado = estado;
            Debug.Log($"[MovimientoJugador] bloqueoEspejado puedeInvertirEspejado = {estado}");
        }
    }


}