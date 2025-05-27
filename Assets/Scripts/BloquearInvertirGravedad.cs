using UnityEngine;

public class BloquearInvertirGravedad : MonoBehaviour
{
    public enum AfectaA { JugadorIzq, JugadorDer }
    public AfectaA afectaA; // Elegir desde el Inspector

    public MovimientoJugador movimientoJugador;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (EsPersonajeObjetivo(other.gameObject))
        {
            Debug.Log($"[BloquearInvertirGravedad] Entró {other.name} a la zona {gameObject.name}");
            movimientoJugador.PuedeInvertir(this, false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (EsPersonajeObjetivo(other.gameObject))
        {
            Debug.Log($"[BloquearInvertirGravedad] Salió {other.name} de la zona {gameObject.name}");
            movimientoJugador.PuedeInvertir(this, true);
        }
    }

    private bool EsPersonajeObjetivo(GameObject objeto)
    {
        switch (afectaA)
        {
            case AfectaA.JugadorIzq:
                return objeto == movimientoJugador.jugador;
            case AfectaA.JugadorDer:
                return objeto == movimientoJugador.jugadorEspejado;
            default:
                return false;
        }
    }
}
