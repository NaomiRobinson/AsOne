using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadTutorial : MonoBehaviour
{
    public enum Direccion
    {
        LeftArriba, LeftAbajo, LeftIzquierda, LeftDerecha,
        RightArriba, RightAbajo, RightIzquierda, RightDerecha
    }

    [System.Serializable]
    public struct InputSprite
    {
        public Direccion direccion;
        public Sprite spriteNormal;
        public Sprite spriteDireccion;

    }
    private SpriteRenderer sr;


    public InputSprite[] inputs;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool cambiado = false;

        foreach (var input in inputs)
        {
            if (CheckDireccion(input.direccion))
            {
                sr.sprite = input.spriteDireccion;
                cambiado = true;
                break;
            }
        }

        if (!cambiado)
            sr.sprite = inputs[0].spriteNormal;
    }

    bool CheckDireccion(Direccion direccion)
    {
        if (Gamepad.current == null) return false;

        Vector2 left = Gamepad.current.leftStick.ReadValue();
        Vector2 right = Gamepad.current.rightStick.ReadValue();

        switch (direccion)
        {
            // Stick izquierdo
            case Direccion.LeftArriba: return left.y > 0.2f;
            case Direccion.LeftAbajo: return left.y < -0.2f;
            case Direccion.LeftIzquierda: return left.x < -0.2f;
            case Direccion.LeftDerecha: return left.x > 0.2f;

            // Stick derecho
            case Direccion.RightArriba: return right.y > 0.2f;
            case Direccion.RightAbajo: return right.y < -0.2f;
            case Direccion.RightIzquierda: return right.x < -0.2f;
            case Direccion.RightDerecha: return right.x > 0.2f;
        }

        return false;
    }
}