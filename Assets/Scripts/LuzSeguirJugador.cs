using UnityEngine;

public class LuzSeguirJugador : MonoBehaviour
{
    public Transform jugador;
    public float suavizado = 5f;

    void LateUpdate()
    {
        if (jugador == null) return;
        transform.position = Vector3.Lerp(transform.position, jugador.position, Time.deltaTime * suavizado);
    }
}