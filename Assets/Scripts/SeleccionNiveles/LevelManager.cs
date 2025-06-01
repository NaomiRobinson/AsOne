using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public int[] nivelesGrupo1;  //REVISAR BIEN EL BUILD PROFILE
    public int[] nivelesGrupo2;
    public int[] nivelesGrupo3;


    [HideInInspector] public int grupoActual = 0;

    public int nivelTutorial; //Index de escena del tutorial
    public int final; //Index de escena de victoria

    public int SeleccionNiveles; //Index del selector de niveles

    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if ((nivelesGrupo1 == null || nivelesGrupo1.Length == 0) ||
                (nivelesGrupo2 == null || nivelesGrupo2.Length == 0) ||
                (nivelesGrupo3 == null || nivelesGrupo3.Length == 0))
            {
                Debug.LogWarning("¡Uno o más grupos de niveles no están configurados en el Inspector!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool EsUltimoNivel(int nivelActual)
    {
        int[] grupo = ObtenerGrupoActual();
        return grupo.Length > 0 && nivelActual == grupo[grupo.Length - 1];
    }

    public void SeleccionarGrupo(int grupo)
    {
        Debug.Log("Seleccionando grupo: " + grupo);
        grupoActual = grupo;

        Debug.Log("nivelesGrupo1: " + string.Join(",", nivelesGrupo1));
        Debug.Log("nivelesGrupo2: " + string.Join(",", nivelesGrupo2));
        Debug.Log("nivelesGrupo3: " + string.Join(",", nivelesGrupo3));

        int primerNivel = 0;

        switch (grupo)
        {
            case 1:
                primerNivel = nivelesGrupo1[0];
                break;
            case 2:
                primerNivel = nivelesGrupo2[0];
                break;
            case 3:
                primerNivel = nivelesGrupo3[0];
                break;
            default:
                Debug.LogError("Grupo inválido seleccionado: " + grupo);
                return;
        }
        Debug.Log("Cargando nivel con índice: " + primerNivel);
        TransicionEscena.Instance.Disolversalida(primerNivel);
    }

    private int[] ObtenerGrupoActual()
    {
        return grupoActual switch
        {
            1 => nivelesGrupo1,
            2 => nivelesGrupo2,
            3 => nivelesGrupo3,
            _ => new int[0],
        };
    }



    public int ObtenerSiguienteNivel(int nivelActual)
    {
        int[] grupo = ObtenerGrupoActual();
        Debug.Log("Grupo actual: " + grupoActual);
        Debug.Log("Niveles en el grupo: " + string.Join(",", grupo));

        for (int i = 0; i < grupo.Length - 1; i++)
        {
            if (grupo[i] == nivelActual)
                return grupo[i + 1];
        }

        Debug.LogWarning("Nivel actual no encontrado en el grupo");
        return 0;
    }

    public void CargarFinal()
    {
        TransicionEscena.Instance.Disolversalida(final);
    }

}


