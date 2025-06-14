using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{

    [SerializeField] private Transform[] puntosMovimiento;
    [SerializeField] private float velocidad;
    private int siguientePlataforma = 1;
    private bool ordenPlataformas = true;

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
            foreach (ContactPoint2D contacto in other.contacts)
            {
                // Convertir la normal del contacto al espacio local de la plataforma
                Vector2 normalLocal = transform.InverseTransformDirection(contacto.normal);

                if (normalLocal.y > 0.5f) // El jugador toca desde ARRIBA
                {
                    Debug.Log("El jugador murió al tocar la plataforma desde arriba");
                    other.gameObject.GetComponent<ReiniciarNivel>().Reiniciar();
                    other.transform.SetParent(null);
                    break;
                }
                else if (normalLocal.y < -0.5f) // El jugador toca desde ABAJO (como si caminara en el techo)
                {
                    other.transform.SetParent(this.transform);
                    break;
                }
            }
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
