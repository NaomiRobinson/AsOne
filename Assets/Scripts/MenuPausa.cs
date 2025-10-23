using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public static MenuPausa Instancia { get; private set; }

    public GameObject PanelDePausa;
    public GameObject botonPorDefecto;
    public AudioSource musica;

    public TextMeshProUGUI textoModoInvencible;
    public TextMeshProUGUI nombreNivel;
    private Controles controles;

    public bool juegoPausado { get; private set; } = false;


    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);

            controles = new Controles();
            controles.Jugador.Enable();
            controles.UI.Disable();
        }
        else if (Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnEscenaCargada;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnEscenaCargada;
    }

    private void OnEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        if (escena.name == "Menu" || escena.name == "Ayuda" || escena.name == "Victoria" ||
            escena.name == "Creditos" || escena.name == "Cinematica")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            ActualizarNombreNivel();
            ActualizarTextoInvencible();
            PanelDePausa.SetActive(false);

            StartCoroutine(ActualizarBotonesExternos());
        }
    }

    private void Start()
    {
        if (PanelDePausa != null)
            PanelDePausa.SetActive(false);

        Button botonPausar = transform.Find("BotonPausa")?.GetComponent<Button>();
        if (botonPausar != null)
            botonPausar.onClick.AddListener(inputPausar);

        ConfigurarBotonesDelPanel();

        if (nombreNivel != null)
            nombreNivel.text = SceneManager.GetActiveScene().name;

        ActualizarTextoInvencible();
    }

    private void ConfigurarBotonesDelPanel()
    {
        if (PanelDePausa == null) return;

        Transform panel = PanelDePausa.transform;

        panel.Find("Reanudar")?.GetComponent<Button>()?.onClick.AddListener(ReanudarJuego);
        panel.Find("Reiniciar")?.GetComponent<Button>()?.onClick.AddListener(ReiniciarNivel);
        panel.Find("VolverMenuPrincipal")?.GetComponent<Button>()?.onClick.AddListener(VolverMenu);
        panel.Find("NivelA")?.GetComponent<Button>()?.onClick.AddListener(NivelAnterior);
        panel.Find("NivelP")?.GetComponent<Button>()?.onClick.AddListener(NivelSiguiente);
        panel.Find("Selector")?.GetComponent<Button>()?.onClick.AddListener(VolverAlSelector);

        Button botonMusicaPanel = panel.Find("MusicaPanel")?.GetComponent<Button>();
        if (botonMusicaPanel != null)
        {
            botonMusicaPanel.onClick.AddListener(() =>
            {
                if (SoundManager.instancia != null)
                {
                    SoundManager.instancia.ToggleMusica();

                    Image img = botonMusicaPanel.GetComponent<Image>();
                    if (img != null)
                        img.sprite = SoundManager.instancia.MusicaMuted
                            ? SoundManager.instancia.iconoSonidoOff
                            : SoundManager.instancia.iconoSonidoOn;

                    SoundManager.instancia.ActualizarIcono();
                }
            });

            Image imgInicial = botonMusicaPanel.GetComponent<Image>();
            if (imgInicial != null)
                imgInicial.sprite = SoundManager.instancia.MusicaMuted
                    ? SoundManager.instancia.iconoSonidoOff
                    : SoundManager.instancia.iconoSonidoOn;
        }

        Button botonSfxPanel = panel.Find("SonidosPanel")?.GetComponent<Button>();
        if (botonSfxPanel != null)
        {
            botonSfxPanel.onClick.AddListener(() =>
            {
                if (SoundManager.instancia != null)
                {
                    SoundManager.instancia.ToggleSonidos();

                    Image img = botonSfxPanel.GetComponent<Image>();
                    if (img != null)
                        img.sprite = SoundManager.instancia.SonidosMuted
                            ? SoundManager.instancia.iconoSfxOff
                            : SoundManager.instancia.iconoSfxOn;

                    SoundManager.instancia.ActualizarIconoSonidos();
                }
            });

            Image imgInicial = botonSfxPanel.GetComponent<Image>();
            if (imgInicial != null)
                imgInicial.sprite = SoundManager.instancia.SonidosMuted
                    ? SoundManager.instancia.iconoSfxOff
                    : SoundManager.instancia.iconoSfxOn;
        }
    }

    public void inputPausar()
    {
        if (TransicionEscena.Instance != null && TransicionEscena.Instance.TransicionEnCurso)
        {
            Debug.Log("No se puede pausar durante la transición.");
            return;
        }

        if (juegoPausado)
            ReanudarJuego();
        else
            PausarJuego();
    }

    private void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        ActualizarTextoInvencible();

        if (PanelDePausa != null)
        {
            controles.Jugador.Disable();
            controles.UI.Enable();
            PanelDePausa.SetActive(true);

            if (SoundManager.instancia != null)
            {
                SoundManager.instancia.ActualizarIcono();
                SoundManager.instancia.ActualizarIconoSonidos();
            }

            StartCoroutine(SeleccionarBotonPorDefecto());
        }
    }

    public void ReanudarJuego()
    {
        if (PanelDePausa != null)
            PanelDePausa.SetActive(false);

        juegoPausado = false;
        Time.timeScale = 1f;
        controles.UI.Disable();
        controles.Jugador.Enable();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        if (PanelDePausa != null)
            PanelDePausa.SetActive(false);

        juegoPausado = false;
        controles.UI.Disable();
        controles.Jugador.Disable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlSelector()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        controles.UI.Disable();
        controles.Jugador.Enable();
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene("SeleccionNiveles");
    }

    private void VolverMenu()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        controles.UI.Disable();
        controles.Jugador.Disable();
        Destroy(gameObject);
        SceneManager.LoadScene("Menu");
    }

    private void NivelAnterior()
    {
        int indice = SceneManager.GetActiveScene().buildIndex;

        if (LevelManager.Instance.EsPrimerNivel(indice))
        {
            juegoPausado = false;
            Time.timeScale = 1f;
            controles.UI.Disable();
            controles.Jugador.Enable();
            EventSystem.current.SetSelectedGameObject(null);
            SceneManager.LoadScene("SeleccionNiveles");
            return;
        }

        if (PanelDePausa != null)
            PanelDePausa.SetActive(false);

        juegoPausado = false;
        Time.timeScale = 1f;
        controles.UI.Disable();
        controles.Jugador.Enable();
        EventSystem.current.SetSelectedGameObject(null);

        int anterior = LevelManager.Instance.ObtenerNivelAnterior(indice);
        SceneManager.LoadScene(anterior);
    }

    private void NivelSiguiente()
    {
        int indice = SceneManager.GetActiveScene().buildIndex;
        int total = SceneManager.sceneCountInBuildSettings;

        if (indice < total - 1)
        {
            if (LevelManager.Instance.EsUltimoNivel(indice))
            {
                LevelManager.Instance.MarcarGrupoCompletado();
                Time.timeScale = 1f;
                juegoPausado = false;
                controles.UI.Disable();
                controles.Jugador.Enable();
                EventSystem.current.SetSelectedGameObject(null);

                SceneManager.LoadScene(LevelManager.Instance.SeleccionNiveles);
                return;
            }

            Time.timeScale = 1f;
            juegoPausado = false;
            controles.UI.Disable();
            controles.Jugador.Enable();
            EventSystem.current.SetSelectedGameObject(null);

            SceneManager.LoadScene(indice + 1);
        }
    }

    public void ControlMusica()
    {
        if (SoundManager.instancia != null)
            SoundManager.instancia.ToggleMusica();
    }

    private IEnumerator SeleccionarBotonPorDefecto()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);

        if (botonPorDefecto != null)
            EventSystem.current.SetSelectedGameObject(botonPorDefecto);
    }

    private void LogControlesActivos()
    {
        foreach (var map in controles.asset.actionMaps)
        {
            Debug.Log($"{map.name} está {(map.enabled ? "HABILITADO" : "DESHABILITADO")}");
        }
    }

    public void ActualizarNombreNivel()
    {
        if (nombreNivel != null)
            nombreNivel.text = SceneManager.GetActiveScene().name;
    }

    public void ActualizarTextoInvencible()
    {
        if (textoModoInvencible != null && MovimientoJugador.Instancia != null)
        {
            textoModoInvencible.gameObject.SetActive(MovimientoJugador.Instancia.ModoInvencible);
        }
    }

    private IEnumerator ActualizarBotonesExternos()
    {
        yield return null;

        Button botonMute = transform.Find("Musica")?.GetComponent<Button>();
        if (botonMute != null && SoundManager.instancia != null)
        {
            botonMute.onClick.RemoveAllListeners();
            botonMute.onClick.AddListener(() => SoundManager.instancia.ToggleMusica());

            Image img = botonMute.GetComponent<Image>();
            if (img != null)
                img.sprite = SoundManager.instancia.MusicaMuted
                    ? SoundManager.instancia.iconoSonidoOff
                    : SoundManager.instancia.iconoSonidoOn;
        }

        Button botonMuteSonidos = transform.Find("Sonidos")?.GetComponent<Button>();
        if (botonMuteSonidos != null && SoundManager.instancia != null)
        {
            botonMuteSonidos.onClick.RemoveAllListeners();
            botonMuteSonidos.onClick.AddListener(() => SoundManager.instancia.ToggleSonidos());

            Image img = botonMuteSonidos.GetComponent<Image>();
            if (img != null)
                img.sprite = SoundManager.instancia.SonidosMuted
                    ? SoundManager.instancia.iconoSfxOff
                    : SoundManager.instancia.iconoSfxOn;
        }
    }
}