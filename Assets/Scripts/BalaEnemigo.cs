using UnityEngine;

public class BalaEnemigo : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;

    [HideInInspector] public Vector2 direccion;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.MovePosition(rb.position + direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Walls") || collision.CompareTag("Caja"))
        {
            Debug.Log("bala tocó pared o caja");
            Destroy(gameObject);
        }
        else if (collision.CompareTag("JugadorIzq") || collision.CompareTag("JugadorDer"))
        {
            Debug.Log("bala tocó al jugador");

            if (collision.TryGetComponent(out ReiniciarNivel reiniciar) && !reiniciar.movimientoJugador.modoInvencible)
                reiniciar.Reiniciar();

            Destroy(gameObject);
        }
    }
}
