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

    void Awake()
    {
        animador = GetComponent<Animator>();

    }

    void Start()
    {
        animador = GetComponent<Animator>();
        nivelSeleccionado = GetComponent<NivelSeleccionado>();

        if (PlayerPrefs.GetInt($"CompuertaAbierta_{grupoNiveles}", 0) == 1)
        {
            AnimacionesControlador.SetBool(animador, "estaAbierta", true);
            generarImpulsoCamara = false;
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

    private void LanzarGemaAnimacion()
    {
        // Instanciamos la gema física después de la animación del grupo
        if (prefabGemaBoomerang != null && jugador != null && sensorCompuerta != null)
        {
            var gema = Instantiate(prefabGemaBoomerang, jugador.position, Quaternion.identity);
            var anim = gema.GetComponent<AnimacionGemaCompuerta>();
            anim.jugador = jugador;
            anim.sensorCompuerta = sensorCompuerta;

            Debug.Log("💎 Gema boomerang lanzada hacia la compuerta");
        }
        else
        {
            Debug.LogWarning("⚠️ Falta asignar el prefab o referencias en la compuerta.");
        }
    }



    private void ActivarImpulsoCamara()
    {
        if (!generarImpulsoCamara) return;

        var impulso = GetComponent<CinemachineImpulseSource>();
        if (impulso != null)
            impulso.GenerateImpulse();

        generarImpulsoCamara = false;
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
        {
            PlayerPrefs.SetInt($"CompuertaAbierta_{grupoNiveles}", 1);
        }
    }

    private IEnumerator ImpulsoConRetraso(float delay)
    {
        yield return new WaitForSeconds(delay * 0.1f); // 10% de la animación, ajustable
        ActivarImpulsoCamara();
    }

    private void AbrirCompuertaDespuesAnimacion()
    {
        Debug.Log($"✨ Animación de grupo completada. Lanzando gema hacia la compuerta {grupoNiveles}.");
        LanzarGemaAnimacion();
    }

}