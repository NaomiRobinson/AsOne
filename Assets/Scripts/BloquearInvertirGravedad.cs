using UnityEngine;

public class BloquearInvertirGravedad : MonoBehaviour
{
    [SerializeField] private LayerMask capaZona;
    [SerializeField] private GameObject jugadorIzq;
    [SerializeField] private GameObject jugadorDer;

    private MovimientoJugador movimientoJugador;

    private bool jugadorIzqDentro = false;
    private bool jugadorDerDentro = false;

    [SerializeField, Tooltip("Radio para chequeo de zona alrededor del jugador")]
    private float chequeoRadio = 0.5f;

    private void Start()
    {
        movimientoJugador = FindObjectOfType<MovimientoJugador>();
        if (movimientoJugador == null)
            Debug.LogWarning("No se encontró MovimientoJugador en la escena.");

        if (jugadorIzq == null && movimientoJugador != null)
            jugadorIzq = movimientoJugador.jugadorIzq;

        if (jugadorDer == null && movimientoJugador != null)
            jugadorDer = movimientoJugador.jugadorDer;
    }

    private void Update()
    {
        bool estabaDentroIzq = jugadorIzqDentro;
        bool estabaDentroDer = jugadorDerDentro;

        jugadorIzqDentro = Physics2D.OverlapCircle(jugadorIzq.transform.position, chequeoRadio, capaZona);
        jugadorDerDentro = Physics2D.OverlapCircle(jugadorDer.transform.position, chequeoRadio, capaZona);

        if (jugadorIzqDentro != estabaDentroIzq)
        {
            movimientoJugador.PuedeInvertir(MovimientoJugador.Jugador.Izq, !jugadorIzqDentro);
            Debug.Log($"[Zona] JugadorIzq dentro={jugadorIzqDentro}");
        }

        if (jugadorDerDentro != estabaDentroDer)
        {
            movimientoJugador.PuedeInvertir(MovimientoJugador.Jugador.Der, !jugadorDerDentro);
            Debug.Log($"[Zona] JugadorDer dentro={jugadorDerDentro}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (jugadorIzq != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(jugadorIzq.transform.position, chequeoRadio);
        }

        if (jugadorDer != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(jugadorDer.transform.position, chequeoRadio);
        }
    }
}