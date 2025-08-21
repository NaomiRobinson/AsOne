using UnityEngine;

public class BloquearInvertirGravedad : MonoBehaviour
{
    [SerializeField] private LayerMask capaZona;       // Capa de la zona que bloquea la gravedad
    [SerializeField] private GameObject jugadorIzq;    // Jugador izquierdo
    [SerializeField] private GameObject jugadorDer;    // Jugador derecho

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
        // Guardar estado previo para detectar cambios
        bool estabaDentroIzq = jugadorIzqDentro;
        bool estabaDentroDer = jugadorDerDentro;

        // Chequear si los jugadores están dentro de la zona usando OverlapCircle y capa Player
        jugadorIzqDentro = Physics2D.OverlapCircle(jugadorIzq.transform.position, chequeoRadio, capaZona);
        jugadorDerDentro = Physics2D.OverlapCircle(jugadorDer.transform.position, chequeoRadio, capaZona);

        // Actualizar bloqueo solo si cambió el estado
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

    // Opcional: visualizar el radio en la escena
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