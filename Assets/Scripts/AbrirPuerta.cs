using UnityEngine;

public class AbrirPuerta : MonoBehaviour
{
    public enum TipoPalanca { Boton, Presion }
    public TipoPalanca tipo = TipoPalanca.Boton;

    public GameObject puertaAsociada;
    public GameObject palanca;

    private Animator animPuerta;
    private Animator animPalanca;

    private bool estaJugador = false;
    private bool puertaAbierta = false;
    private bool palancaActivada = false;

    void Start()
    {
        animPuerta = puertaAsociada?.GetComponent<Animator>();
        animPalanca = palanca?.GetComponent<Animator>();
    }

    void Update()
    {
        if (tipo == TipoPalanca.Boton && estaJugador && Input.GetKeyDown(KeyCode.Space))
        {
            palancaActivada = !palancaActivada;
            puertaAbierta = !puertaAbierta;

            AnimacionesControlador.SetBool(animPalanca, "estaActivada", palancaActivada);
            AnimacionesControlador.SetBool(animPuerta, "estaAbierta", puertaAbierta);
            if (gameObject.CompareTag("lever1"))
            {

                Debug.Log("La palanca ha sido activada.");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("JugadorIzq") || other.CompareTag("JugadorDer") ) return;

        estaJugador = true;

        if (tipo == TipoPalanca.Presion)
        {
            puertaAbierta = true;
            AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("JugadorIzq") || other.CompareTag("JugadorDer")) return;

        estaJugador = false;

        if (tipo == TipoPalanca.Presion)
        {
            puertaAbierta = false;
            AnimacionesControlador.SetBool(animPuerta, "estaAbierta", false);
        }
    }

    public bool EstaAbierta() => puertaAbierta;

    public void Abrir()
    {
        puertaAbierta = true;
        AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);
    }
    
    
}
