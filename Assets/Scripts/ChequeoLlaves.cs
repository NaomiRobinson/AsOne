using UnityEngine;

public class ChequeoLlaves
{

    private static readonly string[] idsLlaves = { "grupo1", "grupo2" };

    public static bool TodasRecolectadas()
    {
        foreach (string id in idsLlaves)
        {
            if (PlayerPrefs.GetInt("llave_" + id, 0) != 1)
                return false;
        }
        return true;
    }
}
