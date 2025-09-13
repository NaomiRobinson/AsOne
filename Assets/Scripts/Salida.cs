using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;
using Unity.Cinemachine;


public class Salida : MonoBehaviour
{
    public GameObject jugadorAsignado;

    private Animator animPuerta;

    public RecolectarFragmento fragmentoAsociado;
    private static int jugadoresEnSalida = 0;

    public bool requiereFragmento = false;

    public ZoomCamara zoomScript;
    public float zoomOrthoSize = 3f;



    private void Start()
    {
        animPuerta = GetComponent<Animator>();

        if (zoomScript == null)
            Debug.LogError("No se asignó ZoomCamara en el inspector.");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject == jugadorAsignado)
        {
            zoomScript.zoomOrthoSize = zoomOrthoSize;
            zoomScript.ActivarZoom(jugadorAsignado.transform);

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
                if (!requiereFragmento || (fragmentoAsociado != null && fragmentoAsociado.juntoFragmento == true))
                {
                    SoundManager.instancia.ReproducirSonido(SoundManager.instancia.portal_atravesarlo);
                    PasarNivel();
                }
                else
                {
                    Debug.Log("Ambos están en la salida, pero la puerta aún está cerrada.");
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (animPuerta != null)
        {
            animPuerta.SetBool("estaAbierta", false);
        }
        if (other.gameObject == jugadorAsignado)
        {
            zoomScript.RestaurarZoom();
            if (jugadoresEnSalida > 0)
            {
                jugadoresEnSalida--;
            }
            Debug.Log($"{jugadorAsignado.name} salió de la salida. Jugadores en salida: {jugadoresEnSalida}");
        }
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

}
