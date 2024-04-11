//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Receiver : MonoBehaviour
{
    static readonly string k_EmissionKeyword = "_EMISSION";
    public Material material;
    public GameObject[] PCs;

    // Start is called before the first frame update
    void Start()
    {
        PCs = GameObject.FindGameObjectsWithTag("PC");
        SetMachineActive(false);
    }

    // Update is called once per frame
    public void CheckConnections()
    {
        Debug.Log("checkConnectionsPC");
        // Contador de pcs con conexión
        //int pcsConConexion = 0;

        // Iterar sobre cada pc
        for (int pcIndex = 0; pcIndex < PCs.Length; pcIndex++)
        {
            GameObject currentPC = PCs[pcIndex];

            SocketWitchTagCheck currentSocket = currentPC.transform.GetChild(0).GetComponent<SocketWitchTagCheck>();
            if (currentSocket.isConnected)
            {
                // Verificar conexiones con otros pcs
                for (int otherPCIndex = 0; otherPCIndex < PCs.Length; otherPCIndex++)
                {
                    if (otherPCIndex != pcIndex) // Evitar comparar un pc consigo mismo
                    {
                        GameObject otherPC = PCs[otherPCIndex];

                        SocketWitchTagCheck otherCurrentSocket = otherPC.transform.GetChild(0).GetComponent<SocketWitchTagCheck>();
                        if (otherCurrentSocket.isConnected && currentSocket.cable == otherCurrentSocket.cable.extremoCable)
                        {
                            //pcsConConexion++;
                            currentPC.GetComponent<Receiver>().SetMachineActive(true);
                            otherPC.GetComponent<Receiver>().SetMachineActive(true);
                            return;
                        }
                        else
                        {
                            currentPC.GetComponent<Receiver>().SetMachineActive(false);
                            otherPC.GetComponent<Receiver>().SetMachineActive(false);
                        }
                    }
                }
            }
        }

        // Verificar si al menos dos routers tienen al menos un socket conectado por el mismo cable
        //if (pcsConConexion >= 1)
        //{
        //    SetMachineActive(true);
        //}
        //else
        //{
        //    SetMachineActive(false);
        //}

        //// Comprueba si ambos sockets están conectados y luego activa la máquina.
        //if (socket1.isConnected == true && socket2.isConnected==true && socket1.cable==socket2.cable.extremoCable)
        //{
        //    SetMachineActive(true);
        //    Debug.Log("true");
        //    //Debug.Log(socket1.isConnected);
        //    //Debug.Log(socket2.isConnected);
        //    //Debug.Log(socket1.cable.extremoCable == socket2.cable.extremoOpuestoCable);
        //}
        //else
        //{
        //    SetMachineActive(false);
        //    Debug.Log("false");
        //    //Debug.Log(socket1.isConnected);
        //    //Debug.Log(socket2.isConnected);
        //    //Debug.Log(socket1.cable.extremoCable == socket2.cable.extremoOpuestoCable);
        //}
    }
    //public void CheckPcConection(SelectEnterEventArgs args)
    //{
    //    var interactable = args.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
    //    interactable.isConnected = true;

    //    var socket = interactable.extremoCable.GetComponent<GrabInteractableCable>();
    //    if (!socket.isConnected) return;

    //    bufferConsole.AddLine("PC>");
    //}

    public void SetMachineActive(bool active)
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