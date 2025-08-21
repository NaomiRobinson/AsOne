using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public int[] nivelesGrupo1;  //REVISAR BIEN EL BUILD PROFILE
    public int[] nivelesGrupo2;
    public int[] nivelesGrupo3;


    [HideInInspector] public int grupoActual = 0;

    public int nivelTutorial; //Index de escena del tutorial
    public int final; //Index de escena de victoria

    public int SeleccionNiveles; //Index del selector de niveles

    public int grupoDesbloqueado = 1;

    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            grupoDesbloqueado = PlayerPrefs.GetInt("GrupoDesbloqueado", 1);

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
        Debug.Log($"EsUltimoNivel: grupoActual={grupoActual}, niveles en grupo={string.Join(",", grupo)}, nivelActual={nivelActual}");
        if (grupo.Length == 0) return false;

        return nivelActual == grupo[grupo.Length - 1];
    }
    public void SeleccionarGrupo(int grupo)
    {
        if (grupo > grupoDesbloqueado)
        {
            Debug.LogWarning($"¡No se puede seleccionar el grupo {grupo} porque el grupo anterior no está completo! Grupo desbloqueado actual: {grupoDesbloqueado}");
            return;
        }

        Debug.Log("Seleccionando grupo: " + grupo);
        grupoActual = grupo;
        Debug.Log("grupoActual seteado a: " + grupoActual);

        Debug.Log("nivelesGrupo1: " + string.Join(",", nivelesGrupo1));
        Debug.Log("nivelesGrupo2: " + string.Join(",", nivelesGrupo2));
        Debug.Log("nivelesGrupo3: " + string.Join(",", nivelesGrupo3));

        int primerNivel = grupo switch
        {
            1 => nivelesGrupo1[0],
            2 => nivelesGrupo2[0],
            3 => nivelesGrupo3[0],
            _ => -1,
        };

        if (primerNivel == -1)
        {
            Debug.LogError("Grupo inválido seleccionado: " + grupo);
            return;
        }

        Debug.Log($"Cargando primer nivel del grupo {grupo}: {primerNivel}");
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
            {
                Debug.Log($"Nivel actual {nivelActual} encontrado en el índice {i}, siguiente nivel: {grupo[i + 1]}");
                return grupo[i + 1];
            }
        }

        Debug.LogWarning("Nivel actual no encontrado en el grupo");
        return 0;
    }

    public void MarcarGrupoCompletado()
    {
        int nuevoGrupo = grupoActual + 1;
        Debug.Log($"Intentando desbloquear nuevo grupo: {nuevoGrupo} (actual desbloqueado: {grupoDesbloqueado})");

        if (nuevoGrupo > grupoDesbloqueado)
        {
            grupoDesbloqueado = nuevoGrupo;
            PlayerPrefs.SetInt("GrupoDesbloqueado", grupoDesbloqueado);
            PlayerPrefs.Save();
            Debug.Log("Nuevo grupo desbloqueado y guardado: " + grupoDesbloqueado);
        }
        else
        {
            Debug.Log("No se desbloqueó ningún grupo nuevo porque ya estaba desbloqueado.");
        }
    }

    public void CargarFinal()
    {
        TransicionEscena.Instance.Disolversalida(final);
    }

    public bool EsPrimerNivel(int buildIndex)
    {
        if (nivelesGrupo1.Length > 0 && nivelesGrupo1[0] == buildIndex) return true;
        if (nivelesGrupo2.Length > 0 && nivelesGrupo2[0] == buildIndex) return true;
        if (nivelesGrupo3.Length > 0 && nivelesGrupo3[0] == buildIndex) return true;
        return false;
    }

    public int ObtenerNivelAnterior(int buildIndex)
    {
        // Buscar en grupo 1
        int idx = System.Array.IndexOf(nivelesGrupo1, buildIndex);
        if (idx > 0) return nivelesGrupo1[idx - 1];

        // Buscar en grupo 2
        idx = System.Array.IndexOf(nivelesGrupo2, buildIndex);
        if (idx > 0) return nivelesGrupo2[idx - 1];

        // Buscar en grupo 3
        idx = System.Array.IndexOf(nivelesGrupo3, buildIndex);
        if (idx > 0) return nivelesGrupo3[idx - 1];

        // Si no lo encuentra o está en primera posición → se queda en el mismo
        return buildIndex;
    }
}


