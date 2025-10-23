using UnityEngine;
using System.Collections;

public class AnimacionGemaCompuerta : MonoBehaviour
{
    public Transform objetivo;
    public Transform jugador;
    private Transform sensor;

    private bool enMovimiento = false;
    public float velocidad = 8f;
    private bool yendoAlSensor = true;
    private bool esperando = false;

    private CompuertaSelectorNiveles compuerta;

    public float amplitudFlotacion = 0.15f;
    public float velocidadFlotacion = 2f;
    private Vector3 posicionBase;

    public void Inicializar(Transform jugador, Transform sensor, CompuertaSelectorNiveles compuerta)
    {
        this.jugador = jugador;
        this.sensor = sensor;
        this.objetivo = sensor;
        this.compuerta = compuerta;
    }

    void Update()
    {
        if (objetivo == null) return;

        if (esperando)
        {
            transform.position = posicionBase + Vector3.up * Mathf.Sin(Time.time * velocidadFlotacion) * amplitudFlotacion;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, objetivo.position, velocidad * Time.unscaledDeltaTime);

        if (Vector3.Distance(transform.position, objetivo.position) < 0.05f)
        {
            if (yendoAlSensor)
            {
                yendoAlSensor = false;
                StartCoroutine(SecuenciaEnSensor());
            }
            else
            {
                StartCoroutine(DestruirConRetraso());
            }
        }
    }

    private IEnumerator SecuenciaEnSensor()
    {
        esperando = true;
        posicionBase = transform.position;

        yield return new WaitForSeconds(1f);

        compuerta.GemaLlegoAlSensor();

        yield return new WaitForSeconds(1f);

        esperando = false;
        objetivo = jugador;
        velocidad *= 1.2f;
    }

    private IEnumerator DestruirConRetraso()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}