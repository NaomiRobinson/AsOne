using UnityEngine;

public class EnemigoPatrulla : MonoBehaviour
{
    public float velocidad = 2f;
    public Transform[] puntosMovimiento;
    public float distanciaMinima = 0.1f;

    public bool mirandoDer = true;

    public LayerMask capaJugador;
    private Vector2 centroDeteccion;
    private float distanciaBusquedaX;
    public float distanciaBusquedaY = 1f;

    private Transform transformJugador;
    private int siguientePunto = 0;

    private enum Estado { Patrullando, Siguiendo, Quieto }
    private Estado estadoActual;

    private void Start()
    {
        if (puntosMovimiento.Length >= 2)
        {
            centroDeteccion = (puntosMovimiento[0].position + puntosMovimiento[1].position) / 2f;
            distanciaBusquedaX = Mathf.Abs(puntosMovimiento[1].position.x - puntosMovimiento[0].position.x) / 2f;
        }
        estadoActual = Estado.Patrullando;
    }

    private void Update()
    {
        switch (estadoActual)
        {
            case Estado.Patrullando:
                Patrullar();
                BuscarJugador();
                break;

            case Estado.Siguiendo:
                SeguirJugador();
                BuscarJugador();
                break;


            case Estado.Quieto:

                break;
        }
    }

    private void Patrullar()
    {
        Transform objetivo = puntosMovimiento[siguientePunto];
        transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad * Time.deltaTime);

        GirarAObjetivo(objetivo.position);

        if (Vector2.Distance(transform.position, objetivo.position) < distanciaMinima)
        {
            Debug.Log("Cambio de punto");
            siguientePunto = (siguientePunto + 1) % puntosMovimiento.Length;
        }
    }


    private void BuscarJugador()
    {
        Vector2 tamaño = new Vector2(distanciaBusquedaX * 2f, distanciaBusquedaY * 2f);
        Collider2D jugadorDetectado = Physics2D.OverlapBox(centroDeteccion, tamaño, 0f, capaJugador);

        if (jugadorDetectado != null)
        {
            transformJugador = jugadorDetectado.transform;
            estadoActual = Estado.Siguiendo;
        }
        else
        {
            if (estadoActual == Estado.Siguiendo)
            {
                estadoActual = Estado.Patrullando;
                transformJugador = null;


                Transform objetivo = puntosMovimiento[siguientePunto];
                GirarAObjetivo(objetivo.position);
            }
        }
    }


    private void SeguirJugador()
    {
        if (transformJugador != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidad * Time.deltaTime);
            GirarAObjetivo(transformJugador.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Caja"))
        {
            estadoActual = Estado.Quieto;
            gameObject.SetActive(false);
        }
    }

    private void Girar()
    {
        mirandoDer = !mirandoDer;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
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



    private void OnDrawGizmosSelected()
    {
        if (puntosMovimiento.Length >= 2)
        {
            Vector2 centro = (puntosMovimiento[0].position + puntosMovimiento[1].position) / 2f;
            Vector2 tamaño = new Vector2(distanciaBusquedaX * 2f, distanciaBusquedaY * 2f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(centro, tamaño);
        }
    }

}
