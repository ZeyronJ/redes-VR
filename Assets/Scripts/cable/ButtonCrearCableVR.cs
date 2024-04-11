using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class ButtonCrearCableVR : MonoBehaviour
{   
    public GameObject button;
    public GameObject objectToCreate;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public Material someMaterial;
    GameObject presser;
    bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(1.15f,0.183f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {            
        UnityEngine.Debug.Log("boton presionado");
        if(other.gameObject == presser)
        {   
            button.transform.localPosition = new Vector3(1.18f, 0.15f, 0.02f);
            onRelease.Invoke();

            isPressed = false;
        }
    }
    public void CreateSelectedObject()
    {
        // Crea un nuevo objeto cable
        GameObject createdObject = Instantiate(objectToCreate);

        //newCableComponent.transform.localPosition = new Vector3(0, 1, 2);
        // Calcula la posición para el nuevo objeto
        float separationDistance = 0.04f; // Ajusta esto a la distancia que desees
        Vector3 newPosition = createdObject.transform.position + new Vector3(separationDistance, 0, 0); // Incrementa la posición en

        // Establece la nueva posición
        createdObject.transform.position = newPosition;

        // Accede al componente CableComponent del nuevo objeto
        CableComponent newCableComponent = createdObject.GetComponent<CableComponent>();

        // Asegúrate de que newCableComponent no sea nulo
        if (newCableComponent != null)
        {
            UnityEngine.Debug.Log("encontrado en el objeto creado.");

            // Configura cualquier valor necesario en newCableComponent
            newCableComponent.cableWidth = 0.1f; // Establece el ancho del cable
            newCableComponent.cableMaterial = someMaterial; // Asigna el material del cable

            // Luego, llama a los métodos de inicialización
            newCableComponent.InitCableParticles();
            newCableComponent.InitLineRenderer();
        }
        else
        {
            UnityEngine.Debug.Log("CableComponent no encontrado en el objeto creado.");
        }
    }
}
