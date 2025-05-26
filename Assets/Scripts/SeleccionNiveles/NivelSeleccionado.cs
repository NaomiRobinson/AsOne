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
            yaSelecciono = false;

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

            yaSelecciono = true;
            if (!esPuertaFinal)
            {
                AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);
            }

            jugadoresEnPuerta++;

            if (jugadoresEnPuerta == 2)
            {
                if (esPuertaFinal)
                {
                    if (ChequeoLlaves.TodasRecolectadas())
                    {
                        AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);
                        Debug.Log("¡Todas las llaves están recolectadas!");
                        LevelManager.Instance.CargarFinal();

                    }

                }
                else
                {
                    LevelManager.Instance.SeleccionarGrupo(grupoSeleccionado);
                }
            }
        }
    }
}

