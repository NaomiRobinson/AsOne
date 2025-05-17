using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;

public class Salida : MonoBehaviour
{
    public GameObject jugadorAsignado;

    private Animator animPuerta;

    public AbrirPuerta palancaAsociada;
    private static int jugadoresEnSalida = 0;

    public bool requiereAbrir = false;


    void Start()
    {
        animPuerta = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject == jugadorAsignado)
        {

            if (requiereAbrir && palancaAsociada != null && !palancaAsociada.EstaAbierta())
            {
                Debug.Log("La puerta está cerrada, no puedes pasar.");
                return;
            }
            if (!requiereAbrir)
            {
                AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);

            }
            if (!requiereAbrir && palancaAsociada != null)
            {
                palancaAsociada.Abrir();

            }

            jugadoresEnSalida++;


            if (jugadoresEnSalida == 2)
            {
                if (!requiereAbrir || (palancaAsociada != null && palancaAsociada.EstaAbierta()))
                {
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
        // SessionData.level++;
        if (LevelManager.Instance.EsUltimoNivel(nivelActual))
        {
            siguiente = LevelManager.Instance.SeleccionNiveles; ;
        }
        else
        {
            siguiente = LevelManager.Instance.ObtenerSiguienteNivel(nivelActual);
        }
        TransicionEscena.Instance.Disolversalida(siguiente);

    }

}
