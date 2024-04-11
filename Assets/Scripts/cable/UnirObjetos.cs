using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UnirObjetos : MonoBehaviour
{
    // Referencias a los dos objetos que se conectarán.
    public GameObject objectToConnectTo;

    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    private FixedJoint joint;

    private void Awake()
    {
        // Obtiene una referencia al componente XRGrabInteractable.
        grabInteractable = GetComponent<XRGrabInteractable>();
        joint = GetComponent<FixedJoint>();
        // Desactiva el Fixed Joint al principio para evitar comportamiento inesperado.
        if (joint != null)
        {
            joint.connectedBody = null;
            joint.gameObject.SetActive(false);
        }
    }


    public void Update()
    {
        // Puedes agregar código de actualización aquí si es necesario.
        // Por ejemplo, puedes realizar comprobaciones continuas o ajustes.
    }
    private void OnEnable()
    {

        // Suscribe el evento OnSelectEnter del XRGrabInteractable.
        grabInteractable.selectEntered.AddListener(ConnectObjects);

    }

    private void OnDisable()
    {
        // Desuscribe el evento OnSelectEnter para evitar fugas de memoria.
        grabInteractable.selectEntered.RemoveListener(ConnectObjects);
    }

    public void ConnectObjects(SelectEnterEventArgs args)
    {
        // Verifica si el objeto ya está agarrado.
        if (isGrabbed)
        {                

            // Realiza la conexión entre los objetos.
            if (objectToConnectTo != null)
            {
                // Establece la posición y la rotación del objeto agarrado
                // al objeto de destino.
                transform.position = objectToConnectTo.transform.position;
                transform.rotation = objectToConnectTo.transform.rotation;

                // Crea una articulación si no existe.
                if (joint == null)
                {
                    joint = gameObject.AddComponent<FixedJoint>();
                }

                // Configura la articulación.
                joint.connectedBody = objectToConnectTo.GetComponent<Rigidbody>();
                joint.anchor = Vector3.zero;
                joint.autoConfigureConnectedAnchor = true;


                // Activa la articulación.
                joint.gameObject.SetActive(true);

            }
        }

        isGrabbed = true;
    }
}
