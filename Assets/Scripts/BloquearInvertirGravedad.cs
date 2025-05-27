using UnityEngine;

public class BloquearInvertirGravedad : MonoBehaviour
{
    private MovimientoJugador movimientoJugador;

    private void Start()
    {
        movimientoJugador = FindObjectOfType<MovimientoJugador>();
        if (movimientoJugador == null)
        {
            Debug.LogWarning($"[BloquearInvertirGravedad] No se encontró MovimientoJugador en la escena para {gameObject.name}");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JugadorIzq"))
        {
            movimientoJugador.PuedeInvertir(MovimientoJugador.Jugador.Izq, false);
            Debug.Log($"[BloquearInvertirGravedad] JugadorIzq entró a la zona {gameObject.name}");
        }
        else if (other.CompareTag("JugadorDer"))
        {
            movimientoJugador.PuedeInvertir(MovimientoJugador.Jugador.Der, false);
            Debug.Log($"[BloquearInvertirGravedad] JugadorDer entró a la zona {gameObject.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("JugadorIzq"))

        {
            movimientoJugador.PuedeInvertir(MovimientoJugador.Jugador.Izq, true);
            Debug.Log($"[BloquearInvertirGravedad] JugadorIzq salió de la zona {gameObject.name}");
        }
        else if (other.CompareTag("JugadorDer"))
        {
            movimientoJugador.PuedeInvertir(MovimientoJugador.Jugador.Der, true);
            Debug.Log($"[BloquearInvertirGravedad] JugadorDer salió de la zona {gameObject.name}");
        }
    }
}
