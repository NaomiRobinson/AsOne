using UnityEngine;
using Unity.Cinemachine;

public class CompuertaSelectorNiveles : MonoBehaviour
{
    public int grupoNiveles;
    private bool compuertaBloqueada = false;
    private Animator animador;

    public bool esUltimaCompuerta;
    public bool generarImpulsoCamara;

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
        }
        else
        {
            RevisarCompuerta();
        }
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

        if (!compuertaBloqueada)
        {
            PlayerPrefs.SetInt($"CompuertaAbierta_{grupoNiveles}", 1);

            if (generarImpulsoCamara)
            {
                var impulso = GetComponent<CinemachineImpulseSource>();
                if (impulso != null)
                    impulso.GenerateImpulse();
            }
        }
    }





}