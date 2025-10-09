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

    private CompuertaSelectorNiveles compuerta;

    public void Inicializar(Transform jugador, Transform sensor)
    {
        this.jugador = jugador;
        this.sensor = sensor;
        this.objetivo = sensor;

        compuerta = sensor.GetComponentInParent<CompuertaSelectorNiveles>();
    }

    void Update()
    {
        if (objetivo == null) return;

        transform.position = Vector3.MoveTowards(transform.position, objetivo.position, velocidad * Time.unscaledDeltaTime);

        // Si llegó al objetivo actual
        if (Vector3.Distance(transform.position, objetivo.position) < 0.05f)
        {
            if (yendoAlSensor)
            {
                // Llamar a la compuerta para abrirla
                compuerta.GemaLlegoAlSensor();

                // Ahora la gema vuelve al jugador
                yendoAlSensor = false;
                objetivo = jugador;

                // Puedes hacer que vuelva más rápido o más lento
                velocidad *= 1.2f;
            }
            else
            {
                // Cuando llega de nuevo al jugador, desaparece
                StartCoroutine(DestruirConRetraso());
            }
        }
    }

    private IEnumerator DestruirConRetraso()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}