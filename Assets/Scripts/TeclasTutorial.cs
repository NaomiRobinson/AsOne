using UnityEngine;
using UnityEngine.InputSystem;

public class TeclasTutorial : MonoBehaviour
{
    public enum Direccion { Arriba, Abajo, Izquierda, Derecha }

    [SerializeField] private Direccion direccion;
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteIluminado;
    private SpriteRenderer sr;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    void Update()
    {
        bool presionada = false;
        switch (direccion)
        {
            case Direccion.Arriba:
                presionada = Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed;
                break;
            case Direccion.Abajo:
                presionada = Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;
                break;
            case Direccion.Izquierda:
                presionada = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
                break;
            case Direccion.Derecha:
                presionada = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
                break;
        }

        sr.sprite = presionada ? spriteIluminado : spriteNormal;
    }
}