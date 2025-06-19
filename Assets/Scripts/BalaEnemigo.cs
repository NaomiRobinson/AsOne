using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalaEnemigo : MonoBehaviour
{
    public float velocidad;
    //public int daño;

    // public MovimientoJugador movimientoJugador;


    private void Update()
    {
        transform.Translate(Time.deltaTime * velocidad * Vector2.right);
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
