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


    public TextMeshProUGUI nombreNivel;
    private Controles controles;

    public bool juegoPausado { get; private set; } = false;


    private void Awake()
    {
        string nombreEscena = SceneManager.GetActiveScene().name;

        // Destruir este objeto si es un menú
        if (nombreEscena == "Menu" || nombreEscena == "Ayuda" || nombreEscena == "Victoria")
        {
            Destroy(gameObject);
            return;
        }

        // Singleton para mantener persistencia en niveles
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
            controles = new Controles();
            controles.Jugador.Enable();
            controles.UI.Disable();
        }
        else
        {
            Destroy(gameObject);
            return;
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


    }

    public void inputPausar()
    {
        if (juegoPausado) ReanudarJuego();
        else PausarJuego();
    }

    private void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f;

        if (PanelDePausa != null)
        {
            controles.Jugador.Disable();
            controles.UI.Enable();
            PanelDePausa.SetActive(true);

            Button botonNivelAnterior = PanelDePausa.transform.Find("NivelA")?.GetComponent<Button>();
            if (botonNivelAnterior != null)
                botonNivelAnterior.interactable = SceneManager.GetActiveScene().buildIndex != 2;

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
        if (indice > 0)
        {
            if (PanelDePausa != null) PanelDePausa.SetActive(false);
            juegoPausado = false;
            Time.timeScale = 1f;
            controles.UI.Disable();
            controles.Jugador.Enable();
            EventSystem.current.SetSelectedGameObject(null);
            SceneManager.LoadScene(indice - 1);
        }
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
        if (musica != null)
            musica.mute = !musica.mute;
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
}
