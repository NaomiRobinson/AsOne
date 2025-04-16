using UnityEngine;
using UnityEngine.SceneManagement;

public class Salida : MonoBehaviour
{
    public GameObject jugadorAsignado;


    public AbrirPuerta puertaAsociada;
    private static int jugadoresEnSalida = 0;

    public bool requiereAbrir = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == jugadorAsignado)
        {
            if (requiereAbrir && puertaAsociada != null && !puertaAsociada.EstaAbierta())
            {
                Debug.Log("La puerta está cerrada, no puedes pasar.");
                return;
            }

            if (!requiereAbrir && puertaAsociada != null)
            {
                puertaAsociada.ForzarAbrir(); // Se abre sola
            }

            jugadoresEnSalida++;

            // Nueva verificación: ¿ambos están y la puerta está abierta?
            if (jugadoresEnSalida == 2)
            {
                if (!requiereAbrir || (puertaAsociada != null && puertaAsociada.EstaAbierta()))
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
        Debug.Log("Ambos jugadores están en sus salidas");

        TransicionEscena.Instance.Disolversalida(SceneManager.GetActiveScene().buildIndex + 1);



    }

}
