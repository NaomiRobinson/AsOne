using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
using System.Collections;

public class CambioCamaraFinal : MonoBehaviour
{
    [Header("Virtual Cameras")]
    public CinemachineCamera camaraJugador1;
    public CinemachineCamera camaraJugador2;
    public CinemachineCamera camaraFusion;

    [Header("Cámaras físicas (con Brain)")]
    public Camera camFisicaJugador1;
    public Camera camFisicaJugador2;
    public Camera camFisicaFusion;

    private bool jugador1Dentro = false;
    private bool jugador2Dentro = false;

    public Image panelNegro;

    public Image divisionPantalla;
    public float duracionFade = 1f;
    private bool enTransicion = false;

    private void Start()
    {

        SetPantallaDividida();

        Debug.Log("[Start] Configuración inicial -> Pantalla dividida activa.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JugadorIzq"))
            jugador1Dentro = true;

        if (other.CompareTag("JugadorDer"))
            jugador2Dentro = true;

        if (jugador1Dentro && jugador2Dentro && !enTransicion)
        {
            StartCoroutine(CambiarConFade(true));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("JugadorIzq"))
            jugador1Dentro = false;

        if (other.CompareTag("JugadorDer"))
            jugador2Dentro = false;

        if ((!jugador1Dentro || !jugador2Dentro) && !enTransicion)
        {
            StartCoroutine(CambiarConFade(false));
        }
    }

    private IEnumerator CambiarConFade(bool aFusion)
    {
        enTransicion = true;

        // Fade in
        yield return StartCoroutine(FadeIn());

        if (aFusion)
        {
            SetCamaraFusion();
            Debug.Log("[Fusion ACTIVADA con fade]");
        }
        else
        {
            SetPantallaDividida();
            Debug.Log("[Fusion DESACTIVADA -> Pantalla dividida con fade]");
        }

        // Fade out
        yield return StartCoroutine(FadeOut());

        enTransicion = false;
    }

    private void SetPantallaDividida()
    {
        camFisicaJugador1.enabled = true;
        camFisicaJugador2.enabled = true;
        camFisicaFusion.enabled = false;

        camaraJugador1.Priority.Value = 10;
        camaraJugador2.Priority.Value = 10;
        camaraFusion.Priority.Value = 0;
        divisionPantalla.gameObject.SetActive(true);
    }

    private void SetCamaraFusion()
    {
        camFisicaJugador1.enabled = false;
        camFisicaJugador2.enabled = false;
        camFisicaFusion.enabled = true;

        camaraFusion.Priority.Value = 20;
        camaraJugador1.Priority.Value = 0;
        camaraJugador2.Priority.Value = 0;
        divisionPantalla.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        panelNegro.gameObject.SetActive(true);
        for (float t = 0; t < duracionFade; t += Time.deltaTime)
        {
            float alpha = t / duracionFade;
            panelNegro.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        panelNegro.color = Color.black;
    }

    private IEnumerator FadeOut()
    {
        for (float t = 0; t < duracionFade; t += Time.deltaTime)
        {
            float alpha = 1 - (t / duracionFade);
            panelNegro.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        panelNegro.color = new Color(0, 0, 0, 0);
        panelNegro.gameObject.SetActive(false);
    }
}