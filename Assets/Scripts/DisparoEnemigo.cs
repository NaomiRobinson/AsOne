using UnityEngine;

public class DisparoEnemigo : MonoBehaviour
{
    public Transform controladorDisparo;

    public GameObject EnemigoTorreta;
    public LayerMask capaJugador;
    public bool jugadorEnRango;
    public float tiempoEntreDisparos;
    public float tiempoUltimoDisparo;
    public GameObject bala;

    public float tiempoEsperaDisparo;
    private Animator animTorreta;

    public float distancia;

    void Start()
    {
        animTorreta = EnemigoTorreta.GetComponent<Animator>();
    }


    private void Update()
    {
        jugadorEnRango = Physics2D.Raycast(controladorDisparo.position, transform.right, distancia, capaJugador);

        if (jugadorEnRango)
        {
            if (Time.time > tiempoEntreDisparos + tiempoUltimoDisparo)
            {
                tiempoUltimoDisparo = Time.time;
                Invoke(nameof(Disparar), tiempoEsperaDisparo);

            }
        }
    }

    private void Disparar()
    {
        if (animTorreta != null)
        {
            AnimacionesControlador.SetTriggerConCallback(this, animTorreta, "dispara", 0.5f, () =>
            {
                GameObject nuevaBala = Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);
                nuevaBala.GetComponent<BalaEnemigo>().direccion = transform.right;

            });
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(controladorDisparo.position, controladorDisparo.position + transform.right * distancia);
    }
}
