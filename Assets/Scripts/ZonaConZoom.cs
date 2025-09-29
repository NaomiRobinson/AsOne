using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class ZonaConZoom : MonoBehaviour
{
    public float zoomOrthoSize = 3f;

    public CinemachineCamera vcJugador;
    public CinemachineCamera vcSalida;

    public int prioridadSalida = 20;

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

        vcJugador.Priority = prioridadOriginalJugador + 10;
        vcSalida.Priority = prioridadOriginalSalida;

        StartCoroutine(RestaurarPrioridadJugador(0f));
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JugadorIzq") || collision.CompareTag("JugadorDer"))
        {

            if (vcSalida != null)
                vcSalida.Priority = prioridadSalida;


            Debug.Log($"Prioridad VC Jugador: {vcJugador.Priority}");
            Debug.Log($"Prioridad VC Salida: {vcSalida.Priority}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!(other.CompareTag("JugadorIzq") || other.CompareTag("JugadorDer"))) return;
        vcSalida.Priority = prioridadOriginalSalida;


        vcJugador.Priority = prioridadOriginalJugador + 1;
        StartCoroutine(RestaurarPrioridadJugador(0.1f));

        Debug.Log($"Prioridad VC Jugador: {vcJugador.Priority}");
        Debug.Log($"Prioridad VC Salida: {vcSalida.Priority}");
    }

    private IEnumerator RestaurarPrioridadJugador(float delay)
    {
        yield return new WaitForSeconds(delay);
        vcJugador.Priority = prioridadOriginalJugador;
    }

}
