using UnityEngine;
using Unity.Cinemachine;

public class ZoomCamara : MonoBehaviour
{
    public float zoomOrthoSize = 3f;    // Tamaño de zoom
    public float zoomVelocidad = 2f;    // Velocidad de zoom
    public float moveVelocidad = 5f;    // Velocidad de movimiento de la cámara

    private CinemachineCamera cinemachineCam;
    private float originalSize;
    private Vector3 originalPosition;
    private Transform target;
    private bool zoomActivo = false;

    void Start()
    {
        // Obtenemos el Cinemachine Camera Component
        cinemachineCam = GetComponent<CinemachineCamera>();
        if (cinemachineCam == null)
        {
            Debug.LogError("No se encontró CinemachineCamera en este GameObject.");
            return;
        }

        originalSize = cinemachineCam.Lens.OrthographicSize;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (!zoomActivo) return;

        // Interpolamos el tamaño del zoom
        cinemachineCam.Lens.OrthographicSize = Mathf.Lerp(
            cinemachineCam.Lens.OrthographicSize,
            zoomOrthoSize,
            Time.deltaTime * zoomVelocidad
        );

        // Interpolamos la posición hacia el target si existe
        Vector3 targetPos = target != null ? new Vector3(target.position.x, target.position.y, transform.position.z) : originalPosition;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveVelocidad);

        // Si ya estamos casi en el tamaño y posición deseada, paramos el zoom
        if (Mathf.Abs(cinemachineCam.Lens.OrthographicSize - zoomOrthoSize) < 0.01f &&
            Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            zoomActivo = false;
        }
    }

    // Activar zoom hacia un target (puede ser null)
    public void ActivarZoom(Transform zoomTarget = null)
    {
        target = zoomTarget;
        zoomActivo = true;
    }

    // Restaurar zoom y posición original
    public void RestaurarZoom()
    {
        target = null;
        zoomOrthoSize = originalSize;
        zoomActivo = true;
    }
}