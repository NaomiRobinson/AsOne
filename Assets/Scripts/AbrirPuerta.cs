using UnityEngine;

public class AbrirPuerta : MonoBehaviour
{
    public enum TipoPalanca { Boton, Presion }
    public TipoPalanca tipo = TipoPalanca.Boton;
    public GameObject puertaAsociada;


    public Sprite palanca;
    public Sprite palancaInclinada;
    public Sprite puertaCerrada;
    public Sprite puertaAbierta;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer puertaRenderer;
    private bool estaJugador = false;

    private bool puertaAbiertaEstado = false;

    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (puertaAsociada != null)
        {
            puertaRenderer = puertaAsociada.GetComponent<SpriteRenderer>();
        }

        ActualizarSprites();
    }


    // Update is called once per frame
    void Update()
    {
        if (tipo == TipoPalanca.Boton && estaJugador && Input.GetKeyDown(KeyCode.Space))
        {

            puertaAbiertaEstado = !puertaAbiertaEstado;
            puertaAsociada.SetActive(puertaAbiertaEstado);
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
            puertaAsociada.SetActive(true);
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
            puertaAsociada.SetActive(false);
            ActualizarSprites();
        }
    }

    void ActualizarSprites()
    {

        spriteRenderer.sprite = puertaAbiertaEstado ? palancaInclinada : palanca;

        if (puertaRenderer != null)
        {
            puertaRenderer.sprite = puertaAbiertaEstado ? puertaAbierta : puertaCerrada;
        }
    }

    public bool EstaAbierta()
    {
        return puertaAbiertaEstado;
    }

    public void ForzarAbrir()
{
    puertaAbiertaEstado = true;

    if (puertaAsociada != null)
    {
        puertaAsociada.SetActive(true);
    }

    ActualizarSprites();
}

}
