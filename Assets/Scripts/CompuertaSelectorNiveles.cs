using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CompuertaSelectorNiveles : MonoBehaviour
{
    public int grupoNiveles;
    private bool compuertaBloqueada = false;
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

        // Si ya estaba abierta de antes
        if (PlayerPrefs.GetInt($"CompuertaAbierta_{grupoNiveles}", 0) == 1)
        {
            AnimacionesControlador.SetBool(animador, "estaAbierta", true);
            generarImpulsoCamara = false;
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
            anim.Inicializar(jugador, sensorCompuerta);
            Debug.Log($"💎 Gema lanzada hacia compuerta {grupoNiveles}");
        }
        else
        {
            Debug.LogWarning($"⚠️ Faltan referencias en compuerta {grupoNiveles}");
        }
    }

    // 🔹 Llamado por la gema al llegar al sensor
    public void GemaLlegoAlSensor()
    {
        Debug.Log($"💥 La gema llegó al sensor de la compuerta {grupoNiveles} → abriendo compuerta...");
        RevisarCompuerta();
        StartCoroutine(ImpulsoConRetraso(animador.GetCurrentAnimatorStateInfo(0).length));
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
        yield return new WaitForSeconds(delay * 0.1f); // 10% del tiempo de animación
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
}