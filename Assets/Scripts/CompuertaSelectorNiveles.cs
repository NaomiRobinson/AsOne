using UnityEngine;
using Unity.Cinemachine;

public class CompuertaSelectorNiveles : MonoBehaviour
{
    public int grupoNiveles;
    private bool compuertaBloqueada = false;
    private Animator animador;

    public bool esUltimaCompuerta;

    private NivelSeleccionado nivelSeleccionado;

    void Start()
    {
        animador = GetComponent<Animator>();
        nivelSeleccionado = GetComponent<NivelSeleccionado>();

        if (nivelSeleccionado == null)
        {
            Debug.LogWarning($"El GameObject {gameObject.name} no tiene el componente NivelSeleccionado.");
        }

        RevisarCompuerta();
    }

    void OnEnable()
    {
        AnimacionCompletoGrupo.OnAnimacionGemasTerminada += RevisarCompuerta;
    }

    void OnDisable()
    {
        AnimacionCompletoGrupo.OnAnimacionGemasTerminada -= RevisarCompuerta;
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
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }


}