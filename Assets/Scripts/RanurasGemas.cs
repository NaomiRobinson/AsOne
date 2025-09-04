using UnityEngine;

public class RanurasGemas : MonoBehaviour
{

    [System.Serializable]
    public class Grupo
    {
        public string idGrupo;
        public Transform contenedorRanuras; // Objeto vacío con todas las ranuras
        public Transform contenedorGemas;   // Objeto vacío con todas las gemas que reemplazan las ranuras
        [HideInInspector] public GameObject[] ranuras;
        [HideInInspector] public GameObject[] gemas;
    }

    public Grupo[] grupos;

    void Start()
    {
        // Inicializar arrays automáticamente
        foreach (var g in grupos)
        {
            int cantidad = Mathf.Min(g.contenedorRanuras.childCount, g.contenedorGemas.childCount);

            g.ranuras = new GameObject[cantidad];
            g.gemas = new GameObject[cantidad];

            for (int i = 0; i < cantidad; i++)
            {
                g.ranuras[i] = g.contenedorRanuras.GetChild(i).gameObject;
                g.gemas[i] = g.contenedorGemas.GetChild(i).gameObject;
            }
        }

        ActualizarRanuras();
    }

    public void ActualizarRanuras()
    {
        foreach (var g in grupos)
        {
            bool estado = PlayerPrefs.GetInt($"GrupoCompletado_{g.idGrupo}", 0) == 1;

            for (int i = 0; i < g.ranuras.Length; i++)
            {
                g.ranuras[i].SetActive(!estado);
                g.gemas[i].SetActive(estado);
            }
        }
    }
}