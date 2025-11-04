using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{
    private bool activado = false;
    private bool jugadorIzqDentro = false;
    private bool jugadorDerDentro = false;
    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JugadorIzq"))
            jugadorIzqDentro = true;

        if (other.CompareTag("JugadorDer"))
            jugadorDerDentro = true;

        // Cuando ambos están dentro y aún no se activó
        if (jugadorIzqDentro || jugadorDerDentro || !activado)
        {
            activado = true;
            StartCoroutine(ProcesoFinal());
        }
    }

    IEnumerator ProcesoFinal()
    {
        // Desactivar movimiento de ambos jugadores
        MovimientoJugador[] jugadores = FindObjectsOfType<MovimientoJugador>();
        foreach (var jugador in jugadores)
        {
            jugador.puedeMoverse = false;

            var rb = jugador.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true;
            }
        }

        yield return new();

        SceneManager.LoadScene("CinematicaFinal");
    }
}
