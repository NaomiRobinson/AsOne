using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    public GameObject jugadorIzq;
    public GameObject jugadorDer;
    private Rigidbody2D rb;
    private Rigidbody2D rbEspejado;
    float velocidadMaximaY = 10f;

    private float tiempoUltimaInversion = 0f;


    public static MovimientoJugador Instancia { get; private set; }

    /// <summary>
    private bool juegoPausado = false;
    public GameObject PanelDePausa;

    /// </summary>



    [SerializeField] private float velocidadX = 5f;
    [SerializeField] private ParticleSystem particulasIzq;
    [SerializeField] private ParticleSystem particulasDer;

    // [SerializeField] private ParticleSystem indicadorJugador;

    [SerializeField] private float cooldownInversion = 0.25f;

    private bool puedeInvertirJugador = true;
    private bool puedeInvertirEspejado = true;

    private bool estaIndicadorJugador = true;

    public bool puedeMoverse = true;

    //variable de cheatmode
    public bool modoInvencible { get; private set; } = false;

    public enum Jugador { Izq, Der }

    private Animator animatorJugador;
    private Animator animatorEspejado;

    private Controles controles;
    private Vector2 inputMovimiento;
    private bool inputGravedadArriba;
    private bool inputGravedadAbajo;
    private bool inputModoInvencible;

    void Awake()
    {
        Instancia = this;
        controles = new Controles();

        controles.Jugador.Moverse.performed += ctx => inputMovimiento = ctx.ReadValue<Vector2>();
        controles.Jugador.Moverse.canceled += ctx => inputMovimiento = Vector2.zero;

        controles.Jugador.GravedadArriba.performed += ctx => inputGravedadArriba = true;
        controles.Jugador.GravedadAbajo.performed += ctx => inputGravedadAbajo = true;
        controles.Jugador.ModoInvencible.performed += ctx => inputModoInvencible = true;
    }

    void OnEnable() => controles.Enable();
    void OnDisable() => controles.Disable();
    void Start()
    {
        rb = jugadorIzq.GetComponent<Rigidbody2D>();
        rbEspejado = jugadorDer.GetComponent<Rigidbody2D>();

        animatorJugador = jugadorIzq.GetComponent<Animator>();
        animatorEspejado = jugadorDer.GetComponent<Animator>();

        rb.gravityScale = 1f;
        rbEspejado.gravityScale = 1f;
        //  indicadorJugador.Play();

    }

    void Update()
    {
        if (!puedeMoverse) return;

        float movimientoHorizontal = inputMovimiento.x;

        if (movimientoHorizontal != 0)
        {
            MoverHorizontal(movimientoHorizontal);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            rbEspejado.linearVelocity = new Vector2(0, rbEspejado.linearVelocity.y);
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            juegoPausado = true;
            Time.timeScale = 0f; //pausa el juego
            PanelDePausa.SetActive(true); //muestra el panel de pausa
        }

        // Actualizar animaciones correctamente


        animatorJugador.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        animatorEspejado.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));

        // Gravedad
        if ((inputGravedadArriba || inputGravedadAbajo) && Time.time - tiempoUltimaInversion > cooldownInversion)
        {
            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.cambiar_gravedad_01);
            CambiarGravedad(inputGravedadArriba);
            tiempoUltimaInversion = Time.time;
        }

        inputGravedadArriba = false;
        inputGravedadAbajo = false;

        // Modo Invencible
        if (inputModoInvencible)
        {
            modoInvencible = !modoInvencible;
            Debug.Log("modo invencible: " + modoInvencible);
            inputModoInvencible = false;
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
        SetFlipX(jugadorIzq, flipX);
        SetFlipX(jugadorDer, flipX);

        //desactivarIndicador();
    }

    void CambiarGravedad(bool invertir)
    {
        if (!puedeInvertirJugador && !puedeInvertirEspejado)
        {
            Debug.Log("[MovimientoJugador] ¡Gravedad bloqueada! No se puede invertir.");
            return;
        }
        float gravedad = invertir ? -1f : 1f;
        int flipY = invertir ? -1 : 1;

        if (puedeInvertirJugador)
        {
            rb.gravityScale = gravedad;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            SetFlipY(jugadorIzq, flipY);
        }
        if (puedeInvertirEspejado)
        {
            rbEspejado.gravityScale = gravedad;
            rbEspejado.linearVelocity = new Vector2(rbEspejado.linearVelocity.x, 0f);
            SetFlipY(jugadorDer, flipY);
        }

        if (puedeInvertirJugador || puedeInvertirEspejado)
        {
            particulasIzq.Play();
            particulasDer.Play();
            // desactivarIndicador();
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

    // void desactivarIndicador()
    // {

    //     if (estaIndicadorJugador)
    //     {
    //         estaIndicadorJugador = false;
    //         indicadorJugador.Stop();
    //     }

    // }

    public void PuedeInvertir(Jugador jugador, bool estado)
    {
        if (jugador == Jugador.Izq)
        {
            puedeInvertirJugador = estado;
            Debug.Log($"[MovimientoJugador] puedeInvertirJugador = {estado}");
        }
        else if (jugador == Jugador.Der)
        {
            puedeInvertirEspejado = estado;
            Debug.Log($"[MovimientoJugador] puedeInvertirEspejado = {estado}");
        }
    }

    public bool GravedadInvertida
    {
        get { return rb.gravityScale < 0f; }
    }


}