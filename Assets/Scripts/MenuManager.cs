using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{


    public AudioSource musica;
    public void PlayGame()
    {
        ReiniciarProgreso();
        SceneManager.LoadScene("Invertidos");
    }

    public void ShowHelp()
    {

        SceneManager.LoadScene("Ayuda");
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ReiniciarProgreso()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); if (LevelManager.Instance != null)
        {
            LevelManager.Instance.grupoDesbloqueado = 1;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ControlMusica()
    {
        musica.mute = !musica.mute;
    }

}
