using UnityEngine;

public class ChequeoLlaves
{

    private static readonly string[] idsLlaves = { "grupo1", "grupo2", "grupo3", "grupo4" };

    private const int FragmentosGrupo = 7;

    public static void AgregarFragmento(string idGrupo, string idNivel)
    {
        
        if (PlayerPrefs.GetInt("fragmentoRecolectado_" + idNivel, 0) == 1)
        {
            Debug.Log($"El fragmento de {idNivel} ya fue recolectado antes.");
            return; 
        }

        
        PlayerPrefs.SetInt("fragmentoRecolectado_" + idNivel, 1);

        
        int fragmentos = PlayerPrefs.GetInt("fragmentos_" + idGrupo, 0);
        fragmentos++;
        PlayerPrefs.SetInt("fragmentos_" + idGrupo, fragmentos);

        Debug.Log($"Fragmento añadido al {idGrupo}. Total: {fragmentos}");

        if (fragmentos >= FragmentosGrupo)
        {
            DesbloquearLlave(idGrupo);
        }
    }


    private static void DesbloquearLlave(string idGrupo)
    {
        PlayerPrefs.SetInt("llave_" + idGrupo, 1);
        Debug.Log($"¡Llave del {idGrupo} desbloqueada!");
    }


    public static bool TodasRecolectadas()
    {
        foreach (string id in idsLlaves)
        {
            if (PlayerPrefs.GetInt("llave_" + id, 0) != 1)
                return false;
        }
        return true;
    }

    public static int LlavesFaltantes()
    {
        int faltantes = 0;
        foreach (string id in idsLlaves)
        {
            if (PlayerPrefs.GetInt("llave_" + id, 0) != 1)
                faltantes++;
        }
        return faltantes;
    }
}
