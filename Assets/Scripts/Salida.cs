using UnityEngine;
using UnityEngine.SceneManagement;

public class Salida : MonoBehaviour
{
    public GameObject jugadorAsignado;
    private static int jugadoresEnSalida = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == jugadorAsignado)
        {
            if (jugadoresEnSalida < 2) // Evita valores incorrectos
            {
                jugadoresEnSalida++;
            }
            if (jugadoresEnSalida == 2)
            {
                PasarNivel();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }



}
