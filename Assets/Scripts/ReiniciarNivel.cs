using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;
using System.Collections;

public class ReiniciarNivel : MonoBehaviour
{
    public MovimientoJugador movimientoJugador;
    private Animator animatorJugador;

    public enum Jugador { Izq, Der }

    void Start()
    {
        animatorJugador = gameObject.GetComponent<Animator>();
    }

    public void Morir(float delay = 0.6f)
    {
        if (MovimientoJugador.Instancia.ModoInvencible) return;
        if (!MovimientoJugador.Instancia.puedeMoverse) return;

        MovimientoJugador.Instancia.puedeMoverse = false;

        Rigidbody2D rbIzq = MovimientoJugador.Instancia.jugadorIzq.GetComponent<Rigidbody2D>();
        Rigidbody2D rbDer = MovimientoJugador.Instancia.jugadorDer.GetComponent<Rigidbody2D>();
        Animator animIzq = MovimientoJugador.Instancia.animatorJugador;
        Animator animDer = MovimientoJugador.Instancia.animatorEspejado;


        Rigidbody2D rbMuerto = GetComponent<Rigidbody2D>();

        rbMuerto.linearVelocity = Vector2.zero;
        rbMuerto.angularVelocity = 0f;
        rbMuerto.bodyType = RigidbodyType2D.Kinematic;

        if (rbMuerto == rbIzq)
        {
            rbDer.linearVelocity = Vector2.zero;
            rbDer.angularVelocity = 0f;
            rbDer.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            rbIzq.linearVelocity = Vector2.zero;
            rbIzq.angularVelocity = 0f;
            rbIzq.bodyType = RigidbodyType2D.Kinematic;
        }

        if (SoundManager.instancia != null)
        {
            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.jugador_muerte, 1.3f);
        }


        if (animIzq != null)
        {
            animIzq.ResetTrigger("murio");
            animIzq.SetTrigger("murio");
        }

        if (animDer != null)
        {
            animDer.ResetTrigger("murio");
            animDer.SetTrigger("murio");
        }

        StartCoroutine(Reiniciar(delay));
    }

    private void DetenerJugador(Rigidbody2D rb)
    {
        if (rb == null) return;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            Morir();
        }
    }
    private IEnumerator Reiniciar(float delay)
    {
        yield return new WaitForSeconds(delay);
        EstadoDeJuego.ReinicioPorMuerte = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
