using UnityEngine;

public class SincronizarMovimientoObjeto : MonoBehaviour
{
    public Transform objetoOriginal;
    private Vector3 posicionAnterior;

    public Collider2D ignorarCollider;

    void Start()
    {
        posicionAnterior = objetoOriginal.position;

        Collider2D colliderCaja = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(colliderCaja, ignorarCollider);
    }

    void Update()
    {
        Vector3 deltaMovimiento = objetoOriginal.position - posicionAnterior;

        transform.position += deltaMovimiento;

        posicionAnterior = objetoOriginal.position;
    }

}
