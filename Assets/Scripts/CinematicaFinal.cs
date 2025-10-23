using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class CinematicaFinal : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nombreEscenaVictoria = "Victoria";
    public string nombreArchivoVideo = "CinematicaFinal.mp4";

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
            videoPlayer.Play();
        };

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            if (!esperaConfirmacion)
            {
                if (saltear != null)
                    saltear.gameObject.SetActive(true);
                esperaConfirmacion = true;
            }
            else
            {
                SaltarCinematica();
            }
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        CargarVictoria();
    }

    void SaltarCinematica()
    {
        videoPlayer.Stop();
        CargarVictoria();
    }

    void CargarVictoria()
    {
        PlayerPrefs.SetInt("CinematicaFinalVista", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nombreEscenaVictoria);
    }
}
