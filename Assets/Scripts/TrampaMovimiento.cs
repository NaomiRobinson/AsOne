using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using Unity.Cinemachine;

public class TrampaMovimiento : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public float distanciaLinea;
    public bool estaSubiendo = false;
    private bool esperandoParaSubir = false;

    public float tiempoAntesDeSubir = 2f;
    public float velocidadSubida;

    public LayerMask capaJugador;
    void Start()
    {

    }


    void Update()
    {
        Vector2 direccion = -transform.up;

        if (estaSubiendo)
        {
            transform.Translate(-direccion * velocidadSubida * Time.deltaTime, Space.World);
            return;
        }

        RaycastHit2D infoJugador = Physics2D.Raycast(transform.position, direccion, distanciaLinea, capaJugador);

        if (infoJugador)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Walls"))
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;

            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.trampa_caer);
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();

            if (estaSubiendo)
            {
                estaSubiendo = false;
            }
            else if (!esperandoParaSubir)
            {
                StartCoroutine(EsperarAntesDeSubir());
            }
        }
    }

    private IEnumerator EsperarAntesDeSubir()
    {
        esperandoParaSubir = true;
        yield return new WaitForSeconds(tiempoAntesDeSubir);
        estaSubiendo = true;
        esperandoParaSubir = false;
    }

    private void OnDrawGizmos()
    {
        Vector2 direccion = -transform.up;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direccion * distanciaLinea));
    }
}