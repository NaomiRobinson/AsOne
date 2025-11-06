using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.Rendering.Universal;


public class Salida : MonoBehaviour
{
    public GameObject jugadorAsignado;
    public GameObject popupFaltaFragmento;
    public GameObject popupFaltaJugador;

    private Animator animPuerta;

    public RecolectarFragmento fragmentoAsociado;
    private static int jugadoresEnSalida = 0;
    private bool jugadorIzqEnSalida = false;
    private bool jugadorDerEnSalida = false;

    public bool requiereFragmento = false;

    private static bool nivelCompletandose = false;

    private static int jugadoresAnimados = 0;

    private void Start()
    {
        jugadoresEnSalida = 0;
        nivelCompletandose = false;
        animPuerta = GetComponent<Animator>();

        if (requiereFragmento && popupFaltaFragmento != null)
            popupFaltaFragmento.SetActive(false);
        if (popupFaltaJugador != null)
            popupFaltaJugador.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != jugadorAsignado) return;

        if (other.CompareTag("JugadorIzq")) jugadorIzqEnSalida = true;
        if (other.CompareTag("JugadorDer")) jugadorDerEnSalida = true;

        if (requiereFragmento && fragmentoAsociado != null && !fragmentoAsociado.juntoFragmento)
        {
            if (popupFaltaFragmento != null) popupFaltaFragmento.SetActive(true);
            return;
        }

        if (animPuerta != null)
            animPuerta.SetBool("estaAbierta", true);

        jugadoresEnSalida++;

        if (jugadoresEnSalida == 1 && popupFaltaJugador != null)
            StartCoroutine(MostrarPopupFaltaJugadorConRetraso());

        if (jugadoresEnSalida == 2 && !nivelCompletandose)
            StartCoroutine(AnimacionPortalAmbos());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != jugadorAsignado) return;

        if (other.CompareTag("JugadorIzq")) jugadorIzqEnSalida = false;
        if (other.CompareTag("JugadorDer")) jugadorDerEnSalida = false;

        if (animPuerta != null)
            animPuerta.SetBool("estaAbierta", false);

        jugadoresEnSalida = Mathf.Max(0, jugadoresEnSalida - 1);

        if (popupFaltaJugador != null) popupFaltaJugador.SetActive(false);
        if (popupFaltaFragmento != null) popupFaltaFragmento.SetActive(false);

        Debug.Log($"[Exit] {other.gameObject.name} salió de la salida");
    }


    private void PasarNivel()
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogError("LevelManager no está instanciado.");
            return;
        }

        if (TransicionEscena.Instance == null)
        {
            Debug.LogError("TransicionEscena no está instanciado.");
            return;
        }

        int nivelActual = SceneManager.GetActiveScene().buildIndex;
        int siguiente;

        if (nivelActual == LevelManager.Instance.nivelTutorial)
            siguiente = LevelManager.Instance.SeleccionNiveles;
        else if (LevelManager.Instance.EsUltimoNivel(nivelActual))
        {
            LevelManager.Instance.MarcarGrupoCompletado();
            siguiente = LevelManager.Instance.SeleccionNiveles;
        }
        else
            siguiente = LevelManager.Instance.ObtenerSiguienteNivel(nivelActual);

        TransicionEscena.Instance.Disolversalida(siguiente);
    }


    private IEnumerator AnimacionPortalAmbos()
    {
        nivelCompletandose = true;

        if (popupFaltaJugador != null) popupFaltaJugador.SetActive(false);
        if (popupFaltaFragmento != null) popupFaltaFragmento.SetActive(false);



        yield return new WaitForSeconds(0.2f);


        MovimientoJugador movimiento = MovimientoJugador.Instancia;

        if (movimiento.indicadorArriba != null)
            movimiento.indicadorArriba.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (movimiento.indicadorAbajo != null)
            movimiento.indicadorAbajo.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        foreach (var luz in movimiento.jugadorIzq.GetComponentsInChildren<Light2D>())
            luz.enabled = false;
        foreach (var luz in movimiento.jugadorDer.GetComponentsInChildren<Light2D>())
            luz.enabled = false;


        movimiento.puedeMoverse = false;

        float duracionAnim = 1f;

        AnimarJugador(movimiento.jugadorIzq, duracionAnim);
        AnimarJugador(movimiento.jugadorDer, duracionAnim);

        yield return new WaitForSeconds(duracionAnim);

        jugadoresAnimados = 2;

        if (jugadoresAnimados == 2)
            PasarNivel();
    }
    private void AnimarJugador(GameObject jugador, float duracion)
    {
        Vector3 destino = jugador.transform.position;
        destino.z = jugador.transform.position.z;

        LeanTween.move(jugador, destino, duracion).setEase(LeanTweenType.easeInQuad);
        LeanTween.rotateZ(jugador, 360f, duracion);
        LeanTween.scale(jugador, Vector3.zero, duracion);

        if (SoundManager.instancia != null)
            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.portal_atravesarlo);
    }

    private IEnumerator MostrarPopupFaltaJugadorConRetraso()
    {
        yield return new WaitForSeconds(0.3f);
        if (nivelCompletandose) yield break;

        if ((jugadorIzqEnSalida ^ jugadorDerEnSalida) && popupFaltaJugador != null)
            popupFaltaJugador.SetActive(true);
    }


}
