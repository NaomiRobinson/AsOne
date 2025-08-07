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

    //sonidos
    public static SoundManager instance;

    public AudioSource sfxSource;

    public AudioClip boton_interfaz_jugar;
    public AudioClip boton_interfsz_generico;
    public AudioClip cambiar_gravedad_01;
    public AudioClip cambiar_gravedad_02;
    public AudioClip cambiar_gravedad_03;
    public AudioClip enemigo_disparo;
    public AudioClip jugador_muerte;
    public AudioClip llave_recolectada;
    public AudioClip mecanismo_compuerta;
    public AudioClip mecanismo_palanca;
    public AudioClip portal_atravesarlo;
    public AudioClip trampa_caer;

    //
    private AudioSource audioSource;
    private static SoundManager instancia;

    private bool silenciado = true;

    void Awake()
    {
        ActualizarIcono();

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

        if (silenciado)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }

        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CambiarMusicaSegunEscena(scene.name);

        // Buscar el botón en la nueva escena
        GameObject botonGO = GameObject.FindWithTag("Audio");
        if (botonGO != null)
        {
            botonSonido = botonGO.GetComponent<Button>();
            botonSonido.onClick.RemoveAllListeners(); // Limpia listeners anteriores
            botonSonido.onClick.AddListener(ToggleMusica); // Asigna función
            ActualizarIcono();
        }
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
    /*
    public void SilenciarMusica(bool silenciar)
    {
        audioSource.mute = silenciar;
    }

    public bool EstaSilenciado()
    {
        return audioSource.mute;
    }
    */
    public void ToggleMusica()
    {
        Debug.Log("Click recibido en el botón de música");
        

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

    //sonidos/efectos especiales
    public void ReproducirSonido(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }


}
