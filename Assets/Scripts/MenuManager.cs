using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public AudioSource musica;
    public void PlayGame()
    {
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfaz_jugar);
        ReiniciarProgreso();
        SceneManager.LoadScene("Tutorial");
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
        PlayerPrefs.Save();if (LevelManager.Instance != null)
    {
        LevelManager.Instance.grupoDesbloqueado = 1;
    }
    }

    public void ExitGame()
    {
        SoundManager.instancia.ReproducirSonido(SoundManager.instancia.boton_interfsz_generico);
        Application.Quit();
    }
    /*
    public void ControlMusica()
    {
        musica.mute = !musica.mute;
    }
    */
}
