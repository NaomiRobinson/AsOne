using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;


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

    private TrailRenderer trail;
    private TrailRenderer trailEspejado;
    public bool GravedadInvertida => rb.gravityScale < 0f;

    [SerializeField] private float velocidadX = 5f;
    [SerializeField] private float velocidadMaximaY = 10f;
    [SerializeField] private float cooldownInversion = 0.25f;

    [SerializeField] private ParticleSystem indicadorArriba;
    [SerializeField] private ParticleSystem indicadorAbajo;
    [SerializeField] private GameObject textoModoInvencible;


    private float tiempoUltimaInversion = 0f;
    private bool puedeInvertirJugador = true;
    private bool puedeInvertirEspejado = true;
 

    private bool inputGravedadArriba, inputGravedadAbajo, inputModoInvencible, inputPausar;
    private Vector2 inputMovimiento;
    [HideInInspector] public bool juegoPausado = false;
    [HideInInspector] public bool puedeMoverse = true;
    public bool ModoInvencible { get; private set; } = false;
    private Controles controles;
    [HideInInspector] public enum EsquemaDeControl { Ninguno, WASD, Flechas }
    [HideInInspector] public EsquemaDeControl esquemaActual = EsquemaDeControl.Ninguno;
    private enum IndicadorActivo { Ninguno, Arriba, Abajo }
    private IndicadorActivo indicadorActual = IndicadorActivo.Ninguno;

    public enum Jugador { Izq, Der }

    private bool esperandoSquash = false;

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

        trail = jugadorIzq.GetComponent<TrailRenderer>();
        trailEspejado = jugadorDer.GetComponent<TrailRenderer>();

        rb.gravityScale = 1f;
        rbEspejado.gravityScale = 1f;

        trail.emitting = false;
        trailEspejado.emitting = false;
    }

    void Update()
    {
        DetectarEsquemaControl();

        if ((MenuPausa.Instancia != null && MenuPausa.Instancia.juegoPausado) ||
            (TransicionEscena.Instance != null && TransicionEscena.Instance.TransicionEnCurso))
        {
            DetenerMovimiento();
            return;
        }

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
            if (SoundManager.instancia != null)
                SoundManager.instancia.ReproducirSonido(SoundManager.instancia.cambiar_gravedad_01, 0.8f);

            StartCoroutine(InvertirGravedadCoroutine(inputGravedadArriba));
            tiempoUltimaInversion = Time.time;
        }

        if (inputModoInvencible)
        {
            ModoInvencible = !ModoInvencible;
            if (MenuPausa.Instancia != null)
                MenuPausa.Instancia.ActualizarTextoInvencible();
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
            int flipX = direccion < 0 ? -1 : 1;
            SetFlipX(obj, flipX * direccionVisual);
        }
    }

    void DetenerMovimiento()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        rbEspejado.linearVelocity = new Vector2(0, rbEspejado.linearVelocity.y);
    }

    IEnumerator InvertirGravedadCoroutine(bool invertir)
    {
        yield return new WaitForSeconds(0.1f); 

        float gravedad = invertir ? -1f : 1f;
        int flipY = invertir ? -1 : 1;

        if (puedeInvertirJugador)
        {
            rb.gravityScale = gravedad;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            SetFlipY(jugadorIzq, flipY);
            animatorJugador.ResetTrigger("stretch");
            animatorJugador.SetTrigger("stretch"); // jugador izquierdo
        }
        else
        {              
                SoundManager.instancia.ReproducirSonido(SoundManager.instancia.cambiar_gravedad_03);           
        }


        if (puedeInvertirEspejado)
        {
            rbEspejado.gravityScale = gravedad;
            rbEspejado.linearVelocity = new Vector2(rbEspejado.linearVelocity.x, 0f);
            SetFlipY(jugadorDer, flipY);
            animatorEspejado.ResetTrigger("stretch"); // CORRECTO: usar animatorEspejado
            animatorEspejado.SetTrigger("stretch");   // CORRECTO: jugador derecho
        }
        else
        {
            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.cambiar_gravedad_03);
        }

        esperandoSquash = true;

        // Trail
        trail.emitting = true;
        trailEspejado.emitting = true;
        StartCoroutine(DesactivarTrail(trail, 0.5f));
        StartCoroutine(DesactivarTrail(trailEspejado, 0.5f));
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

    public void HandleCollision(Collision2D collision)
    {
        // Solo colisiones con "Walls"
        if (!collision.gameObject.CompareTag("Walls"))
            return;

        float velocidadCaida = Mathf.Abs(rb.linearVelocity.y); // rb del jugador correspondiente

        bool caidaFuerte = velocidadCaida > 6f; // por ejemplo, si cae más rápido que 5 unidades/s

        foreach (var contacto in collision.contacts)
        {
            Vector2 normal = contacto.normal;
            bool contactoVertical = Vector2.Angle(normal, Vector2.up) < 50f ||
                                    Vector2.Angle(normal, Vector2.down) < 50f;

            if (contactoVertical && (esperandoSquash || caidaFuerte))
            {
                animatorJugador.ResetTrigger("squash");
                animatorEspejado.ResetTrigger("squash");
                animatorJugador.SetTrigger("squash");
                animatorEspejado.SetTrigger("squash");
                esperandoSquash = false;
                break;
            }
        }
    }

    void DetectarEsquemaControl()
    {
        var nuevoEsquema = esquemaActual;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed ||
                Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed)
                nuevoEsquema = EsquemaDeControl.WASD;
            else if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed ||
                     Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                nuevoEsquema = EsquemaDeControl.Flechas;
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftStick.ReadValue().magnitude > 0.2f)
                nuevoEsquema = EsquemaDeControl.WASD;
            else if (Gamepad.current.rightStick.ReadValue().magnitude > 0.2f)
                nuevoEsquema = EsquemaDeControl.Flechas;
        }

        if (nuevoEsquema != esquemaActual)
        {
            esquemaActual = nuevoEsquema;
            Debug.Log("Esquema detectado: " + esquemaActual);
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
/*
        if (!estado && SoundManager.instancia != null)
        {
            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.cambiar_gravedad_03);
        }
*/
    }

    private IEnumerator DesactivarTrail(TrailRenderer trail, float delay)
    {
        yield return new WaitForSeconds(delay);
        trail.emitting = false;
    }




}
