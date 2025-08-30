using UnityEngine;

public class RanurasGemas : MonoBehaviour
{
    [System.Serializable]
    public class Ranura
    {
        public string idLlave;       // Ej: "grupo1", "grupo2"
        public GameObject ranuraVacia; // Objeto que representa la ranura vacía
        public GameObject gemaLlena;   // Objeto que representa la gema puesta
    }

    public Ranura[] ranuras;

    void Start()
    {
        ActualizarRanuras();
    }

    public void ActualizarRanuras()
    {

        foreach (var r in ranuras)
        {
            r.gemaLlena.SetActive(false);
            r.ranuraVacia.SetActive(false);
            int estado = PlayerPrefs.GetInt("llave_" + r.idLlave, 0);

            if (estado == 1)
            {
                r.gemaLlena.SetActive(true);
                r.ranuraVacia.SetActive(false);
            }
            else
            {
                r.gemaLlena.SetActive(false);
                r.ranuraVacia.SetActive(true);
            }
        }
    }
}
