using UnityEngine;

public class NivelSeleccionado : MonoBehaviour
{
    public int grupoSeleccionado;
    public GameObject jugadorAsignado;
    private Animator animPuerta;
    private static int jugadoresEnPuerta = 0;
    private bool estaEnPuerta = false;

    public bool esPuertaFinal = false;
    public bool llavesRecolectadas = false;

    private bool yaSelecciono = false;
    void Start()
    {
        animPuerta = GetComponent<Animator>();
    }

    void Update()
    {
        RevisarEntrada();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject == jugadorAsignado)
        {
            estaEnPuerta = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == jugadorAsignado)
        {
            estaEnPuerta = false;

            if (animPuerta != null)
            {
                animPuerta.SetBool("estaAbierta", false);
            }

            if (jugadoresEnPuerta > 0)
            {
                jugadoresEnPuerta--;
            }

            Debug.Log($"{jugadorAsignado.name} salió de la salida. Jugadores en salida: {jugadoresEnPuerta}");
        }
    }

    private void RevisarEntrada()
    {
        if (estaEnPuerta && Input.GetKeyDown(KeyCode.Space) && !yaSelecciono)
        {

            if (!esPuertaFinal || (esPuertaFinal && llavesRecolectadas))
            {
                yaSelecciono = true;

                AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);
                jugadoresEnPuerta++;

                if (jugadoresEnPuerta == 2)
                {
                    if (esPuertaFinal)
                    {
                        LevelManager.Instance.CargarFinal();
                    }
                    else
                    {
                        LevelManager.Instance.SeleccionarGrupo(grupoSeleccionado);
                    }
                }
            }
            else
            {
                Debug.Log("Faltan partes de la llave para abrir la puerta final");
            }
        }
    }
}
