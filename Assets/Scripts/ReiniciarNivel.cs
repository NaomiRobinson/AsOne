using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;

public class ReiniciarNivel : MonoBehaviour
{
    public MovimientoJugador movimientoJugador;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            if (movimientoJugador.modoInvencible == false)
            {
                Reiniciar();
            }

        }
    }

    public void  Reiniciar(){
          Debug.Log("GameOver");
          
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
