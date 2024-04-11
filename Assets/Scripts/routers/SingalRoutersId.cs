using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalRoutersId : MonoBehaviour
{
    static readonly string k_EmissionKeyword = "_EMISSION";
    public Material[] materials;
    public GameObject[] routers;

    void Start()
    {
        routers = GameObject.FindGameObjectsWithTag("Router");
        SetMachineActive(false);
        //InvokeRepeating("CheckConecctions", 0f, 0.1f);
    }

    public void CheckConecctions()
    {
        // Contador de routers con conexión
        int routersConConexion = 0;

        // Iterar sobre cada router
        for (int routerIndex = 0; routerIndex < routers.Length; routerIndex++)
        {
            GameObject currentRouter = routers[routerIndex];

            for (int socketIndex = 0; socketIndex < currentRouter.transform.childCount; socketIndex++)
            {
                SocketTagRoutersId currentSocket = currentRouter.transform.GetChild(socketIndex).GetComponent<SocketTagRoutersId>();

                if (currentSocket.isConnected)
                {
                    // Verificar conexiones con otros routers
                    for (int otherRouterIndex = 0; otherRouterIndex < routers.Length; otherRouterIndex++)
                    {
                        if (otherRouterIndex != routerIndex) // Evitar comparar un router consigo mismo
                        {
                            GameObject otherRouter = routers[otherRouterIndex];

                            for (int otherSocketIndex = 0; otherSocketIndex < otherRouter.transform.childCount; otherSocketIndex++)
                            {
                                SocketTagRoutersId otherSocket = otherRouter.transform.GetChild(otherSocketIndex).GetComponent<SocketTagRoutersId>();

                                if (otherSocket.isConnected && currentSocket.cableId == otherSocket.cableId)
                                {
                                    routersConConexion++;
                                    break; // Romper el bucle para el socket actual
                                }
                            }
                        }
                    }
                    break; // Romper el bucle para el router actual (routers con conexion)
                }
            }
        }

        // Verificar si al menos dos routers tienen al menos un socket conectado por el mismo cable
        if (routersConConexion >= 1)
        {
            SetMachineActive(true);
        }
        else
        {
            SetMachineActive(false);
        }
    }

    public void SetMachineActive(bool active)
    {
        foreach (var material in materials)
        {
            if (active)
            {
                material.EnableKeyword(k_EmissionKeyword);
            }
            else
            {
                material.DisableKeyword(k_EmissionKeyword);
            }
        }
    }
}
