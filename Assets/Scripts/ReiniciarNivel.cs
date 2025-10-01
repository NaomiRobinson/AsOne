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
            if (!MovimientoJugador.Instancia.ModoInvencible)
            {
                SoundManager.instancia.ReproducirSonido(SoundManager.instancia.jugador_muerte);
                Reiniciar();
            }

        }
    }
    public void Reiniciar()
    {
        Debug.Log("GameOver");
        EstadoDeJuego.ReinicioPorMuerte = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
