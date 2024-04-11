using System.Diagnostics;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;


public class activoSocket : MonoBehaviour
{
    public XRSocketInteractor socketInteractor; // Asigna el XR Socket Interactor desde el Inspector
    private XRGrabInteractable grabInteractable;
    private bool socketActive = false;
    private bool isGrabbed = false;
    public TextMeshProUGUI textMeshPro; // Asigna el objeto TextMeshPro desde el Inspector

    private void OnEnable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnSocketActivated);
        }
    }

    private void OnDisable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnSocketActivated);
        }
    }
    private void OnSocketActivated(SelectEnterEventArgs args)
    {
        // Aquí puedes agregar tus condiciones específicas cuando el XRSocketInteractor se activa
        UnityEngine.Debug.Log("El XRSocketInteractor se ha activado");
        textMeshPro.text = "El cable esta conectado";
    }
}

