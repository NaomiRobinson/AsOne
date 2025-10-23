using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class Cinematica : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nombreNivelTutorial = "Invertidos";
    public string nombreArchivoVideo = "Cinematica_Inicial.mp4";

    public TextMeshProUGUI saltear;

    private bool esperaConfirmacion = false;


    void Start()
    {
        if (saltear != null)
            saltear.gameObject.SetActive(false);

        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, nombreArchivoVideo);
        videoPlayer.url = videoPath;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (source) =>
        {
            videoPlayer.playbackSpeed = 0.5f;
            videoPlayer.Play();
        };

        // Evento al terminar
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        // Detectar tecla o botón
        if (Keyboard.current.anyKey.wasPressedThisFrame ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            if (!esperaConfirmacion)
            {
                // Primera vez: mostrar el texto de confirmación
                if (saltear != null)
                    saltear.gameObject.SetActive(true);
                esperaConfirmacion = true;
            }
            else
            {
                // Segunda vez: saltar la cinemática
                SaltarCinematica();
            }
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        CargarTutorial();
    }

    void SaltarCinematica()
    {
        videoPlayer.Stop();
        CargarTutorial();
    }

    void CargarTutorial()
    {
        PlayerPrefs.SetInt("CinematicaVista", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nombreNivelTutorial);
    }
}