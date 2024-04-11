using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class crearObjeto : MonoBehaviour
{
    public GameObject objectToCreate; // Asigna la plantilla desde el Inspector
    public Transform spawnPoint; // Asigna el punto de aparición desde el Inspector

    private XRGrabInteractable grabInteractable;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.onSelectEntered.AddListener(CreateNewObject);
        }
    }

    private void CreateNewObject(XRBaseInteractor interactor)
    {
        // Clona el objeto en la posición especificada
        Instantiate(objectToCreate, spawnPoint.position, spawnPoint.rotation);
    }
}
