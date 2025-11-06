using UnityEngine;


public class FijarSentidoPopUp : MonoBehaviour
{
    public Vector3 offset = new Vector3(1f, 0.5f, 0f);

    private Transform jugador;

    void Start()
    {
        jugador = transform.parent;
    }

    void LateUpdate()
    {
        if (jugador == null) return;

        float lado = Mathf.Sign(jugador.localScale.x);

        // Ajustar posición
        transform.localPosition = new Vector3(offset.x * lado, offset.y, offset.z);

        // Evitar flip visual: siempre mantener escala positiva
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x);
        transform.localScale = escala;
    }
}
