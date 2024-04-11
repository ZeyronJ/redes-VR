using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CrearObjetoPanel : MonoBehaviour
{

    public GameObject objectToCreate;
    private GameObject selectedObject;
    private int objectsCreatedCount = 0;
    private float separationDistance = -11f; // Ajusta esto a la distancia que desees
    public Vector3 position = new Vector3(1, 1, 2); // Propiedad serializada para la posición
    public Vector3 localScale = new Vector3(0.5f, 0.2f, 0.4f); // Propiedad serializada para la escala

    private void Start()
    {
    }



    public void CreateSelectedObject()
    {
        objectsCreatedCount++;
        if(objectsCreatedCount == 1)
        {
            // Crea un nuevo objeto a partir del prefab especificado
            GameObject createdObject = Instantiate(objectToCreate);
            // Ajusta la posici�n, rotaci�n y escala del objeto seg�n tus necesidades
            createdObject.transform.position += new Vector3(0f, 0f, separationDistance );
            createdObject.transform.localScale = localScale; // Usa la propiedad serializada para la escala
            createdObject.transform.rotation = Quaternion.Euler(0, 270, 0); // Cambia esto para la rotaci�n deseada
        }
        // Notifica que el objeto se ha creado correctamente
        UnityEngine.Debug.Log($"router #{objectsCreatedCount } creado correctamente.");
        // Puedes realizar configuraciones adicionales si es necesario

       if ((objectsCreatedCount == 2))
        {
            GameObject secondObject = Instantiate(objectToCreate);
            float additionalSeparation = separationDistance * (objectsCreatedCount - 2);
            secondObject.transform.position += new Vector3(-8.7f, 0f, additionalSeparation );
        }
        // Realiza cualquier otra configuraci�n espec�fica del objeto
        if(objectsCreatedCount == 3)
        {
            GameObject secondObject = Instantiate(objectToCreate);            
            float additionalSeparation = separationDistance * (objectsCreatedCount - 2);
            secondObject.transform.position += new Vector3(-8.7f, 0f, additionalSeparation);
        }

    }
}
