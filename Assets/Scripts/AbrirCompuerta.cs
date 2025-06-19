using UnityEngine;

public class AbrirCompuerta : MonoBehaviour
{
    public enum TipoPalanca { Boton, Presion }
    public TipoPalanca tipo = TipoPalanca.Boton;
    public GameObject compuertaAsociada;
    public GameObject palanca;


    private Animator animPalanca;
    private Animator animCompuerta;

    private int objetosEnPresion = 0;

    private bool estaJugador = false;
    private bool compuertaAbierta = false;
    private bool palancaActivada = false;

    void Start()
    {
        animPalanca = palanca?.GetComponent<Animator>();
        animCompuerta = compuertaAsociada?.GetComponent<Animator>();
    }

    void Update()
    {
        if (tipo == TipoPalanca.Boton && estaJugador && Input.GetKeyDown(KeyCode.Space) && !palancaActivada)
        {
            palancaActivada = true;
            compuertaAbierta = true;
            ActualizarEstado();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.CompareTag("JugadorIzq") || other.CompareTag("JugadorDer") || other.CompareTag("Caja"))) return;
        estaJugador = true;
        objetosEnPresion++;

        if (tipo == TipoPalanca.Presion && objetosEnPresion > 0)
        {
            compuertaAbierta = true;
            ActualizarEstado();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!(other.CompareTag("JugadorIzq") || other.CompareTag("JugadorDer") || other.CompareTag("Caja"))) return;
        estaJugador = false;
        objetosEnPresion = Mathf.Max(0, objetosEnPresion - 1); // por seguridad, no baja de cero

        if (tipo == TipoPalanca.Presion && objetosEnPresion == 0)
        {
            compuertaAbierta = false;
            ActualizarEstado();
        }
    }

    public bool EstaAbierta() => compuertaAbierta;

    public void Cerrar()
    {
        compuertaAbierta = false;
        ActualizarEstado();
    }
    public void Abrir()
    {
        compuertaAbierta = true;
        ActualizarEstado();

    }

    private void ActualizarEstado()
    {
        if (animPalanca != null)
        {
            AnimacionesControlador.SetBool(animPalanca, "estaActivada", compuertaAbierta);
        }

        if (animCompuerta != null)
        {
            AnimacionesControlador.SetBool(animCompuerta, "estaAbierta", compuertaAbierta);
        }
    }

}
