using UnityEngine;

public class AbrirCompuerta : MonoBehaviour
{
    public enum TipoPalanca { Boton, Presion }
    public TipoPalanca tipo = TipoPalanca.Boton;
    public GameObject compuertaAsociada;
    public GameObject palanca;
    // public Transform puntoA;
    // public Transform puntoB;
    // public float speed = 2f;

    private Animator animPalanca;
    private Animator animCompuerta;

    private Vector3 target;

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
        if (tipo == TipoPalanca.Boton && estaJugador && Input.GetKeyDown(KeyCode.Space))
        {
            palancaActivada = !palancaActivada;
            compuertaAbierta = !compuertaAbierta;

            ActualizarEstado();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.CompareTag("Player") || other.CompareTag("Caja"))) return;

        estaJugador = true;

        if (tipo == TipoPalanca.Presion)
        {
            compuertaAbierta = true;
            ActualizarEstado();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!(other.CompareTag("Player") ||  other.CompareTag("Caja"))) return;

        estaJugador = false;

        if (tipo == TipoPalanca.Presion)
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
