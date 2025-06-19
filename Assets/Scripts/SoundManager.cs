using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
   
    public AudioClip musicaMenu;
    public AudioClip musicaJuego;
    public Button botonSonido;
    public Sprite iconoSonidoOn;
    public Sprite iconoSonidoOff;

    private AudioSource audioSource;
    private static SoundManager instancia;

    private bool silenciado = true;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ActualizarIcono();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CambiarMusicaSegunEscena(scene.name);
    }

    void CambiarMusicaSegunEscena(string nombreEscena)
    {
        AudioClip clipAUsar = null;

        if (nombreEscena == "Menu" || nombreEscena == "Tutorial" || nombreEscena == "SeleccionNiveles" || nombreEscena == "Ayuda")
        {
            clipAUsar = musicaMenu;
        }
        else
        {
            clipAUsar = musicaJuego;
        }

        if (audioSource.clip != clipAUsar)
        {
            audioSource.clip = clipAUsar;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void SilenciarMusica(bool silenciar)
    {
        audioSource.mute = silenciar;
    }

    public bool EstaSilenciado()
    {
        return audioSource.mute;
    }
    public void ToggleMusica()
    {
        silenciado = !silenciado;

        if (silenciado)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }

        ActualizarIcono();
    }
    void ActualizarIcono()
    {
        if (botonSonido != null)
        {
            Image imagenBoton = botonSonido.GetComponent<Image>();
            if (imagenBoton != null)
            {
                imagenBoton.sprite = silenciado ? iconoSonidoOff : iconoSonidoOn;
            }
        }
    }
}
