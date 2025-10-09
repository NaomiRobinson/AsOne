using UnityEngine;

public class ColisionJugador : MonoBehaviour
{
    private MovimientoJugador movimiento;

    void Awake()
    {
        movimiento = FindObjectOfType<MovimientoJugador>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Walls")) return;

        movimiento.HandleCollision(collision);
    }
}