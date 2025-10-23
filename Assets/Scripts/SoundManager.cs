using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instancia;
    //música
    public AudioClip musicaMenu;
    public AudioClip musicaGrupo1;
    public AudioClip musicaGrupo2;
    public AudioClip musicaGrupo3;
    public AudioClip musicaGrupo4;

    public AudioClip musicaCinematica;
    public AudioClip musicaCinematicaFinal;
    public Button botonSonido;
    public Sprite iconoSonidoOn;
    public Sprite iconoSonidoOff;

    //sonidos o sfx dasd

    public Button botonSfx;
    public Sprite iconoSfxOn;
    public Sprite iconoSfxOff;

    private bool sfxSilenciado = false;

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
    public AudioClip contador;
    public AudioClip portal_activandose;
    public AudioClip portal_viajando;
    public AudioClip gema_sensor;

    //
    private AudioSource audioSource;



    private bool silenciado = false;

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
        audioSource.mute = silenciado;
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
            botonSonido.onClick.RemoveAllListeners();
            botonSonido.onClick.AddListener(ToggleMusica);
            ActualizarIcono();
        }

        // Botón de sonidos (SFX)
        GameObject botonSfxGO = GameObject.FindWithTag("AudioSFX");
        if (botonSfxGO != null)
        {
            botonSfx = botonSfxGO.GetComponent<Button>();
            botonSfx.onClick.RemoveAllListeners();
            botonSfx.onClick.AddListener(ToggleSonidos);
            ActualizarIconoSonidos();
        }

    }

    void CambiarMusicaSegunEscena(string nombreEscena)
    {
        AudioClip clipAUsar = null;

        if (nombreEscena == "Menu" || nombreEscena == "Invertidos" ||
            nombreEscena == "SeleccionNiveles" || nombreEscena == "Ayuda")
        {
            clipAUsar = musicaMenu;
        }
        else
        {
            switch (LevelManager.Instance.grupoActual)
            {
                case 1: clipAUsar = musicaGrupo1; audioSource.volume = 1f; break;
                case 2: clipAUsar = musicaGrupo2; audioSource.volume = 0.3f; break;
                case 3: clipAUsar = musicaGrupo3; audioSource.volume = 0.5f; break;
                case 4: clipAUsar = musicaGrupo4; audioSource.volume = 1f; break;
                default: clipAUsar = musicaMenu; audioSource.volume = 1f; break;
            }
        }

        if (audioSource.clip != clipAUsar)
        {
            audioSource.clip = clipAUsar;
            audioSource.loop = true;
            audioSource.mute = silenciado;
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
        audioSource.mute = silenciado;

        if (silenciado)
        {
            Debug.Log("Música silenciada");
        }
        else
        {
            Debug.Log("Música activada");
        }


        ActualizarIcono();
    }

    public void ToggleSonidos()
    {
        Debug.Log("Click recibido en el botón de SFX");

        sfxSilenciado = !sfxSilenciado;
        sfxSource.mute = sfxSilenciado;

        if (sfxSilenciado)
            Debug.Log("Sonidos silenciados");
        else
            Debug.Log("Sonidos activados");

        ActualizarIconoSonidos();
    }



    public void ActualizarIcono()
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

    public void ActualizarIconoSonidos()
    {
        if (botonSfx != null)
        {
            Image imagenBoton = botonSfx.GetComponent<Image>();
            if (imagenBoton != null)
            {
                imagenBoton.sprite = sfxSilenciado ? iconoSfxOff : iconoSfxOn;
            }
        }
    }


    public void ReproducirSonido(AudioClip clip, float volumen = 1f)
    {
        if (!sfxSilenciado && clip != null)
        {
            sfxSource.PlayOneShot(clip,volumen);
        }
    }



}
