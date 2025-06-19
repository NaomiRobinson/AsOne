using UnityEngine;
using UnityEngine.SceneManagement;
public class NivelSeleccionado : MonoBehaviour
{
    public int grupoSeleccionado;
    public GameObject jugadorAsignado;
    private Animator animPuerta;
    private static int jugadoresEnPuerta = 0;
    private bool estaEnPuerta = false;

    public bool esPuertaFinal = false;
    //public bool llavesRecolectadas = false;

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
        if (!estaEnPuerta || yaSelecciono) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            yaSelecciono = true;

            if (animPuerta != null && !esPuertaFinal)
                AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);

            jugadoresEnPuerta++;
            Debug.Log($"Jugadores en puerta incrementado a: {jugadoresEnPuerta}");

            if (jugadoresEnPuerta == 2)
            {
                int escenaActual = SceneManager.GetActiveScene().buildIndex;
                Debug.Log($"Escena actual buildIndex: {escenaActual}");

                if (escenaActual == LevelManager.Instance.SeleccionNiveles)
                {
                    Debug.Log($"Intentando seleccionar grupo {grupoSeleccionado}");
                    LevelManager.Instance.SeleccionarGrupo(grupoSeleccionado);
                }
                else if (esPuertaFinal)
                {
                    if (ChequeoLlaves.TodasRecolectadas())
                    {
                        AnimacionesControlador.SetBool(animPuerta, "estaAbierta", true);
                        Debug.Log("¡Todas las llaves están recolectadas!");
                        LevelManager.Instance.CargarFinal();
                    }
                    else
                    {
                        Debug.LogWarning("No todas las llaves están recolectadas, no se puede cargar final.");
                    }
                }
                else
                {

                    Debug.Log("Entrando en flujo de niveles dentro de un grupo.");
                    int nivelActual = escenaActual;

                    Debug.Log("Es último nivel? " + LevelManager.Instance.EsUltimoNivel(nivelActual));
                    if (LevelManager.Instance.EsUltimoNivel(nivelActual))
                    {
                        Debug.Log("Último nivel detectado. Marcando grupo como completado.");
                        LevelManager.Instance.MarcarGrupoCompletado();
                        Debug.Log("Grupo desbloqueado tras completar: " + LevelManager.Instance.grupoDesbloqueado);
                        TransicionEscena.Instance.Disolversalida(LevelManager.Instance.SeleccionNiveles);
                    }
                    else
                    {
                        int siguiente = LevelManager.Instance.ObtenerSiguienteNivel(nivelActual);
                        Debug.Log($"Siguiente nivel a cargar: {siguiente}");
                        TransicionEscena.Instance.Disolversalida(siguiente);
                    }
                }
            }
        }
    }
}