using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;
using System.Collections;

public class ReiniciarNivel : MonoBehaviour
{
    public MovimientoJugador movimientoJugador;
    private Animator animatorJugador;

    void Start()
    {
        animatorJugador = gameObject.GetComponent<Animator>();
    }

    public void Morir(float delay = 0.6f)
    {
        if (MovimientoJugador.Instancia.ModoInvencible) return;

        if (!MovimientoJugador.Instancia.puedeMoverse) return;

        MovimientoJugador.Instancia.puedeMoverse = false;
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.jugador_muerte);

        // Animación
        animatorJugador.ResetTrigger("murio");
        animatorJugador.SetTrigger("murio");

        // Reiniciar escena después del delay
        StartCoroutine(Reiniciar(delay));
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            Morir();
        }
    }
    private IEnumerator Reiniciar(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("GameOver");
        EstadoDeJuego.ReinicioPorMuerte = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
