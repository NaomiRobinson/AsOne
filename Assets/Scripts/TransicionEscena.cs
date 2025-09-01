using Unity.VisualScripting;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
using Unity.Services.Core;
using static EventManager;
using static StaticVariables;
using TMPro;

public class TransicionEscena : MonoBehaviour
{
    public static TransicionEscena Instance;
    public CanvasGroup disolverCanvas;
    public CanvasGroup disolverTexto;
    public float tiempoDisolverEntrada;

    public float tiempoDisolverTexto;
    public float tiempoDisolverSalida;

    public TextMeshProUGUI nombreNivel;

    public bool TransicionEnCurso { get; private set; } = false;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (disolverCanvas != null) disolverCanvas.alpha = 0f;
        if (disolverTexto != null) disolverTexto.alpha = 0f;

        string nombreEscena = SceneManager.GetActiveScene().name;
        DisolverEntrada(nombreEscena);

    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("LevelStart: " + scene.name);

        // Guardar el NivelActual si es un nivel jugable
        if (scene.buildIndex != LevelManager.Instance.SeleccionNiveles &&
            scene.buildIndex != LevelManager.Instance.final &&
            scene.name != "Menu" && scene.name != "Ayuda")
        {
            PlayerPrefs.SetInt("NivelActual", scene.buildIndex);
            PlayerPrefs.Save();
            Debug.Log("Guardando NivelActual: " + scene.buildIndex);
        }

        if (disolverCanvas != null)
        {
            disolverCanvas.alpha = 1f;
            DisolverEntrada(scene.name);
        }

        if (scene.name == "Victoria")
        {
            Debug.Log("GameComplete");
        }
    }



    private void DisolverEntrada(string nombreEscena)
    {
        TransicionEnCurso = true;

        disolverCanvas.alpha = 1f;
        disolverCanvas.blocksRaycasts = true;
        disolverCanvas.interactable = true;

        if (nombreEscena != "Menu" && nombreEscena != "Ayuda" && nombreEscena != "SeleccionNiveles" && nombreEscena != "Victoria")
        {
            nombreNivel.text = nombreEscena;

            // Aseguramos que el CanvasGroup del texto esté activo
            if (disolverTexto != null)
                disolverTexto.alpha = 1f;

            LeanTween.alphaCanvas(disolverTexto, 0f, tiempoDisolverTexto).setOnComplete(() =>
            {
                LeanTween.alphaCanvas(disolverCanvas, 0f, tiempoDisolverEntrada).setOnComplete(() =>
                {
                    disolverCanvas.blocksRaycasts = false;
                    disolverCanvas.interactable = false;
                    TransicionEnCurso = false;
                });
            });
        }
        else
        {
            LeanTween.alphaCanvas(disolverCanvas, 0f, tiempoDisolverEntrada).setOnComplete(() =>
            {
                disolverCanvas.blocksRaycasts = false;
                disolverCanvas.interactable = false;
                TransicionEnCurso = false;
            });
        }
    }
    public void Disolversalida(int IndexEscena)
    {
        if (IndexEscena <= 0 || IndexEscena >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Índice de escena inválido: " + IndexEscena);
            return;
        }
        TransicionEnCurso = true;
        disolverCanvas.blocksRaycasts = true;
        disolverCanvas.interactable = true;


        LeanTween.alphaCanvas(disolverCanvas, 1f, tiempoDisolverSalida).setOnComplete(() =>
        {
            SceneManager.LoadScene(IndexEscena);

        });
    }



}
