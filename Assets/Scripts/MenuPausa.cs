using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
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


        // Suscribirse al evento de cambio de escena
        SceneManager.sceneLoaded += OnEscenaCargada;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnEscenaCargada;
    }

    private void OnEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        if (escena.name == "Menu" || escena.name == "Ayuda" || escena.name == "Victoria")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);

            ActualizarNombreNivel();
            ActualizarTextoInvencible();
        }
    }


    private void Start()
    {
        if (PanelDePausa != null)
            PanelDePausa.SetActive(false);

        // Botón de pausa
        Button botonPausar = transform.Find("BotonPausa")?.GetComponent<Button>();
        if (botonPausar != null)
            botonPausar.onClick.AddListener(inputPausar);

        Button botonMute = transform.Find("Musica")?.GetComponent<Button>();
        
        if (botonMute != null)
        {
            botonMute.onClick.RemoveAllListeners();
            botonMute.onClick.AddListener(() =>
            {
                if (SoundManager.instancia != null)
                    SoundManager.instancia.ToggleMusica();
            });

            if (SoundManager.instancia != null)
                SoundManager.instancia.ActualizarIcono();
        }

        // Botones dentro del panel
        if (PanelDePausa != null)
        {
            Transform panel = PanelDePausa.transform;

            panel.Find("Reanudar")?.GetComponent<Button>()?.onClick.AddListener(ReanudarJuego);
            panel.Find("Reiniciar")?.GetComponent<Button>()?.onClick.AddListener(ReiniciarNivel);
            panel.Find("VolverMenuPrincipal")?.GetComponent<Button>()?.onClick.AddListener(VolverMenu);

            panel.Find("NivelA")?.GetComponent<Button>()?.onClick.AddListener(NivelAnterior);

            panel.Find("NivelP")?.GetComponent<Button>()?.onClick.AddListener(NivelSiguiente);
        }

        if (nombreNivel != null) { nombreNivel.text = SceneManager.GetActiveScene().name; }
        ActualizarTextoInvencible();

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

            int indice = SceneManager.GetActiveScene().buildIndex;
            int total = SceneManager.sceneCountInBuildSettings;

            Button botonNivelAnterior = PanelDePausa.transform.Find("NivelA")?.GetComponent<Button>();
            if (botonNivelAnterior != null)
            {
                bool esSeleccion = SceneManager.GetActiveScene().name == "SeleccionNiveles";
                bool esPrimer = LevelManager.Instance.EsPrimerNivel(indice);

                botonNivelAnterior.interactable = !esSeleccion && !esPrimer;
            }

            Button botonNivelSiguiente = PanelDePausa.transform.Find("NivelP")?.GetComponent<Button>();
            if (botonNivelSiguiente != null)
            {
                bool esSeleccion = SceneManager.GetActiveScene().name == "SeleccionNiveles";
                bool esUltimo = LevelManager.Instance.EsUltimoNivel(indice);

                botonNivelSiguiente.interactable = !esSeleccion && !esUltimo && indice < total - 1;
            }

            StartCoroutine(SeleccionarBotonPorDefecto());
        }
    }


    public void ReanudarJuego()
    {
        if (PanelDePausa != null) PanelDePausa.SetActive(false);
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

        // Evitar retroceder en SeleccionNiveles o si es el primer nivel del grupo
        if (SceneManager.GetActiveScene().name == "SeleccionNiveles" ||
            LevelManager.Instance.EsPrimerNivel(indice))
        {
            Debug.Log("No se puede retroceder: primer nivel del grupo o SeleccionNiveles.");
            return;
        }

        if (PanelDePausa != null) PanelDePausa.SetActive(false);
        juegoPausado = false;
        Time.timeScale = 1f;
        controles.UI.Disable();
        controles.Jugador.Enable();
        EventSystem.current.SetSelectedGameObject(null);

        // Cargar nivel anterior dentro del grupo
        int anterior = LevelManager.Instance.ObtenerNivelAnterior(indice);
        SceneManager.LoadScene(anterior);
    }

    private void NivelSiguiente()
    {
        int indice = SceneManager.GetActiveScene().buildIndex;
        int total = SceneManager.sceneCountInBuildSettings;
        if (indice < total - 1)
        {
            if (PanelDePausa != null) PanelDePausa.SetActive(false);
            juegoPausado = false;
            Time.timeScale = 1f;
            controles.UI.Disable();
            controles.Jugador.Enable();
            EventSystem.current.SetSelectedGameObject(null);
            SceneManager.LoadScene(indice + 1);
        }
    }

    public void ControlMusica()
    {
        if (SoundManager.instancia != null)
        {
            SoundManager.instancia.ToggleMusica();
        }
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
            nombreNivel.text = "Nivel: " + SceneManager.GetActiveScene().name;
    }

    public void ActualizarTextoInvencible()
    {
        if (textoModoInvencible != null && MovimientoJugador.Instancia != null)
        {
            // Activar o desactivar el GameObject del texto según modoInvencible
            textoModoInvencible.gameObject.SetActive(MovimientoJugador.Instancia.ModoInvencible);
        }
    }

    //voler al selector de niveles
    public void VolverAlSelector()
    {
        //SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfsz_generico);
        SceneManager.LoadScene("Seleccion Niveles");

    }
}
