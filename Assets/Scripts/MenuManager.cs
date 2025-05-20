using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void PlayGame()
    {
        ReiniciarProgreso();
        SceneManager.LoadScene("Tutorial");
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
        PlayerPrefs.Save();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
