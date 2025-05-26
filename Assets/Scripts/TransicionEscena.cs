using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
using Unity.Services.Core;
using static EventManager;
using static StaticVariables;

public class TransicionEscena : MonoBehaviour
{
    public static TransicionEscena Instance;

    //Disolver
    public CanvasGroup disolverCanvasGroup;
    public float tiempoDisolverEntrada;
    public float tiempoDisolverSalida;



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
        DisolverEntrada();

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

        if (disolverCanvasGroup != null)
        {
            disolverCanvasGroup.alpha = 1f;
            DisolverEntrada();
        }


        // LevelStartEvent LevelStart = new LevelStartEvent
        // { 
        //     level = SessionData.level
        // };

        //AnalyticsService.Instance.RecordEvent(LevelStart);


        if (scene.name == "Victoria")
        {
            Debug.Log("GameComplete");
        }
    }


    private void DisolverEntrada()
    {
        LeanTween.alphaCanvas(disolverCanvasGroup, 0f, tiempoDisolverEntrada).setOnComplete(() =>
        {
            disolverCanvasGroup.blocksRaycasts = false;
            disolverCanvasGroup.interactable = false;
        });
    }

    public void Disolversalida(int IndexEscena)
    {
        if (IndexEscena <= 0 || IndexEscena >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Índice de escena inválido: " + IndexEscena);
            return;
        }
        disolverCanvasGroup.blocksRaycasts = true;
        disolverCanvasGroup.interactable = true;

        LeanTween.alphaCanvas(disolverCanvasGroup, 1f, tiempoDisolverSalida).setOnComplete(() =>
        {
            SceneManager.LoadScene(IndexEscena);

        });
    }


}
