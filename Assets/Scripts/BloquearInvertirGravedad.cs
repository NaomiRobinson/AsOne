using UnityEngine;

public class BloquearInvertirGravedad : MonoBehaviour
{
    private MovimientoJugador movimientoJugador;

    [SerializeField] private LayerMask capaZonaSinInvertir;
    [SerializeField] private GameObject jugadorIzq;
    [SerializeField] private GameObject jugadorDer;

    private bool jugadorIzqDentro = false;
    private bool jugadorDerDentro = false;

    [SerializeField, Tooltip("Radio para chequeo de zona alrededor del jugador")]
    private float chequeoRadio = 0.5f;

    private void Start()
    {
        movimientoJugador = FindObjectOfType<MovimientoJugador>();
        if (movimientoJugador == null)
            Debug.LogWarning("No se encontró MovimientoJugador en la escena.");

        // Asignar jugadores si no están asignados en inspector
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

        // Chequear si los jugadores están dentro de la zona (usando OverlapCircle)
        jugadorIzqDentro = Physics2D.OverlapCircle(jugadorIzq.transform.position, chequeoRadio, capaZonaSinInvertir);
        jugadorDerDentro = Physics2D.OverlapCircle(jugadorDer.transform.position, chequeoRadio, capaZonaSinInvertir);

        // Si hubo cambio, actualizar permiso de invertir gravedad y loggear
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
}
