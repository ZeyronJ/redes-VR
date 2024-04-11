using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CrearPanel1 : MonoBehaviour
{

    public GameObject objectToCreate;
    private GameObject selectedObject;
    private int objectsCreatedCount = 0;
    private float separationDistance = -11f; // Ajusta esto a la distancia que desees
    public Vector3 position = new Vector3(-13, 13, 12); // Propiedad serializada para la posición
    private void Start()
    {
    }



    public void CreateSelectedObject()
    {
    
        if (objectToCreate != null)
        {
            // Activar el objeto.
            objectToCreate.SetActive(true);
        }
        // Crea un nuevo objeto a partir del prefab especificado
        objectsCreatedCount++;
        if(objectsCreatedCount == 1)
        {        
            GameObject createdObject = Instantiate(objectToCreate);
            // Ajusta la posici�n, rotaci�n y escala del objeto seg�n tus necesidades
            createdObject.transform.position += new Vector3(0f, 0f, separationDistance );
            createdObject.transform.rotation = Quaternion.Euler(0, 0, 0); // Cambia esto para la rotaci�n deseada
        }

        // Puedes realizar configuraciones adicionales si es necesario

        // Aseg�rate de que el objeto creado tenga un Rigidbody si es necesario para la f�sica
  

        // Realiza cualquier otra configuraci�n espec�fica del objeto

        // Notifica que el objeto se ha creado correctamente
        UnityEngine.Debug.Log($"pc #{objectsCreatedCount } creado correctamente.");

        // Incrementa el contador de objetos creados
    

        // Si es la segunda vez que se llama a la función, crea el segundo objeto y muévelo en la coordenada Z
         if ((objectsCreatedCount == 2) )
        {
            GameObject secondObject = Instantiate(objectToCreate);

            separationDistance = -10.75f;
            
            float additionalSeparation = separationDistance * (objectsCreatedCount);
            secondObject.transform.position += new Vector3(0f, 0f, additionalSeparation );
        }
        
         if(objectsCreatedCount ==3 )
        {
            GameObject secondObject = Instantiate(objectToCreate);
            secondObject.transform.position += new Vector3(-10f, 0f, 0f );

        } 
        if((objectsCreatedCount <6) && (objectsCreatedCount>3))
        {
            GameObject secondObject = Instantiate(objectToCreate);

            if(objectsCreatedCount ==4)
                {
                    separationDistance = -11f;
                }
            if(objectsCreatedCount ==5)
                {
                    separationDistance = -10.75f;
                }
            float additionalSeparation = separationDistance * (objectsCreatedCount - 3);
            secondObject.transform.position += new Vector3(-10f, 0f,additionalSeparation );
        }

        

    }
}
