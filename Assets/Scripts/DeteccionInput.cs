using UnityEngine;
using UnityEngine.InputSystem;

public class DeteccionInput : MonoBehaviour
{
    public GameObject teclado;
    public GameObject gamepad;

    void Update()
    {
       
        if (Keyboard.current != null)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                teclado.SetActive(true);
                gamepad.SetActive(false);
            }
        }

       
        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftStick.ReadValue().magnitude > 0.2f ||
                Gamepad.current.rightStick.ReadValue().magnitude > 0.2f)
            {
                teclado.SetActive(false);
                gamepad.SetActive(true);
            }
        }
    }
}
