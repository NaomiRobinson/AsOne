using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;
using Unity.Cinemachine;
using System.Collections;


public class Salida : MonoBehaviour
{
    public GameObject jugadorAsignado;
    public GameObject popupFaltaFragmento;
    public GameObject popupFaltaJugador;

    private Animator animPuerta;

    public RecolectarFragmento fragmentoAsociado;
    private static int jugadoresEnSalida = 0;

    public bool requiereFragmento = false;

    private static bool nivelCompletandose = false;

    private void Start()
    {
        jugadoresEnSalida = 0;
        nivelCompletandose = false;
        animPuerta = GetComponent<Animator>();

        if (requiereFragmento && popupFaltaFragmento != null)
            popupFaltaFragmento.SetActive(false);
        if (popupFaltaJugador != null)
            popupFaltaJugador.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != jugadorAsignado) return;
        if (nivelCompletandose) return;

        Debug.Log($"[Enter] {jugadorAsignado.name} entró a la salida");

        if (requiereFragmento && fragmentoAsociado != null && !fragmentoAsociado.juntoFragmento)
        {
            Debug.Log("Falta el fragmento, puerta cerrada.");

            if (popupFaltaFragmento != null)
                popupFaltaFragmento.SetActive(true);

            return;
        }

        jugadoresEnSalida++;

        if (jugadoresEnSalida == 1)
        {
            StartCoroutine(MostrarPopupFaltaJugadorConRetraso());

        }

        if (jugadoresEnSalida == 2)
        {
            StartCoroutine(DesactivarPopupsYCambiarNivel());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (animPuerta != null)
            animPuerta.SetBool("estaAbierta", false);

        if (other.gameObject != jugadorAsignado) return;

        Debug.Log($"[Exit] {jugadorAsignado.name} salió de la salida");

        if (jugadoresEnSalida > 0)
            jugadoresEnSalida--;

        if (popupFaltaJugador != null)
            popupFaltaJugador.SetActive(false);
        if (popupFaltaFragmento != null)
            popupFaltaFragmento.SetActive(false);
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


    private IEnumerator DesactivarPopupsYCambiarNivel()
    {
        nivelCompletandose = true;
        if (popupFaltaJugador != null) popupFaltaJugador.SetActive(false);
        if (popupFaltaFragmento != null) popupFaltaFragmento.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.portal_atravesarlo);
        PasarNivel();
    }

    private IEnumerator MostrarPopupFaltaJugadorConRetraso()
    {
        yield return new WaitForSeconds(0.3f); // ⏱ espera un poco por si entra el otro jugador enseguida

        if (nivelCompletandose) yield break; // si ya está terminando el nivel, no mostrar nada
        if (jugadoresEnSalida == 1 && popupFaltaJugador != null)
            popupFaltaJugador.SetActive(true);
    }





}
