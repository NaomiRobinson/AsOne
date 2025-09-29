using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Final : MonoBehaviour
{
    public GameObject jugador;
    public GameObject canvasFinal;  

    public float tiempoMostrarTexto = 2f;
    public float tiempoAntesDeMostrarBoton = 1f;

    private TMP_Text textoFinal;
    private Button botonVolverAlMenu;
    private bool activado = false;

    private void Start()
    {
        canvasFinal.SetActive(false);

        // Buscamos el texto y el botón dentro del Canvas para manejarlos
        textoFinal = canvasFinal.GetComponentInChildren<TMP_Text>();
        botonVolverAlMenu = canvasFinal.GetComponentInChildren<Button>();

        textoFinal.gameObject.SetActive(false);
        botonVolverAlMenu.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activado) return;

        if (other.CompareTag("JugadorIzq") || other.CompareTag("JugadorDer"))
        {
            activado = true;
            StartCoroutine(ProcesoFinal(other.gameObject));
        }
    }

    IEnumerator ProcesoFinal(GameObject jugadorActual)
    {
        // Desactivar movimiento
        MovimientoJugador movimiento = jugadorActual.GetComponent<MovimientoJugador>();
        if (movimiento) movimiento.puedeMoverse = false;

        var rb = jugadorActual.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // Activar Canvas completo
        canvasFinal.SetActive(true);


        textoFinal.gameObject.SetActive(true);

        yield return new WaitForSeconds(tiempoMostrarTexto);

        yield return new WaitForSeconds(tiempoAntesDeMostrarBoton);

        botonVolverAlMenu.gameObject.SetActive(true);
    }
}
