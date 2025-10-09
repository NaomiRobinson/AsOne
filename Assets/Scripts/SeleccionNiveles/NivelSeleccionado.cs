using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using Unity.Cinemachine;

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
    private bool estabaBloqueadaAntes = true; // para detectar desbloqueo


    void Start()
    {
        animPuerta = GetComponent<Animator>();

        LevelManager.Instance.grupoActual = Mathf.Min(grupoSeleccionado, LevelManager.Instance.grupoDesbloqueado);

        RevisarEstadoPuerta();
        textoEstado.gameObject.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == jugadorAsignado)
        {
            
            estaEnPuerta = true;
            RevisarEstadoPuerta();

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
            textoEstado.gameObject.SetActive(false);
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

            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.contador); // sonido del contador

            yield return StartCoroutine(FadeTextOut(textoEstado, 1f));
            if (!estaEnPuerta) yield break;
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

    bool todosCompletos = NivelSeleccionado.TodosLosGruposCompletados();
    bool grupoCompletado = PlayerPrefs.GetInt($"GrupoCompletado_{grupoSeleccionado}", 0) == 1;

    if (grupoSeleccionado > LevelManager.Instance.grupoDesbloqueado ||
        (esPuertaFinal && !todosCompletos))
    {
        puertaBloqueada = true;
        indBloqueado.SetActive(true);
    }
    else if (grupoCompletado)
    {
        indCompleto.SetActive(true);
    }
    else
    {
        indAbierto.SetActive(true);
    }

        //reproducir sonido cuando la puerta se caba de desbloquear
        if (estabaBloqueadaAntes && !puertaBloqueada)
        {
            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.portal_activandose);
        }

    }

    void EjecutarCargaNivel()
    {
        if (animPuerta != null && !esPuertaFinal)
            AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);

        jugadoresEnPuerta++;
        if (jugadoresEnPuerta < 2) return;

        //reproducir sonido cuando los jugadores pasan de escena
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.portal_viajando);

        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        int nivelACargar = -1;

        if (escenaActual == LevelManager.Instance.SeleccionNiveles && grupoSeleccionado > 0)
        {
            nivelACargar = grupoSeleccionado switch
            {
                1 => LevelManager.Instance.nivelesGrupo1[0],
                2 => LevelManager.Instance.nivelesGrupo2[0],
                3 => LevelManager.Instance.nivelesGrupo3[0],
                4 => LevelManager.Instance.nivelesGrupo4[0],
                _ => -1
            };
            if (nivelACargar == -1) { Debug.LogError("Grupo inválido"); return; }
            LevelManager.Instance.SeleccionarGrupo(grupoSeleccionado);
        }
        else if (esPuertaFinal)
        {
            if (!TodosLosGruposCompletados())
            {
                Debug.LogWarning("No todos los grupos fueron completados, no se puede cargar final.");
                return;
            }
            LevelManager.Instance.CargarFinal();
            return;
        }
        else
        {
            if (!LevelManager.Instance.EsUltimoNivel(escenaActual))
            {
                nivelACargar = LevelManager.Instance.ObtenerSiguienteNivel(escenaActual);
            }
            else
            {
                LevelManager.Instance.MarcarGrupoCompletado();
                nivelACargar = LevelManager.Instance.SeleccionNiveles;
            }
        }

        if (nivelACargar <= 0 || nivelACargar >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"Índice de escena inválido: {nivelACargar}");
            return;
        }

        TransicionEscena.Instance.Disolversalida(nivelACargar);
    }


    private bool FueCompletado(int grupo)
    {
        return PlayerPrefs.GetInt($"GrupoCompletado_{grupo}", 0) == 1;
    }

    public static bool TodosLosGruposCompletados()
    {

        for (int i = 1; i <= 4; i++)
        {
            if (PlayerPrefs.GetInt($"GrupoCompletado_{i}", 0) == 0)
                return false;
        }
        return true;
    }

   
}
