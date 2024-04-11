using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Assertions;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using System;
using NSU;
using System.Net.Sockets;
using System.Text;

public class RouterConManager : MonoBehaviour, IConnectionManager, INetworkManager
{
    public ConsoleBuffer buffer;
    private Dictionary<string, GameObject> routingTable;
    private List<GameObject> connectedDevices;
    private string _IPAddress = "";
    public string IPAddress
    {
        get { return _IPAddress; }
        set { _IPAddress = value; }
    }

    private void Awake()
    {
        buffer = new ConsoleBuffer();
        buffer.AddLine("Router");
        routingTable = new Dictionary<string, GameObject>();
        connectedDevices = new List<GameObject>();
    }

    public ConsoleBuffer GetBuffer() { return buffer; }

    public void handleConnection(BaseInteractionEventArgs args)
    {
        if(args is SelectEnterEventArgs enterArgs)
        {
            var interactable = enterArgs.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
            interactable.isConnected = true;

            interactable.device = gameObject;

            var socket = interactable.extremoCable.GetComponent<GrabInteractableCable>();
            if (!socket.isConnected) return;

            if (!connectedDevices.Contains(socket.device))
            {
                connectedDevices.Add(socket.device);
                routingTable[socket.device.GetComponent<INetworkManager>().IPAddress] = socket.device;
            }

            var other = socket.device.GetComponent<IConnectionManager>();

            other?.handleSignal(enterArgs);
        }
        else if(args is SelectExitEventArgs exitArgs)
        {
            var interactable = exitArgs.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
            interactable.isConnected = false;

            interactable.device = null;
            var socket = interactable.extremoCable.GetComponent<GrabInteractableCable>();
            if (!socket.isConnected) return;

            if (connectedDevices.Contains(socket.device))
            {
                connectedDevices.Remove(socket.device);
                routingTable[socket.device.GetComponent<INetworkManager>().IPAddress] = null;
            }

            var other = socket.device.GetComponent<IConnectionManager>();
            other?.handleSignal(exitArgs);
        }
    }

    public void handleSignal(BaseInteractionEventArgs args)
    {
        if (args is SelectEnterEventArgs enterArgs)
        {
            var interactable = enterArgs.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
            if (!connectedDevices.Contains(interactable.device))
            {
                connectedDevices.Add(interactable.device);
                routingTable[interactable.device.GetComponent<INetworkManager>().IPAddress] = interactable.device;
            }
        }
        else if (args is SelectExitEventArgs exitArgs)
        {
            var interactable = exitArgs.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
            if (connectedDevices.Contains(interactable.device))
            {
                connectedDevices.Remove(interactable.device);
                routingTable[interactable.device.GetComponent<INetworkManager>().IPAddress] = null;
            }
        }
    }

    //public void ConfigureIPAddress(GameObject pc, string ipAddress)
    //{
    //    var pcConManager = pc.GetComponent<PCConManager>();
    //    if(pcConManager != null)
    //    {
    //        routingTable[ipAddress] = $"Interface {pcConManager.IPAddress}";
    //    }
    //}

    public void ReceiveFrame(byte[] frame, string sourceInterface) // falta parametro interfaz
    {
        buffer.AddLine($"Router recibe frame en interface {sourceInterface}");

        NetworkPacket packet = DataLinkLayer.Decapsulate(frame);

        //ForwardPacket(packet, sourceInterface);
        // Enviar el paquete al controlador del router para manejarlo
        ReceivePacket(packet, sourceInterface);
    }

    //public void ForwardPacket(NetworkPacket packet, string sourceInterface)
    //{
    //    // revisar la tabla de enrutamiento y reenviar a donde corresponda
    //    string destInterface = routingTable[packet.DestinationAddress];
    //}

    public void ReceivePacket(NetworkPacket packet, string sourceInterface)
    {
        buffer.AddLine($"Router recibe packet desde {packet.SourceAddress} a {packet.DestinationAddress} en interface {sourceInterface}");

        if (!routingTable.ContainsKey(packet.DestinationAddress))
        {
            buffer.AddLine($"Router no encuentra el dispositivo de destino {packet.DestinationAddress}.");
            HandleUnreachable(packet.SourceAddress);
            return;
        }

        // Si el paquete es ICMP, manejar el ping
        if (packet.Data.Length >= 8 && packet.Data[0] == 8)  // Echo Request
        {
            ForwardPing(packet, sourceInterface);
        }
        else if (packet.Data.Length >= 8 && packet.Data[0] == 0)  // Echo Response
        {
            ForwardResponse(packet, sourceInterface);
        }
        // Agregar lógica para otros tipos de paquetes según sea necesario
    }

    private void ForwardPing(NetworkPacket packet, string sourceInterface)
    {
        buffer.AddLine($"Router reenvia ping desde {packet.SourceAddress} a {packet.DestinationAddress}...");

        // Obtener el dispositivo de destino
        var destinationDevice = routingTable[packet.DestinationAddress];

        if (destinationDevice != null)
        {
            // Simular la transmisión del paquete al dispositivo de destino
            byte[] frame = DataLinkLayer.Encapsulate(packet);
            destinationDevice.GetComponent<INetworkManager>().ReceiveFrame(frame, sourceInterface);
        }
        else
        {
            buffer.AddLine($"Router no encuentra el dispositivo de destino {packet.DestinationAddress}. Packet descartado.");
        }
    }

    private void ForwardResponse(NetworkPacket packet, string sourceInterface)
    {
        buffer.AddLine($"Router reenvia response ping desde {packet.SourceAddress} a {packet.DestinationAddress}...");

        // Obtener el dispositivo de destino
        var destinationDevice = routingTable[packet.DestinationAddress];

        if (destinationDevice != null)
        {
            // Simular la transmisión del paquete al dispositivo de destino
            byte[] frame = DataLinkLayer.Encapsulate(packet);
            destinationDevice.GetComponent<INetworkManager>().ReceiveFrame(frame, sourceInterface);
        }
        else
        {
            buffer.AddLine($"Router no encuentra el dispositivo de destino {packet.DestinationAddress}. Packet descartado.");
        }
    }

    private void HandleUnreachable(string targetIPAddress)
    {
        // Crear un paquete ICMP
        var icmpPacket = new IcmpPacket
        {
            Type = 3, // Destination Unreachable
            Code = 0,
            Identifier = (ushort)UnityEngine.Random.Range(0, ushort.MaxValue),
            SequenceNumber = (ushort)UnityEngine.Random.Range(0, ushort.MaxValue)
        };

        // Convertir el paquete ICMP a bytes
        byte[] icmpData = IcmpPacket.Serialize(icmpPacket);

        // Crear un paquete de red con el paquete ICMP
        NetworkPacket packet = new NetworkPacket
        {
            SourceAddress = IPAddress,
            DestinationAddress = targetIPAddress,
            Data = icmpData
        };

        // Simular la transmisión del paquete a través de la capa de enlace de datos
        byte[] frame = DataLinkLayer.Encapsulate(packet);

        routingTable[targetIPAddress].GetComponent<INetworkManager>().ReceiveFrame(frame, "");
        buffer.AddLine($"Enviando packet unreachable a {targetIPAddress}");
    }

}
