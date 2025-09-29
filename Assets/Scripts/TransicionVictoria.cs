using UnityEngine;

public class TransicionVictoria : MonoBehaviour
{
    public int Menuvictoria = 33;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("JugadorDer"))
        {
            TransicionEscena.Instance.Disolversalida(Menuvictoria);
        }
    }
}
