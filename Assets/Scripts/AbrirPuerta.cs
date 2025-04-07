using UnityEngine;

public class AbrirPuerta : MonoBehaviour
{
    public enum TipoPalanca { Boton, Presion }
    public TipoPalanca tipo = TipoPalanca.Boton;
    public GameObject puertaAsociada;

    private bool estaJugador = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tipo == TipoPalanca.Boton && estaJugador && Input.GetKeyDown(KeyCode.Space))
        {

            puertaAsociada.SetActive(!puertaAsociada.activeSelf);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        estaJugador = true;

        if (tipo == TipoPalanca.Presion)
        {
            puertaAsociada.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Jugador")) return;

        estaJugador = false;

        if (tipo == TipoPalanca.Presion)
        {
            puertaAsociada.SetActive(false);
        }
    }

}
