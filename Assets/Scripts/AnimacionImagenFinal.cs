using UnityEngine;
using UnityEngine.UI;
public class AnimacionImagenFinal : MonoBehaviour
{
    public float escalaInicial = 2f;
    public float escalaFinal = 1f;
    public float duracionZoom = 2f;

    public float movimientoDistancia = 1000f;
    public float duracionMovimiento = 3f;

    public float anguloRotacion = 5f;
    public float duracionRotacion = 4f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.localScale = Vector3.one * escalaInicial;

        LeanTween.scale(rectTransform, Vector3.one * escalaFinal, duracionZoom)
                 .setEaseOutCubic()
                 .setOnComplete(() =>
                 {
                     MovimientoLeve();
                     RotacionLeve();
                 });
    }

    void MovimientoLeve()
    {
        Vector3 posOriginal = rectTransform.anchoredPosition;
        Vector3 destino = posOriginal + new Vector3(0, movimientoDistancia, 0);

        LeanTween.move(rectTransform, destino, duracionMovimiento)
                 .setEaseInOutSine()
                 .setLoopPingPong();
    }
    void RotacionLeve()
    {
        LeanTween.rotateZ(rectTransform.gameObject, anguloRotacion, duracionRotacion)
                 .setEaseInOutSine()
                 .setLoopPingPong(); // gira a un lado y al otro infinitamente
    }
}