using UnityEngine;
using System.Collections;

public class AnimacionGemaCompuerta : MonoBehaviour
{
    public Transform jugador; // el jugador desde donde sale y a donde vuelve
    public Transform sensorCompuerta; // punto hacia donde debe volar
    public float velocidad = 5f;
    public float escalaMaxima = 1.5f;
    public float escalaMinima = 0.3f;
    public float tiempoEscala = 0.3f;

    private Vector3 posicionInicial;
    private bool yendoALaCompuerta = true;
    private bool regreso = false;
    private bool compuertaActivada = false;

    void Start()
    {
        posicionInicial = jugador.position - jugador.transform.right * 0.5f; // aparece detrás del jugador
        transform.position = posicionInicial;
        StartCoroutine(MovimientoBoomerang());
    }

    IEnumerator MovimientoBoomerang()
    {
        // 1️⃣ Mover hacia la compuerta
        while (yendoALaCompuerta)
        {
            transform.position = Vector3.MoveTowards(transform.position, sensorCompuerta.position, velocidad * Time.deltaTime);
            if (Vector3.Distance(transform.position, sensorCompuerta.position) < 0.05f)
            {
                yendoALaCompuerta = false;
                StartCoroutine(EscalaEfecto(true));
                ActivarCompuerta();
                yield return new WaitForSeconds(0.3f);
                regreso = true;
            }
            yield return null;
        }

        // 2️⃣ Regresar al jugador
        while (regreso)
        {
            transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * 1.5f * Time.deltaTime);
            if (Vector3.Distance(transform.position, jugador.position) < 0.1f)
            {
                StartCoroutine(EscalaEfecto(false));
                yield return new WaitForSeconds(0.3f);
                Destroy(gameObject); // desaparece
                break;
            }
            yield return null;
        }
    }

    IEnumerator EscalaEfecto(bool agrandar)
    {
        float t = 0;
        Vector3 escalaInicial = transform.localScale;
        Vector3 escalaObjetivo = agrandar ? Vector3.one * escalaMaxima : Vector3.one * escalaMinima;

        while (t < tiempoEscala)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(escalaInicial, escalaObjetivo, t / tiempoEscala);
            yield return null;
        }
    }

    void ActivarCompuerta()
    {
        if (compuertaActivada) return;
        compuertaActivada = true;

        CompuertaSelectorNiveles compuerta = sensorCompuerta.GetComponentInParent<CompuertaSelectorNiveles>();
        if (compuerta != null)
        {
            compuerta.RevisarCompuerta();
        }

        Debug.Log("✨ Gema activó la compuerta!");
    }
}