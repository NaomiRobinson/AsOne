using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;
using Unity.Cinemachine;
using System.Collections;


public class Salida : MonoBehaviour
{
    public GameObject jugadorAsignado;
    // public GameObject popupFaltaFragmento;

    private Animator animPuerta;

    public RecolectarFragmento fragmentoAsociado;
    private static int jugadoresEnSalida = 0;

    public bool requiereFragmento = false;






    private void Start()
    {
        jugadoresEnSalida = 0;
        animPuerta = GetComponent<Animator>();


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != jugadorAsignado) return;



        Debug.Log($"[Enter] {jugadorAsignado.name} entró a la salida");


        if (requiereFragmento && fragmentoAsociado != null && fragmentoAsociado.juntoFragmento == false)
        {
            Debug.Log("La puerta está cerrada, no puedes pasar.");

            // if (popupFaltaFragmento != null)
            // {
            //     popupFaltaFragmento.SetActive(true);

            // }

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


        Debug.Log($"[Exit] {jugadorAsignado.name} salió de la salida");


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







}
