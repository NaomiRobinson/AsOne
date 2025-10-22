using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CompuertaSelectorNiveles : MonoBehaviour
{
    public int grupoNiveles;
    private bool compuertaBloqueada = false;
    private bool compuertaYaAbierta = false;
    private Animator animador;

    public bool esUltimaCompuerta;
    bool generarImpulsoCamara = true;

    public GameObject prefabGemaBoomerang;
    public Transform jugador;
    public Transform sensorCompuerta;

    private NivelSeleccionado nivelSeleccionado;

    public static event System.Action<int> OnAnimacionGemasTerminada;

    void Awake()
    {
        animador = GetComponent<Animator>();
    }

    void Start()
    {
        nivelSeleccionado = GetComponent<NivelSeleccionado>();

        if (PlayerPrefs.GetInt($"CompuertaAbierta_{grupoNiveles}", 0) == 1)
        {
            AnimacionesControlador.SetBool(animador, "estaAbierta", true);
            generarImpulsoCamara = false;
            compuertaYaAbierta = true;
            Debug.Log($"✅ Compureta {grupoNiveles} abierta desde PlayerPrefs");
        }
    }

    void OnEnable()
    {
        AnimacionCompletoGrupo.OnAnimacionGemasTerminada += AbrirCompuertaDespuesAnimacion;
    }

    void OnDisable()
    {
        AnimacionCompletoGrupo.OnAnimacionGemasTerminada -= AbrirCompuertaDespuesAnimacion;
    }

    private void AbrirCompuertaDespuesAnimacion(int grupoTerminado)
    {
        if (grupoTerminado == grupoNiveles)
        {
            if (compuertaYaAbierta)
            {
                Debug.Log($"⛔ Compureta {grupoNiveles} ya abierta, no lanzo gema.");
                return;
            }

            Debug.Log($"✨ Grupo {grupoTerminado} completado → lanzando gema hacia compuerta {grupoNiveles}");
            LanzarGemaAnimacion();
        }
    }

    public void LanzarGemaAnimacion()
    {
        if (prefabGemaBoomerang != null && jugador != null && sensorCompuerta != null)
        {
            var gema = Instantiate(prefabGemaBoomerang, jugador.position, Quaternion.identity);
            var anim = gema.GetComponent<AnimacionGemaCompuerta>();
            anim.Inicializar(jugador, sensorCompuerta, this);
            Debug.Log($"💎 Gema lanzada hacia compuerta {grupoNiveles}");
        }
        else
        {
            Debug.LogWarning($"⚠️ Faltan referencias en compuerta {grupoNiveles}");
        }
    }

    public void GemaLlegoAlSensor()
    {
        Debug.Log($"💥 La gema llegó al sensor de la compuerta {grupoNiveles} → abriendo compuerta...");
        RevisarCompuerta();
        StartCoroutine(ImpulsoConRetraso(animador.GetCurrentAnimatorStateInfo(0).length));
        compuertaYaAbierta = true; // 🔹 MARCAMOS DEFINITIVAMENTE ABIERTA
    }

    public void RevisarCompuerta()
    {
        compuertaBloqueada = false;

        if (esUltimaCompuerta)
        {
            if (!NivelSeleccionado.TodosLosGruposCompletados())
                compuertaBloqueada = true;
        }
        else
        {
            if (grupoNiveles > LevelManager.Instance.grupoDesbloqueado)
                compuertaBloqueada = true;
        }

        AnimacionesControlador.SetBool(animador, "estaAbierta", !compuertaBloqueada);

        if (!compuertaBloqueada)
            PlayerPrefs.SetInt($"CompuertaAbierta_{grupoNiveles}", 1);
    }

    private IEnumerator ImpulsoConRetraso(float delay)
    {
        yield return new WaitForSeconds(delay * 0.1f);
        ActivarImpulsoCamara();
    }

    private void ActivarImpulsoCamara()
    {
        if (!generarImpulsoCamara) return;

        var impulso = GetComponent<CinemachineImpulseSource>();
        if (impulso != null)
            impulso.GenerateImpulse();

        generarImpulsoCamara = false;
    }
    public float GetDuracionApertura()
    {
        // 🔹 Si tu animador tiene una animación de apertura llamada "Abrir", podés hacer:
        var clipInfo = animador.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
            return clipInfo[0].clip.length;

        // 🔹 Si no, podés devolver un tiempo fijo (por ejemplo 1 segundo)
        return 1f;
    }


}