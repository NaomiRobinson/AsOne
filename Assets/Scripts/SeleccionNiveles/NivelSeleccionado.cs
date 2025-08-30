using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class NivelSeleccionado : MonoBehaviour
{
    public int grupoSeleccionado;
    public GameObject jugadorAsignado;
    private Animator animPuerta;
    private static int jugadoresEnPuerta = 0;
    private bool estaEnPuerta = false;

    public bool esPuertaFinal = false;

    private bool yaSelecciono = false;

    public TMP_Text textoEstado;

    public GameObject indAbierto;
    public GameObject indBloqueado;
    public GameObject indCompleto;



    private bool puertaBloqueada = false;

    void Start()
    {
        animPuerta = GetComponent<Animator>();
        RevisarEstadoPuerta();
        textoEstado.gameObject.SetActive(false);
    }

    void Update()
    { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == jugadorAsignado)
        {
            estaEnPuerta = true;
            RevisarEstadoPuerta(); // <- Revisa si está bloqueada


            if (!puertaBloqueada && !yaSelecciono)
            {
                StartCoroutine(CountdownCoroutine());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == jugadorAsignado)
        {
            estaEnPuerta = false;
            yaSelecciono = false;
            StopAllCoroutines();
            textoEstado.gameObject.SetActive(false); // <- Oculta el texto al salir
            textoEstado.text = "";

            if (animPuerta != null)
                animPuerta.SetBool("estaAbierta", false);

            if (jugadoresEnPuerta > 0)
                jugadoresEnPuerta--;

            Debug.Log($"{jugadorAsignado.name} salió de la salida. Jugadores en salida: {jugadoresEnPuerta}");
        }
    }


    IEnumerator CountdownCoroutine()
    {
        textoEstado.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            textoEstado.text = i.ToString();
            yield return StartCoroutine(FadeTextOut(textoEstado, 1f)); // 1 segundo para el fade out
            if (!estaEnPuerta) yield break; // si sale antes del timer, cancela el conteo
        }

        textoEstado.gameObject.SetActive(false);
        yaSelecciono = true;
        EjecutarCargaNivel();
    }

    IEnumerator FadeTextOut(TMP_Text text, float duration)
    {
        Color originalColor = text.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    void RevisarEstadoPuerta()
    {
        indAbierto.SetActive(false);
        indBloqueado.SetActive(false);
        indCompleto.SetActive(false);

        puertaBloqueada = false;

        // Bloqueado por grupo
        if (grupoSeleccionado > LevelManager.Instance.grupoDesbloqueado)
        {
            puertaBloqueada = true;
            indBloqueado.SetActive(true);
        }
        // Puerta final pero faltan fragmentos
        else if (esPuertaFinal && !ChequeoLlaves.TodasRecolectadas())
        {
            puertaBloqueada = true;
            indBloqueado.SetActive(true);
        }
        else
        {
            // Chequeamos si ya fue completado
            if (FueCompletado(grupoSeleccionado))
            {
                indCompleto.SetActive(true);
            }
            else
            {
                indAbierto.SetActive(true);
            }
        }
    }
    void EjecutarCargaNivel()
    {
        if (animPuerta != null && !esPuertaFinal)
            AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);

        jugadoresEnPuerta++;
        Debug.Log($"Jugadores en puerta incrementado a: {jugadoresEnPuerta}");

        if (jugadoresEnPuerta == 2)
        {
            int escenaActual = SceneManager.GetActiveScene().buildIndex;
            Debug.Log($"Escena actual buildIndex: {escenaActual}");

            if (escenaActual == LevelManager.Instance.SeleccionNiveles)
            {
                if (grupoSeleccionado > 0)
                {
                    Debug.Log($"Intentando seleccionar grupo {grupoSeleccionado}");
                    LevelManager.Instance.SeleccionarGrupo(grupoSeleccionado);
                }
                else if (esPuertaFinal)
                {
                    if (ChequeoLlaves.TodasRecolectadas())
                    {
                        AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);
                        Debug.Log("¡Todas las llaves están recolectadas!");
                        LevelManager.Instance.CargarFinal();
                    }
                    else
                    {
                        Debug.LogWarning("No todas las llaves están recolectadas, no se puede cargar final.");
                    }
                }
                else
                {
                    Debug.LogError("Grupo inválido para selección: " + grupoSeleccionado);
                }
            }
            else if (esPuertaFinal)
            {
                if (ChequeoLlaves.TodasRecolectadas())
                {
                    AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);
                    Debug.Log("¡Todas las llaves están recolectadas!");
                    LevelManager.Instance.CargarFinal();
                }
                else
                {
                    Debug.LogWarning("No todas las llaves están recolectadas, no se puede cargar final.");
                }
            }
            else
            {
                Debug.Log("Entrando en flujo de niveles dentro de un grupo.");
                int nivelActual = escenaActual;

                Debug.Log("Es último nivel? " + LevelManager.Instance.EsUltimoNivel(nivelActual));
                if (LevelManager.Instance.EsUltimoNivel(nivelActual))
                {
                    Debug.Log("Último nivel detectado. Marcando grupo como completado.");
                    LevelManager.Instance.MarcarGrupoCompletado();
                    Debug.Log("Grupo desbloqueado tras completar: " + LevelManager.Instance.grupoDesbloqueado);
                    TransicionEscena.Instance.Disolversalida(LevelManager.Instance.SeleccionNiveles);
                }
                else
                {
                    int siguiente = LevelManager.Instance.ObtenerSiguienteNivel(nivelActual);
                    Debug.Log($"Siguiente nivel a cargar: {siguiente}");
                    TransicionEscena.Instance.Disolversalida(siguiente);
                }
            }
        }
    }

    private bool FueCompletado(int grupo)
    {
        return PlayerPrefs.GetInt($"GrupoCompletado_{grupo}", 0) == 1;
    }



}
