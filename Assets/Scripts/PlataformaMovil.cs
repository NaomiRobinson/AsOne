using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [SerializeField] private Transform[] puntosMovimiento;
    [SerializeField] private float velocidad;
    private int siguientePlataforma = 1;
    private bool ordenPlataformas = true;

    public MovimientoJugador movimientoJugador;

    private void Update()
    {
        if (ordenPlataformas && siguientePlataforma + 1 >= puntosMovimiento.Length)
        {
            ordenPlataformas = false;
        }

        if (!ordenPlataformas && siguientePlataforma <= 0)
        {
            ordenPlataformas = true;
        }

        if (Vector2.Distance(transform.position, puntosMovimiento[siguientePlataforma].position) < 0.1f)
        {
            if (ordenPlataformas)
            {
                siguientePlataforma += 1;
            }
            else
            {
                siguientePlataforma -= 1;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, puntosMovimiento[siguientePlataforma].position, velocidad * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("JugadorIzq") || other.gameObject.CompareTag("JugadorDer"))
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("JugadorIzq") || other.gameObject.CompareTag("JugadorDer"))
        {
            other.transform.SetParent(null);
        }
    }
}
