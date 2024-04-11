using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
public class CrearCablePanel : MonoBehaviour
{
    public GameObject objectToCreate;
    public Material someMaterial;
    private int objectsCreatedCount = 0;
    private float separationDistance = -10f; // Ajusta esto a la distancia que desees
    private float separar = -1f;
    public Vector3 position = new Vector3(1, 1, 2); // Propiedad serializada para la posición
    public void CreateSelectedObject()
    {
        objectsCreatedCount++;
        if (objectsCreatedCount == 1)
        {
            // Crea un nuevo objeto a partir del prefab especificado
            GameObject createdObject = Instantiate(objectToCreate);
            // Ajusta la posici�n, rotaci�n y escala del objeto seg�n tus necesidades   
            createdObject.SetActive(true);

            createdObject.transform.position += new Vector3(0f, 0f, separationDistance);


        }
        // Notifica que el objeto se ha creado correctamente
        UnityEngine.Debug.Log($"router #{objectsCreatedCount} creado correctamente.");
        // Puedes realizar configuraciones adicionales si es necesario

        if ((objectsCreatedCount == 2))
        {
            GameObject secondObject = Instantiate(objectToCreate);
            secondObject.SetActive(true);
            float additionalSeparation = separationDistance * (objectsCreatedCount);
            secondObject.transform.position += new Vector3(0f, 0f, additionalSeparation);
        }
        if (objectsCreatedCount == 3)
        {
            GameObject secondObject = Instantiate(objectToCreate);
            secondObject.SetActive(true);

            secondObject.transform.position += new Vector3(-10.7f, 0f, 1.36f);

        }
        if ((objectsCreatedCount < 6) && (objectsCreatedCount > 3))
        {
            GameObject secondObject = Instantiate(objectToCreate);
            secondObject.SetActive(true);

            float additionalSeparation = separationDistance * (objectsCreatedCount - 3);
            secondObject.transform.position += new Vector3(-10.7f, 0f, additionalSeparation);
        }

    }
    public void CreateSelectedObject1()
    {
        objectsCreatedCount++;
        if (objectsCreatedCount == 1)
        {
            // Crea un nuevo objeto cable
            GameObject createdObject = Instantiate(objectToCreate);


            // Establece la nueva posici�n
            createdObject.transform.position = position;

            Vector3 newPosition = createdObject.transform.position + new Vector3(0, 0, separar); // Incrementa la posici�n en

            // Accede al componente CableComponent del nuevo objeto
            CableComponent newCableComponent = createdObject.GetComponent<CableComponent>();

            // Aseg�rate de que newCableComponent no sea nulo
            if (newCableComponent != null)
            {
                UnityEngine.Debug.Log("encontrado en el objeto creado.");

                // Configura cualquier valor necesario en newCableComponent
                newCableComponent.cableWidth = 0.1f; // Establece el ancho del cable
                newCableComponent.cableMaterial = someMaterial; // Asigna el material del cable

                // Luego, llama a los m�todos de inicializaci�n
                newCableComponent.InitCableParticles();
                newCableComponent.InitLineRenderer();
            }
            else
            {
                UnityEngine.Debug.Log("CableComponent no encontrado en el objeto creado.");
            }
        }

    }
}
