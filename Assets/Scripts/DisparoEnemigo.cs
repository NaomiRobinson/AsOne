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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animTorreta = EnemigoTorreta.GetComponent<Animator>();
    }

    // Update is called once per frame
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
            SoundManager.instance.ReproducirSonido(SoundManager.instance.enemigo_disparo);
            AnimacionesControlador.SetTriggerConCallback(this, animTorreta, "dispara", 0.5f, () =>
            {
                Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);
            });
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(controladorDisparo.position, controladorDisparo.position + transform.right * distancia);
    }
}
