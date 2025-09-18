using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;
using Unity.Cinemachine;
using System.Collections;


public class Salida : MonoBehaviour
{
    public GameObject jugadorAsignado;

    private Animator animPuerta;

    public RecolectarFragmento fragmentoAsociado;
    private static int jugadoresEnSalida = 0;

    public bool requiereFragmento = false;


    public float zoomOrthoSize = 3f;

    public CinemachineCamera vcJugador;
    public CinemachineCamera vcSalida;

    public int prioridadSalida = 20;

    private int prioridadOriginalSalida;
    private int prioridadOriginalJugador;



    private void Start()
    {
        animPuerta = GetComponent<Animator>();

        if (vcJugador == null)
            Debug.LogError("No se asignó Virtual Camera del jugador en el inspector.");
        if (vcSalida == null)
            Debug.LogError("No se asignó Virtual Camera de salida en el inspector.");

        prioridadOriginalJugador = vcJugador.Priority;
        prioridadOriginalSalida = vcSalida.Priority;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != jugadorAsignado) return;

        // Activar zoom usando prioridad
        if (vcSalida != null)
            vcSalida.Priority = prioridadSalida;

        Debug.Log($"[Enter] {jugadorAsignado.name} entró a la salida");
        Debug.Log($"Prioridad VC Jugador: {vcJugador.Priority}");
        Debug.Log($"Prioridad VC Salida: {vcSalida.Priority}");

        if (requiereFragmento && fragmentoAsociado != null && fragmentoAsociado.juntoFragmento == false)
        {
            Debug.Log("La puerta está cerrada, no puedes pasar.");
            return;
        }

        if (!requiereFragmento || (fragmentoAsociado != null && fragmentoAsociado.juntoFragmento == true))
        {
            jugadoresEnSalida++;
        }

        if (jugadoresEnSalida == 2)
        {
            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.portal_atravesarlo);
            PasarNivel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (animPuerta != null)
            animPuerta.SetBool("estaAbierta", false);

        if (other.gameObject != jugadorAsignado) return;

        vcSalida.Priority = prioridadOriginalSalida;

        vcJugador.Priority = prioridadOriginalJugador + 1;
        StartCoroutine(RestorePlayerPriority());
        Debug.Log($"[Exit] {jugadorAsignado.name} salió de la salida");
        Debug.Log($"Prioridad VC Jugador: {vcJugador.Priority}");
        Debug.Log($"Prioridad VC Salida: {vcSalida.Priority}");

        if (jugadoresEnSalida > 0)
            jugadoresEnSalida--;
    }


    private void PasarNivel()
    {
        int nivelActual = SceneManager.GetActiveScene().buildIndex;
        int siguiente;

        Debug.Log("Ambos jugadores están en sus salidas");
        Debug.Log("Completo un nivel");

        if (nivelActual == LevelManager.Instance.nivelTutorial)
        {
            siguiente = LevelManager.Instance.SeleccionNiveles;
        }
        else if (LevelManager.Instance.EsUltimoNivel(nivelActual))
        {
            Debug.Log("¡Es el último nivel del grupo!");

            LevelManager.Instance.MarcarGrupoCompletado();
            Debug.Log("Grupo desbloqueado tras completar: " + LevelManager.Instance.grupoDesbloqueado);

            siguiente = LevelManager.Instance.SeleccionNiveles;
        }
        else
        {
            siguiente = LevelManager.Instance.ObtenerSiguienteNivel(nivelActual);
        }

        TransicionEscena.Instance.Disolversalida(siguiente);
    }



    private IEnumerator RestorePlayerPriority()
    {
        yield return new WaitForSeconds(0.1f);
        vcJugador.Priority = prioridadOriginalJugador;
    }

}
