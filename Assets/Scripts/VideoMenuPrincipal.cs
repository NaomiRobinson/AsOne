using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoMenuPrincipal : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Ruta dentro de StreamingAssets
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Fondo_AS_ONE.mp4");

        videoPlayer.url = videoPath;
        videoPlayer.isLooping = true;

        // Asignar textura antes de reproducir
        rawImage.texture = videoPlayer.targetTexture;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (source) =>
        {
            videoPlayer.Play();
        };
    }
}
