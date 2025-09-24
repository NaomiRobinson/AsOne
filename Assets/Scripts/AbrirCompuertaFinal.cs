using UnityEngine;

public class AbrirCompuertaFinal : MonoBehaviour
{
    public GameObject gemaAsociada;

    public GameObject compuertaAsociada;
    public GameObject jugadorAsignado;
    public float offsetAltura = 2f;
    public float duracionMovimiento = 1.5f;
    public LeanTweenType tipoEasing = LeanTweenType.easeInOutQuad;

    private bool yaActivado = false;
    private Vector3 posicionFinal;

    private AbrirCompuerta abrirCompuerta;

    private Animator animCompuerta;

    private void Awake()
    {
        posicionFinal = gemaAsociada.transform.position;

        gemaAsociada.SetActive(false);

        if (compuertaAsociada != null)
        {
            abrirCompuerta = compuertaAsociada.GetComponent<AbrirCompuerta>();
        }
        animCompuerta = compuertaAsociada?.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (yaActivado) return;
        if (other.gameObject != jugadorAsignado) return;

        yaActivado = true;

        Vector3 spawnPos = other.transform.position + new Vector3(0, offsetAltura, 0);

        gemaAsociada.transform.position = spawnPos;
        gemaAsociada.SetActive(true);

        LeanTween.move(gemaAsociada, posicionFinal, duracionMovimiento)
                 .setEase(tipoEasing)
                 .setOnComplete(() =>
                 {
                     Debug.Log("La gema llegó a su posición final");

                     if (animCompuerta != null)
                     {
                         SoundManager.instancia.ReproducirSonido(SoundManager.instancia.mecanismo_compuerta);
                         AnimacionesControlador.SetBool(animCompuerta, "estaAbierta", true);
                     }
                 });
    }
}
