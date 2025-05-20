using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public int[] nivelesGrupo1 = { 4, 5 };  //REVISAR BIEN EL BUILD PROFILE
    public int[] nivelesGrupo2 = { 6, 7 };
    public int grupoActual = 0;

    public int final = 8; //Index de escena de victoria

    public int SeleccionNiveles = 3; //Index del selector de niveles

    public static LevelManager Instance;

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

    public bool EsUltimoNivel(int nivelActual)
    {
        int[] grupo = grupoActual == 1 ? nivelesGrupo1 : nivelesGrupo2;
        return nivelActual == grupo[grupo.Length - 1];
    }

    public void SeleccionarGrupo(int grupo)
    {
        grupoActual = grupo;

        int primerNivel = 0;

        switch (grupo)
        {
            case 1:
                primerNivel = nivelesGrupo1[0];
                break;
            case 2:
                primerNivel = nivelesGrupo2[0];
                break;
        }

        TransicionEscena.Instance.Disolversalida(primerNivel);
    }

    private int[] ObtenerGrupoActual()
    {
        return grupoActual switch
        {
            1 => nivelesGrupo1,
            2 => nivelesGrupo2,
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


