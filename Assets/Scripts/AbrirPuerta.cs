using UnityEngine;

public class AbrirPuerta : MonoBehaviour
{
    public enum TipoPalanca { Boton, Presion }
    public TipoPalanca tipo = TipoPalanca.Boton;

    public GameObject puertaAsociada;
    public GameObject palanca;

    public Transform puntoA;
    public Transform puntoB;
    public float speed = 2f;
    public bool moverEste = false; //control especifico


    private Vector3 target;

    private Animator animPuerta;
    private Animator animPalanca;

    private bool estaJugador = false;
    private bool puertaAbierta = false;
    private bool palancaActivada = false;
    private bool porton = false; //control general

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
            porton = puertaAbierta;

            AnimacionesControlador.SetBool(animPalanca, "estaActivada", palancaActivada);
            AnimacionesControlador.SetBool(animPuerta, "estaAbierta", puertaAbierta);
            if (gameObject.CompareTag("lever1"))
            {

                Debug.Log("La palanca ha sido activada.");
            }
        }

        
        
            if (!moverEste) return;
        Debug.Log("Portón: " + porton + " | Moviendo hacia: " + (porton ? "B" : "A"));

        // Elegir el punto de destino según el estado de la variable
        target = porton ? puntoB.position : puntoA.position;

            // Mover el rectángulo hacia el destino
            //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime));

        


    }
    


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || !other.CompareTag("Caja")) return;

        estaJugador = true;

        if (tipo == TipoPalanca.Presion)
        {
            puertaAbierta = true;
            porton = true;
            AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || !other.CompareTag("Caja")) return;

        estaJugador = false;

        if (tipo == TipoPalanca.Presion)
        {
            puertaAbierta = false;
            porton = false;
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
