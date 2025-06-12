using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalaEnemigo : MonoBehaviour
{
    public float velocidad;
    public int daño;

    public MovimientoJugador movimientoJugador;

    
    private void Update()
    {
        transform.Translate(Time.deltaTime * velocidad * Vector2.right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si colisiona con algún jugador
        if (collision.gameObject.CompareTag("JugadorIzq") || collision.gameObject.CompareTag("JugadorDer"))
        {
            MovimientoJugador jugador = collision.gameObject.GetComponent<MovimientoJugador>();

            // Si el jugador no es invencible, reiniciar la escena
            if (jugador != null && jugador.modoInvencible == false)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // Si colisiona con una pared, destruir la bala
        if (collision.gameObject.CompareTag("Walls"))
        {
            Debug.Log("bala toco pared");
            Destroy(gameObject);
        }
    }


    /*   private void OnTriggerEnter2D(Collider2D collision)
       {
           if ((collision.gameObject.CompareTag("JugadorIzq") & movimientoJugador.modoInvencible == false) || (collision.gameObject.CompareTag("JugadorDer") & movimientoJugador.modoInvencible == false))
           {

               SceneManager.LoadScene(SceneManager.GetActiveScene().name);
           }


           if (collision.gameObject.CompareTag("Walls"))
           {
               Debug.Log("bala toco pared");
               Destroy(gameObject);
           }


       }
   */
}
