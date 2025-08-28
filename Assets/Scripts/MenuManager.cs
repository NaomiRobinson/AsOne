using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public AudioSource musica;
    public GameObject botonPorDefecto;

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
        //ReiniciarProgreso();
        SceneManager.LoadScene("Invertidos");
    }

    public void ContinueGame()
    {
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfaz_jugar);

        if (PlayerPrefs.HasKey("GrupoDesbloqueado"))
        {

            SceneManager.LoadScene(LevelManager.Instance.SeleccionNiveles);
        }
        else
        {

            SceneManager.LoadScene("Invertidos");
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
        SceneManager.LoadScene("Invertidos");
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
