using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject PanelDePausa;

    private bool juegoPausado = false;

    public void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f; //pausa el juego
        PanelDePausa.SetActive(true); //muestra el panel de pausa
    }

    public void RenaudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f; //renauda el juego
        PanelDePausa.SetActive(false); //oculta el panel
    }

    public void VolverMenuPrin()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        PanelDePausa.SetActive(false);

        SceneManager.LoadScene("Menu");

    }

}
