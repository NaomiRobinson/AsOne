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
    }


    public float tiempoAnim = 0.5f;

    public static event System.Action OnAnimacionGemasTerminada;

    void Start()
    {
        StartCoroutine(EsperarTransicionYMostrar());
    }

    private IEnumerator EsperarTransicionYMostrar()
    {
        // Esperar hasta que TransicionEscena haya terminado
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

        // Solo desactivar fragmentos y gemas de todos los grupos
        foreach (var grupo in grupos)
        {
            foreach (var f in grupo.fragmentos)
                if (f != null) f.SetActive(false);
            if (grupo.gemaCompleta != null) grupo.gemaCompleta.SetActive(false);
        }

        StartCoroutine(AnimarGrupoCoroutine(g));
    }
    private IEnumerator AnimarGrupoCoroutine(GrupoFragmentos g)
    {
        foreach (var f in g.fragmentos)
        {
            if (f != null)
            {
                // 🔹 Guardar escala original antes de tocarla
                Vector3 escalaOriginal = f.transform.localScale;

                // 🔹 Empezar desde cero
                f.transform.localScale = Vector3.zero;
                f.SetActive(true);

                Debug.Log($"[AnimarGrupo] Animando fragmento {f.name}");

                LeanTween.scale(f, escalaOriginal, tiempoAnim)
                    .setEaseOutBack()
                    .setIgnoreTimeScale(true);

                yield return new WaitForSecondsRealtime(tiempoAnim);
            }
        }

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
        CerrarAnimacion();
    }

    public void CerrarAnimacion()
    {
        Debug.Log("[CerrarAnimacion] Cerrando animación → desactivando panel y objetos.");

        panelOscuro.SetActive(false);
        Time.timeScale = 1f;

        foreach (var grupo in grupos)
        {
            foreach (var f in grupo.fragmentos)
            {
                if (f != null) f.SetActive(false);
            }
            if (grupo.gemaCompleta != null)
                grupo.gemaCompleta.SetActive(false);
        }

        Debug.Log("[CerrarAnimacion] Finalizado, todo desactivado y juego reanudado.");
        OnAnimacionGemasTerminada?.Invoke();

    }
}
