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
    private SpriteRenderer spriteRenderer;

    private static int jugadoresEnPuerta = 0;
    private bool estaEnPuerta = false;

    public bool esPuertaFinal = false;

    private bool yaSelecciono = false;

    public TMP_Text textoEstado;

    public GameObject indAbierto;

    public GameObject indCompleto;

    public Transform idCheckpoint;
    public int checkpointID;

    private bool puertaBloqueada = false;
    private bool estabaBloqueadaAntes = true;

    [SerializeField] private Color colorTerminado = Color.black;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animPuerta = GetComponent<Animator>();

        LevelManager.Instance.grupoActual = Mathf.Min(grupoSeleccionado, LevelManager.Instance.grupoDesbloqueado);

        RevisarEstadoPuerta();
        textoEstado.gameObject.SetActive(false);
        int checkpointGrupoGuardado = PlayerPrefs.GetInt("CheckpointGrupo", -1);
        int checkpointIDGuardado = PlayerPrefs.GetInt("CheckpointID", -1);

        if (checkpointGrupoGuardado == grupoSeleccionado && checkpointIDGuardado == checkpointID)
        {
            jugadorAsignado.transform.position = idCheckpoint.position;
        }

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
        indCompleto.SetActive(false);
        puertaBloqueada = false;

        //bool todosCompletos = NivelSeleccionado.TodosLosGruposCompletados();
        bool grupoCompletado = PlayerPrefs.GetInt($"GrupoCompletado_{grupoSeleccionado}", 0) == 1;

        if (grupoCompletado)
        {
            indCompleto.SetActive(true);
            indAbierto.SetActive(false);
            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorTerminado;
            }
        }
        else
        {
            indAbierto.SetActive(true);
            indCompleto.SetActive(false);
        }


        // if (estabaBloqueadaAntes && !puertaBloqueada)
        // {
        //     SoundManager.instancia.ReproducirSonido(SoundManager.instancia.portal_activandose);
        // }

        estabaBloqueadaAntes = puertaBloqueada;
    }
    void EjecutarCargaNivel()
    {
        PlayerPrefs.SetInt("CheckpointGrupo", grupoSeleccionado);
        PlayerPrefs.SetInt("CheckpointID", checkpointID);
        if (animPuerta != null)
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
            StartCoroutine(AnimacionPortalAmbos_Nivel(LevelManager.Instance.final));
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

        StartCoroutine(AnimacionPortalAmbos_Nivel(nivelACargar));
        return;
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


    private IEnumerator AnimacionPortalAmbos_Nivel(int nivelACargar)
    {
        MovimientoJugador movimiento = MovimientoJugador.Instancia;
        movimiento.puedeMoverse = false;

        // Apagar partículas
        if (movimiento.indicadorArriba != null)
            movimiento.indicadorArriba.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (movimiento.indicadorAbajo != null)
            movimiento.indicadorAbajo.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // foreach (var luz in movimiento.jugadorIzq.GetComponentsInChildren<Light2D>())
        //    luz.enabled = false;
        // foreach (var luz in movimiento.jugadorDer.GetComponentsInChildren<Light2D>())
        //     luz.enabled = false;

        float duracionAnim = 1f;

        AnimarJugadorPortal(movimiento.jugadorIzq, duracionAnim);
        AnimarJugadorPortal(movimiento.jugadorDer, duracionAnim);

        yield return new WaitForSeconds(duracionAnim);

        TransicionEscena.Instance.Disolversalida(nivelACargar);
    }

    private void AnimarJugadorPortal(GameObject jugador, float duracion)
    {
        Vector3 destino = jugador.transform.position;
        LeanTween.move(jugador, destino, duracion).setEase(LeanTweenType.easeInQuad);
        LeanTween.rotateZ(jugador, 360f, duracion);
        LeanTween.scale(jugador, Vector3.zero, duracion);

        if (SoundManager.instancia != null)
            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.portal_atravesarlo);
    }

}
