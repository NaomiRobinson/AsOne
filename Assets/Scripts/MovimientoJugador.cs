using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    public static MovimientoJugador Instancia { get; private set; }

    public GameObject jugadorIzq;
    public GameObject jugadorDer;
    public GameObject PanelDePausa;

    private Rigidbody2D rb;
    private Rigidbody2D rbEspejado;
    private Animator animatorJugador;
    private Animator animatorEspejado;

    public bool GravedadInvertida => rb.gravityScale < 0f;

    [SerializeField] private float velocidadX = 5f;
    [SerializeField] private float velocidadMaximaY = 10f;
    [SerializeField] private float cooldownInversion = 0.25f;
    [SerializeField] private ParticleSystem particulasArriba;
    [SerializeField] private ParticleSystem particulasDer;
    [SerializeField] private ParticleSystem indicadorArriba;
    [SerializeField] private ParticleSystem indicadorAbajo;


    private float tiempoUltimaInversion = 0f;
    private bool puedeInvertirJugador = true;
    private bool puedeInvertirEspejado = true;
    private bool inputGravedadArriba, inputGravedadAbajo, inputModoInvencible, inputPausar;
    private Vector2 inputMovimiento;
    [HideInInspector] public bool juegoPausado = false;
    [HideInInspector] public bool puedeMoverse = true;
    public bool modoInvencible { get; private set; } = false;

    private Controles controles;
    private enum EsquemaDeControl { Ninguno, WASD, Flechas }
    private EsquemaDeControl esquemaActual = EsquemaDeControl.Ninguno;
    private enum IndicadorActivo { Ninguno, Arriba, Abajo }
    private IndicadorActivo indicadorActual = IndicadorActivo.Ninguno;

    public enum Jugador { Izq, Der }

    void Awake()
    {
        Instancia = this;
        controles = new Controles();

        controles.Jugador.Moverse.performed += ctx => inputMovimiento = ctx.ReadValue<Vector2>();
        controles.Jugador.Moverse.canceled += ctx => inputMovimiento = Vector2.zero;
        controles.Jugador.GravedadArriba.performed += _ => inputGravedadArriba = true;
        controles.Jugador.GravedadAbajo.performed += _ => inputGravedadAbajo = true;
        controles.Jugador.ModoInvencible.performed += _ => inputModoInvencible = true;
        controles.Jugador.Pausar.performed += _ => MenuPausa.Instancia.inputPausar();
    }

    void OnEnable() => controles.Enable();
    void OnDisable() => controles.Disable();

    void Start()
    {
        controles.UI.Disable();
        controles.Jugador.Enable();
        rb = jugadorIzq.GetComponent<Rigidbody2D>();
        rbEspejado = jugadorDer.GetComponent<Rigidbody2D>();
        animatorJugador = jugadorIzq.GetComponent<Animator>();
        animatorEspejado = jugadorDer.GetComponent<Animator>();

        rb.gravityScale = 1f;
        rbEspejado.gravityScale = 1f;

        LogControlesActivos();
    }

    void Update()
    {
        DetectarEsquemaControl();

        if (MenuPausa.Instancia.juegoPausado) return;
        if (!puedeMoverse) return;

        float movimiento = inputMovimiento.x;

        if (movimiento != 0)
        {
            if (esquemaActual == EsquemaDeControl.WASD)
            {
                MoverPersonaje(jugadorIzq, rb, movimiento, 1);
                MoverPersonaje(jugadorDer, rbEspejado, -movimiento, -1);
            }
            else if (esquemaActual == EsquemaDeControl.Flechas)
            {
                MoverPersonaje(jugadorIzq, rb, -movimiento, 1);
                MoverPersonaje(jugadorDer, rbEspejado, movimiento, -1);
            }
        }
        else
        {
            DetenerMovimiento();
        }

        animatorJugador.SetFloat("Horizontal", Mathf.Abs(movimiento));
        animatorEspejado.SetFloat("Horizontal", Mathf.Abs(movimiento));

        if ((inputGravedadArriba || inputGravedadAbajo) && Time.time - tiempoUltimaInversion > cooldownInversion)
        {
            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.cambiar_gravedad_01);
            CambiarGravedad(inputGravedadArriba);
            tiempoUltimaInversion = Time.time;
        }

        if (inputModoInvencible)
        {
            modoInvencible = !modoInvencible;
            Debug.Log("Modo invencible: " + modoInvencible);
        }

        inputGravedadArriba = inputGravedadAbajo = inputModoInvencible = false;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -velocidadMaximaY, velocidadMaximaY));
        rbEspejado.linearVelocity = new Vector2(rbEspejado.linearVelocity.x, Mathf.Clamp(rbEspejado.linearVelocity.y, -velocidadMaximaY, velocidadMaximaY));
    }

    void MoverPersonaje(GameObject obj, Rigidbody2D rb, float direccion, int direccionVisual)
    {
        rb.linearVelocity = new Vector2(direccion * velocidadX, rb.linearVelocity.y);

        if (direccion != 0)
        {
            int flipX = direccion < 0 ? 1 : -1;
            SetFlipX(obj, flipX * direccionVisual);
        }
    }

    void DetenerMovimiento()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        rbEspejado.linearVelocity = new Vector2(0, rbEspejado.linearVelocity.y);
    }

    void CambiarGravedad(bool invertir)
    {
        float gravedad = invertir ? -1f : 1f;
        int flipY = invertir ? -1 : 1;

        if (puedeInvertirJugador)
        {
            rb.gravityScale = gravedad;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
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
            particulasArriba.Play();
            particulasDer.Play();
            Debug.Log($"Gravedad {(invertir ? "invertida" : "normal")}");
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
        foreach (var contacto in collision.contacts)
        {
            Vector2 normal = contacto.normal;
            if (Vector2.Angle(normal, Vector2.right) < 10f || Vector2.Angle(normal, Vector2.left) < 10f)
            {
                DetenerMovimiento();
            }
        }
    }

    void DetectarEsquemaControl()
    {
        EsquemaDeControl esquemaDetectado = EsquemaDeControl.Ninguno;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed)
            esquemaDetectado = EsquemaDeControl.WASD;
        else if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            esquemaDetectado = EsquemaDeControl.Flechas;

        if (esquemaDetectado != EsquemaDeControl.Ninguno && esquemaDetectado != esquemaActual)
        {
            esquemaActual = esquemaDetectado;
            ActualizarIndicadorVisual();
        }
    }

    void ActualizarIndicadorVisual()
    {
        if (indicadorArriba == null || indicadorAbajo == null) return;

        if (esquemaActual == EsquemaDeControl.WASD && (!indicadorArriba.isPlaying || indicadorActual != IndicadorActivo.Arriba))
        {
            indicadorAbajo.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            indicadorArriba.Play();
            indicadorActual = IndicadorActivo.Arriba;
        }
        else if (esquemaActual == EsquemaDeControl.Flechas && (!indicadorAbajo.isPlaying || indicadorActual != IndicadorActivo.Abajo))
        {
            indicadorArriba.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            indicadorAbajo.Play();
            indicadorActual = IndicadorActivo.Abajo;
        }
    }

    public void PuedeInvertir(Jugador jugador, bool estado)
    {
        if (jugador == Jugador.Izq)
            puedeInvertirJugador = estado;
        else if (jugador == Jugador.Der)
            puedeInvertirEspejado = estado;
    }



    private void LogControlesActivos()
    {
        foreach (var map in controles.asset.actionMaps)
        {
            Debug.Log($"{map.name} está {(map.enabled ? "HABILITADO" : "DESHABILITADO")}");
        }
    }


}
