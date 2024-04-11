using UnityEngine;

public class CableRef : MonoBehaviour
{
    public CableRef extremoCable;  // Cambiado a tipo CableRef
    //public CableRef extremoOpuestoCable;  // Nuevo atributo para guardar el extremo opuesto

    void Start()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            GameObject currentChild = transform.parent.GetChild(i).gameObject;
            if (currentChild != this.gameObject)
            {
                extremoCable = currentChild.GetComponent<CableRef>();  // Obtener el componente CableRef
                //extremoOpuestoCable = this;  // Establecer el extremo opuesto como este componente
                break;  // Detener el bucle luego de encontrar el extremo opuesto
            }
        }
    }
}
