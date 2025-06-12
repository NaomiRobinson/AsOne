using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuPausa : MonoBehaviour
{
    public GameObject PanelDePausa;


    private bool juegoPausado = false;
    public TextMeshProUGUI nombreNivel;

    private void Start()
    {
        if (nombreNivel !=null)
        {
            nombreNivel.text =  SceneManager.GetActiveScene().name;
        }
    }

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
    //reiniciar el nivel
    public void RNivel()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        PanelDePausa.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    //volver al menu principal
    public void VolverMenuPrin()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        PanelDePausa.SetActive(false);

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
}
