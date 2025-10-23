using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public AudioSource musica;
    public GameObject botonPorDefecto;
    public GameObject popUpConfirmacion;
    public GameObject botonPopUp;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botonPorDefecto);
    }


    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(botonPorDefecto);
        }
    }

    public void PlayGame()
    {
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfaz_jugar);
        popUpConfirmacion.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botonPopUp);

    }

    public void ConfirmarPlayGame()
    {
        popUpConfirmacion.SetActive(false); // Ocultar pop-up
        ReiniciarProgreso(); // Reinicia todo

        if (!PlayerPrefs.HasKey("CinematicaVista"))
        {
            SceneManager.LoadScene("Cinematica");
        }
        else
        {
            SceneManager.LoadScene("Invertidos");
        }
    }

    public void CancelarPlayGame()
    {
        popUpConfirmacion.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botonPorDefecto);

    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("NivelActual"))
        {
            int nivel = PlayerPrefs.GetInt("NivelActual");
            int grupo = LevelManager.Instance.ObtenerGrupoDeNivel(nivel);
            LevelManager.Instance.grupoActual = grupo;
            SceneManager.LoadScene(nivel);
        }
        else
        {
            if (!PlayerPrefs.HasKey("CinematicaVista"))
            {
                SceneManager.LoadScene("Cinematica");
            }
            else
            {
                SceneManager.LoadScene("Invertidos");
            }
        }
    }



    public void ShowHelp()
    {
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfsz_generico);
        SceneManager.LoadScene("Ayuda");
    }


    public void ReturnToMenu()
    {
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfsz_generico);
        SceneManager.LoadScene("Menu");
    }

    public void ReiniciarProgreso()
    {
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfsz_generico);

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.grupoDesbloqueado = 1;
        }
    }

    public void ExitGame()
    {
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfsz_generico);
        Application.Quit();
    }



    public void Creditos()
    {
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfsz_generico);
        SceneManager.LoadScene("Creditos");
    }


}
