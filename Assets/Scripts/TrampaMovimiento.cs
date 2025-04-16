using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

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
        RaycastHit2D infoJugador = Physics2D.Raycast(transform.position, Vector3.down, distanciaLinea, capaJugador);

        if (infoJugador && !estaSubiendo)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        if (estaSubiendo)
        {
            transform.Translate(Time.deltaTime * velocidadSubida * Vector3.up);

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distanciaLinea);
    }
}