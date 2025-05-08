using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void PlayGame()
    {

        SceneManager.LoadScene("Nivel1");
    }

    public void ShowHelp()
    {

        SceneManager.LoadScene("Ayuda");
        Debug.Log("Entro a ayuda");
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }



}
