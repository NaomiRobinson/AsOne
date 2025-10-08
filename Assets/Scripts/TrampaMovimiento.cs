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
    private bool yaImpacto = false;
    private bool puedeCaer = true;

    public float tiempoAntesDeSubir = 2f;
    public float velocidadSubida;

    [SerializeField] private GameObject efecto;

    public LayerMask capaJugador;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }


    void Update()
    {
        Vector2 direccion = -transform.up;


        if (estaSubiendo)
        {
            transform.Translate(-direccion * velocidadSubida * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, posicionInicial) < 0.05f)
            {
                transform.position = posicionInicial;
                estaSubiendo = false;
                rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
                puedeCaer = true;
                yaImpacto = false;
            }
            return;
        }


        if (puedeCaer)
        {
            RaycastHit2D infoJugador = Physics2D.Raycast(transform.position, direccion, distanciaLinea, capaJugador);
            if (infoJugador)
            {
                rb2D.constraints = RigidbodyConstraints2D.None;
                rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Walls") && !yaImpacto)
        {
            yaImpacto = true;
            puedeCaer = false;
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;

            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.trampa_caer);
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();

            ContactPoint2D contacto = other.GetContact(0);
            Vector2 normal = contacto.normal;

            Quaternion rotacion = Quaternion.LookRotation(Vector3.forward, normal);
            GameObject chispas = Instantiate(efecto, contacto.point, rotacion);
            Destroy(chispas, 1f);
            StartCoroutine(SquashImpact(transform, 0.2f, 0.1f));

            if (!esperandoParaSubir)
                StartCoroutine(EsperarAntesDeSubir());
        }
    }

    private IEnumerator EsperarAntesDeSubir()
    {
        esperandoParaSubir = true;
        yield return new WaitForSeconds(tiempoAntesDeSubir);
        estaSubiendo = true;
        esperandoParaSubir = false;
    }

    IEnumerator SquashImpact(Transform obj, float intensidad, float duracion)
    {
        Vector3 original = obj.localScale;
        Vector3 squash = new Vector3(original.x * 1.1f, original.y * 0.7f, 1f);
        obj.localScale = squash;
        yield return new WaitForSeconds(duracion);
        obj.localScale = original;
    }

    private void OnDrawGizmos()
    {
        Vector2 direccion = -transform.up;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direccion * distanciaLinea));
    }
}