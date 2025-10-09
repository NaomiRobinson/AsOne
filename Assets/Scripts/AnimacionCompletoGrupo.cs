using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class AnimacionCompletoGrupo : MonoBehaviour
{
    public GameObject panelOscuro;
    public GrupoFragmentos[] grupos;
    [System.Serializable]
    public class GrupoFragmentos
    {
        public string idLlave;

        public GameObject[] fragmentos;

        public GameObject gemaCompleta;
        public CompuertaSelectorNiveles compuertaJugador1;
        public CompuertaSelectorNiveles compuertaJugador2;
    }

    public CompuertaSelectorNiveles[] compuertas;
    public float tiempoAnim = 0.5f;

    public static event System.Action<int> OnAnimacionGemasTerminada;

    void Start()
    {
        StartCoroutine(EsperarTransicionYMostrar());
    }

    private IEnumerator EsperarTransicionYMostrar()
    {
        while (TransicionEscena.Instance != null && TransicionEscena.Instance.TransicionEnCurso)
            yield return null;

        foreach (var g in grupos)
        {
            int completado = PlayerPrefs.GetInt($"GrupoCompletado_{g.idLlave}", 0);
            int animMostrada = PlayerPrefs.GetInt($"AnimMostrada_{g.idLlave}", 0);

            if (completado == 1 && animMostrada == 0)
            {
                MostrarAnimacionGrupo(g);
                PlayerPrefs.SetInt($"AnimMostrada_{g.idLlave}", 1);
                PlayerPrefs.Save();
                yield break;
            }
        }

        panelOscuro.SetActive(false);
    }

    void MostrarAnimacionGrupo(GrupoFragmentos g)
    {
        panelOscuro.SetActive(true);
        Time.timeScale = 0f;

        // Ocultar todas las gemas y fragmentos
        foreach (var grupo in grupos)
        {
            foreach (var f in grupo.fragmentos)
                if (f != null) f.SetActive(false);
            if (grupo.gemaCompleta != null)
                grupo.gemaCompleta.SetActive(false);
        }

        StartCoroutine(AnimarGrupoCoroutine(g));
    }

    private IEnumerator AnimarGrupoCoroutine(GrupoFragmentos g)
    {
        // Aparecen los fragmentos uno a uno
        foreach (var f in g.fragmentos)
        {
            if (f != null)
            {
                Vector3 escalaOriginal = f.transform.localScale;
                f.transform.localScale = Vector3.zero;
                f.SetActive(true);

                Debug.Log($"[AnimarGrupo] Animando fragmento {f.name}");

                LeanTween.scale(f, escalaOriginal, tiempoAnim)
                    .setEaseOutBack()
                    .setIgnoreTimeScale(true);

                yield return new WaitForSecondsRealtime(tiempoAnim);
            }
        }

        // Aparece la gema completa
        if (g.gemaCompleta != null)
        {
            Vector3 escalaOriginal = g.gemaCompleta.transform.localScale;
            g.gemaCompleta.transform.localScale = Vector3.zero;
            g.gemaCompleta.SetActive(true);

            LeanTween.scale(g.gemaCompleta, escalaOriginal, tiempoAnim)
                .setEaseOutBack()
                .setIgnoreTimeScale(true);

            yield return new WaitForSecondsRealtime(tiempoAnim);
        }

        Debug.Log("[AnimarGrupo] Animación completa → cerrando");
        CerrarAnimacion(g);
    }

    private void CerrarAnimacion(GrupoFragmentos g)
    {
        panelOscuro.SetActive(false);
        Time.timeScale = 1f;

        foreach (var f in g.fragmentos)
            if (f != null) f.SetActive(false);
        if (g.gemaCompleta != null)
            g.gemaCompleta.SetActive(false);

        // 🔹 Lanzar gema hacia ambas compuertas
        if (g.compuertaJugador1 != null)
            g.compuertaJugador1.LanzarGemaAnimacion();

        if (g.compuertaJugador2 != null)
            g.compuertaJugador2.LanzarGemaAnimacion();

        // ⚡ NUEVO: avisar a las compuertas que el grupo terminó
        int grupoTerminado = 0;
        if (int.TryParse(g.idLlave, out grupoTerminado))
        {
            Debug.Log($"📣 Evento OnAnimacionGemasTerminada lanzado para grupo {grupoTerminado}");
            OnAnimacionGemasTerminada?.Invoke(grupoTerminado);
        }
    }
}
