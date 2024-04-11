using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
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
        // Crea un nuevo objeto a partir del prefab especificado
        GameObject createdObject = Instantiate(objectToCreate);

        // Ajusta la posici�n, rotaci�n y escala del objeto seg�n tus necesidades
        createdObject.transform.position = new Vector3(0, 1, 2); // Cambia esto para la posici�n dese
        createdObject.transform.localScale = new Vector3(0.5f, 0.2f, 0.4f); //justa la escala del objeto (en este ejemplo, se establece a 2 en todas las direcciones)
        createdObject.transform.rotation = Quaternion.identity; // Cambia esto para la rotaci�n deseada
        // Puedes realizar configuraciones adicionales si es necesario

        // Aseg�rate de que el objeto creado tenga un Rigidbody si es necesario para la f�sica
        Rigidbody rb = createdObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = createdObject.AddComponent<Rigidbody>();
        }

        // Realiza cualquier otra configuraci�n espec�fica del objeto

        // Notifica que el objeto se ha creado correctamente
        UnityEngine.Debug.Log("Objeto creado correctamente.");
    }
    
}
