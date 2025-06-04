using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{

    [SerializeField] private Transform[] puntosMovimiento;
    [SerializeField] private float velocidad;
    private int siguientePlataforma = 1;
    private bool ordenPlataformas = true;

    public bool modoInvencible { get; private set; }

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
            if (contacto.normal.y > 0.5f & modoInvencible == false) // El jugador toca desde abajo
            {
                Debug.Log("El jugador murió al tocar la plataforma desde abajo");

                  other.gameObject.GetComponent<ReiniciarNivel>().Reiniciar();  
          
                other.transform.SetParent(null);
                break;
            }
            else if (contacto.normal.y < -0.5f) 
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
