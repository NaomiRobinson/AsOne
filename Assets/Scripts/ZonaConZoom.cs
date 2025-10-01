using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class ZonaConZoom : MonoBehaviour
{
    public float zoomOrthoSize = 3f;

    public CinemachineCamera vcJugador;
    public CinemachineCamera vcSalida;

    public int prioridadSalida = 25;

    private int prioridadOriginalSalida;
    private int prioridadOriginalJugador;

    void Start()
    {
        if (vcJugador == null)
            Debug.LogError("No se asignó Virtual Camera del jugador en el inspector.");
        if (vcSalida == null)
            Debug.LogError("No se asignó Virtual Camera de salida en el inspector.");

        prioridadOriginalJugador = vcJugador.Priority;
        prioridadOriginalSalida = vcSalida.Priority;

        // Jugador arranca con prioridad máxima
        vcJugador.Priority = prioridadOriginalJugador + 10;
        vcSalida.Priority = prioridadOriginalSalida;
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JugadorIzq") || collision.CompareTag("JugadorDer"))
        {
            // Subimos salida y bajamos jugador
            vcSalida.Priority = prioridadSalida;
            vcJugador.Priority = prioridadOriginalJugador;

            Debug.Log($"[ENTER] Jugador: {vcJugador.Priority} | Salida: {vcSalida.Priority}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!(other.CompareTag("JugadorIzq") || other.CompareTag("JugadorDer"))) return;

        // Restauramos: salida vuelve a su prioridad original y jugador vuelve a mandar
        vcSalida.Priority = prioridadOriginalSalida;
        vcJugador.Priority = prioridadOriginalJugador + 10;

        Debug.Log($"[EXIT] Jugador: {vcJugador.Priority} | Salida: {vcSalida.Priority}");
    }
    private IEnumerator RestaurarPrioridadJugador(float delay)
    {
        yield return new WaitForSeconds(delay);
        vcJugador.Priority = prioridadOriginalJugador;
    }

}
