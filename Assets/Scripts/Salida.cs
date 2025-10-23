using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;
using Unity.Cinemachine;
using System.Collections;


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
        MovimientoJugador movimiento = MovimientoJugador.Instancia;

        if (other.gameObject != jugadorAsignado) return;

        // Verificar fragmento si aplica
        if (requiereFragmento && fragmentoAsociado != null && !fragmentoAsociado.juntoFragmento)
        {
            if (popupFaltaFragmento != null) popupFaltaFragmento.SetActive(true);
            return;
        }

        if (animPuerta != null)
            animPuerta.SetBool("estaAbierta", true);

        // Aumentar contador global
        jugadoresEnSalida++;

        // Mostrar popup si hay solo 1 jugador
        if (jugadoresEnSalida == 1 && popupFaltaJugador != null)
            StartCoroutine(MostrarPopupFaltaJugadorConRetraso());

        // Iniciar animación si hay 2 jugadores
        if (jugadoresEnSalida == 2 && !nivelCompletandose)
            StartCoroutine(AnimacionPortalAmbos());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (animPuerta != null)
            animPuerta.SetBool("estaAbierta", false);

        if (other.gameObject != jugadorAsignado) return;

        // Reducir contador global
        if (jugadoresEnSalida > 0)
            jugadoresEnSalida--;

        if (popupFaltaJugador != null) popupFaltaJugador.SetActive(false);
        if (popupFaltaFragmento != null) popupFaltaFragmento.SetActive(false);

        Debug.Log($"[Exit] {other.gameObject.name} salió de la salida");
    }


    private void PasarNivel()
    {
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

        // Desactivar popups
        if (popupFaltaJugador != null) popupFaltaJugador.SetActive(false);
        if (popupFaltaFragmento != null) popupFaltaFragmento.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        MovimientoJugador movimiento = MovimientoJugador.Instancia;

        // Desactivar movimiento de ambos jugadores
        movimiento.puedeMoverse = false;

        // Animar ambos jugadores hacia sus portales simultáneamente
        float duracionAnim = 1f;

        AnimarJugador(movimiento.jugadorIzq, duracionAnim);
        AnimarJugador(movimiento.jugadorDer, duracionAnim);

        // Esperar la animación
        yield return new WaitForSeconds(duracionAnim);

        // Contador de animaciones completadas
        jugadoresAnimados = 2;

        // Pasar nivel
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
