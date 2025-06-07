using System;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public GameObject jugadorIzq;
    public GameObject jugadorDer;
    private Rigidbody2D rb;
    private Rigidbody2D rbEspejado;
    float velocidadMaximaY = 10f;

    [SerializeField] private float velocidadX = 5f;
    [SerializeField] private ParticleSystem particulasIzq;
    [SerializeField] private ParticleSystem particulasDer;

    [SerializeField] private ParticleSystem indicadorJugador;

    private bool puedeInvertirJugador = true;
    private bool puedeInvertirEspejado = true;

    private bool estaIndicadorJugador = true;

    //varaible de cheatmode
    public bool modoInvencible { get; private set; } = false;

    public enum Jugador { Izq, Der }

    private Animator animatorJugador;
    private Animator animatorEspejado;


    void Start()
    {
        rb = jugadorIzq.GetComponent<Rigidbody2D>();
        rbEspejado = jugadorDer.GetComponent<Rigidbody2D>();

        animatorJugador = jugadorIzq.GetComponent<Animator>();
        animatorEspejado = jugadorDer.GetComponent<Animator>();

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

        //CHEAT MODE
        if (Input.GetKeyDown(KeyCode.I))
        {
            modoInvencible = !modoInvencible;
            Debug.Log("modo invencible: " + modoInvencible);
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

        desactivarIndicador();
    }

    void CambiarGravedad(bool invertir)
    {

        float gravedad = invertir ? -1f : 1f;
        int flipY = invertir ? -1 : 1;

        if (puedeInvertirJugador)
        {
            rb.gravityScale = gravedad;
            SetFlipY(jugadorIzq, flipY);
        }
        if (puedeInvertirEspejado)
        {
            rbEspejado.gravityScale = gravedad;
            SetFlipY(jugadorDer, flipY);
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


}