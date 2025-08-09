using UnityEngine;

public class DeteccionZonaBloqueo : MonoBehaviour
{
    public MovimientoJugador movimientoJugador;
    public MovimientoJugador.Jugador jugadorTipo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ZonaSinInvertir"))
        {
            movimientoJugador.PuedeInvertir(jugadorTipo, false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ZonaSinInvertir"))
        {
            movimientoJugador.PuedeInvertir(jugadorTipo, true);
        }
    }
}
