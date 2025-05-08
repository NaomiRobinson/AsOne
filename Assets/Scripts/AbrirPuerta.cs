using UnityEngine;

public class AbrirPuerta : MonoBehaviour
{
    public enum TipoPalanca { Boton, Presion }
    public TipoPalanca tipo = TipoPalanca.Boton;
    public GameObject puertaAsociada;


    public Sprite palanca;
    public Sprite palancaInclinada;


    private SpriteRenderer spriteRenderer;
    private Animator puertaAnimator;
    private bool estaJugador = false;

    private bool puertaAbiertaEstado = false;

    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (puertaAsociada != null)
        {
            puertaAnimator = puertaAsociada.GetComponent<Animator>();
        }


        ActualizarSprites();
    }


    // Update is called once per frame
    void Update()
    {
        if (tipo == TipoPalanca.Boton && estaJugador && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Interactuo con palanca");
            puertaAbiertaEstado = !puertaAbiertaEstado;
            if (puertaAnimator != null)
            {
                puertaAnimator.SetBool("estaAbierta", puertaAbiertaEstado);
            }
            ActualizarSprites();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Jugador entró al trigger");
        estaJugador = true;

        if (tipo == TipoPalanca.Presion)
        {
            puertaAbiertaEstado = true;
            if (puertaAnimator != null)
            {
                puertaAnimator.SetBool("estaAbierta", true);
            }
            ActualizarSprites();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        estaJugador = false;

        if (tipo == TipoPalanca.Presion)
        {
            puertaAbiertaEstado = false;
            if (puertaAnimator != null)
            {
                puertaAnimator.SetBool("estaAbierta", false);
            }
            ActualizarSprites();
        }
    }

    void ActualizarSprites()
    {

        spriteRenderer.sprite = puertaAbiertaEstado ? palancaInclinada : palanca;
    }

    public bool EstaAbierta()
    {
        return puertaAbiertaEstado;
    }

    public void ForzarAbrir()
    {
        puertaAbiertaEstado = true;
        if (puertaAnimator != null)
        {
            puertaAnimator.SetBool("estaAbierta", true);
        }
        ActualizarSprites();
    }

}
