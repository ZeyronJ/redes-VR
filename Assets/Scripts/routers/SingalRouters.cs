using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalRouters : MonoBehaviour
{
    static readonly string k_EmissionKeyword = "_EMISSION";
    public Material[] materials;
    public GameObject[] routers;
    public bool stateMachine = false;

    void Start()
    {
        routers = GameObject.FindGameObjectsWithTag("Router");
        SetMachineActive(false);
        //InvokeRepeating("CheckConecctions", 0f, 0.1f);
    }

    public void CheckConnections()
    {
        //bool anyRouterConnected = false;

        for (int routerIndex = 0; routerIndex < routers.Length; routerIndex++)
        {
            GameObject currentRouter = routers[routerIndex];
            bool currentRouterConnected = false;

            for (int socketIndex = 0; socketIndex < currentRouter.transform.childCount; socketIndex++)
            {
                SocketTagRouters currentSocket = currentRouter.transform.GetChild(socketIndex).GetComponent<SocketTagRouters>();

                if (currentSocket.isConnected)
                {
                    for (int otherRouterIndex = 0; otherRouterIndex < routers.Length; otherRouterIndex++)
                    {
                        if (otherRouterIndex != routerIndex)
                        {
                            GameObject otherRouter = routers[otherRouterIndex];

                            for (int otherSocketIndex = 0; otherSocketIndex < otherRouter.transform.childCount; otherSocketIndex++)
                            {
                                SocketTagRouters otherSocket = otherRouter.transform.GetChild(otherSocketIndex).GetComponent<SocketTagRouters>();

                                if (otherSocket.isConnected && currentSocket.cable == otherSocket.cable.extremoCable)
                                {
                                    currentRouterConnected = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (currentRouterConnected)
            {
                //anyRouterConnected = true;
                currentRouter.GetComponent<SignalRouters>().SetMachineActive(true);
            }
            else
            {
                currentRouter.GetComponent<SignalRouters>().SetMachineActive(false);
            }
        }

        // Después de todas las comprobaciones, puedes decidir cómo manejar el estado de la máquina globalmente.
        //SetMachineActive(anyRouterConnected);
    }

    public void SetMachineActive(bool active)
    {
        foreach (var material in materials)
        {
            stateMachine = active;
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
