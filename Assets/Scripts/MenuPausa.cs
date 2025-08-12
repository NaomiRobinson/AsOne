using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;


public class MenuPausa : MonoBehaviour
{
    public static MenuPausa Instancia { get; private set; }
    public GameObject PanelDePausa;

    private Controles controles;

    public GameObject botonPorDefecto;

    public AudioSource musica;

    [HideInInspector] public bool juegoPausado = false;
    public TextMeshProUGUI nombreNivel;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            controles = new Controles();
            controles.Jugador.Enable();
            controles.UI.Disable();
        }
    }

    private void Start()
    {
        if (nombreNivel != null)
        {
            nombreNivel.text = SceneManager.GetActiveScene().name;
        }

    }

    public void inputPausar()
    {
        if (juegoPausado) ReanudarJuego();
        else PausarJuego();
    }


    public void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        PanelDePausa.SetActive(true);
        controles.Jugador.Disable();
        controles.UI.Enable();
        StartCoroutine(SeleccionarBotonPorDefecto());
    }

    public void ReanudarJuego()
    {

        juegoPausado = false;
        Time.timeScale = 1f;
        PanelDePausa.SetActive(false);
        controles.UI.Disable();
        controles.Jugador.Enable();
        EventSystem.current.SetSelectedGameObject(null);


    }
    //reiniciar el nivel
    public void RNivel()
    {

        Time.timeScale = 1f;
        juegoPausado = false;
        PanelDePausa.SetActive(false);
        controles.UI.Disable();
        controles.Jugador.Disable();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     //   LogControlesActivos();

    }
    //volver al menu principal
    public void VolverMenuPrin()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        // PanelDePausa.SetActive(false);
        controles.UI.Disable();
        controles.Jugador.Disable();
        SceneManager.LoadScene("Menu");
    }
    //cambiar a un nivel anterior
    public void NivelAnterior()
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        if (escenaActual > 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(escenaActual - 1);
        }
    }
    //cambiar a un nivel posterior
    public void NivelSiguiente()
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        int totalEscenas = SceneManager.sceneCountInBuildSettings;

        if (escenaActual < totalEscenas - 1)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(escenaActual + 1);
        }
    }
    public void ControlMusica()
    {
        musica.mute = !musica.mute;
    }
    private IEnumerator SeleccionarBotonPorDefecto()
    {
        yield return null; // espera un frame

        EventSystem.current.SetSelectedGameObject(null);
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
