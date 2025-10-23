using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class PopUpReintentar : MonoBehaviour
{
    [SerializeField] private GameObject popUpReintento;
    [SerializeField] private GameObject botonPorDefecto;
    [SerializeField] private TextMeshProUGUI reintentarTexto;

    private Controles controles;

    public static Action OnConfirmarReintento;

    void Awake()
    {
        controles = new Controles();
    }

    void Update()
    {

    }

    public void MostrarPopUp(string mensaje = "¿Quieres reintentar?")
    {
        if (reintentarTexto != null)
            reintentarTexto.text = mensaje;

        controles.Jugador.Disable();
        controles.UI.Enable();

        if (popUpReintento != null)
            popUpReintento.SetActive(true);

        if (botonPorDefecto != null)
            EventSystem.current.SetSelectedGameObject(botonPorDefecto);
    }

    public void confirmarReintento()
    {
        controles.UI.Disable();
        controles.Jugador.Enable();

        Debug.Log("Reintento confirmado");

        if (popUpReintento != null)
            popUpReintento.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        OnConfirmarReintento?.Invoke();
    }

    public void cancelar()
    {
        if (popUpReintento != null)
            popUpReintento.SetActive(false);

        controles.UI.Disable();
        controles.Jugador.Enable();
        EventSystem.current.SetSelectedGameObject(null);

        Debug.Log("Reintento cancelado");
    }

}
