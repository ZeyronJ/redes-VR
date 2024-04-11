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
using System.Linq;

public class PCConManager : MonoBehaviour, IConnectionManager, INetworkManager
{
    public ConsoleBuffer buffer;
    private static int _index = 0;
    public List<GameObject> connectedDevices = new List<GameObject>();
    private string _IPAddress = "192.168.0.0";
    public string IPAddress
    {
        get { return _IPAddress; }
        set { _IPAddress = value; }
    }

    private void Start()
    {
        buffer = new ConsoleBuffer(id: _index);
        IPAddress = "192.168.0." + _index.ToString();
        _index++;
        buffer.AddLine(_IPAddress);
        //var router = GameObject.FindGameObjectWithTag("Router");
        //if(router != null)
        //{
        //    var routerConManager = router.GetComponent<RouterConManager>();
        //    routerConManager?.ConfigureIPAddress(gameObject, IPAddress);
        //}
    }

    public void SendPacket(NetworkPacket packet, GameObject router) //falta parametro de interface
    {
        byte[] frame = DataLinkLayer.Encapsulate(packet);
        router.GetComponent<RouterConManager>().ReceiveFrame(frame, "");
    }

    public void handleConnection(BaseInteractionEventArgs args)
    {
        Debug.Log($"Entered handleConnection. Object: {gameObject.name}, Tag: {gameObject.tag}");

        if (args is SelectEnterEventArgs enterArgs)
        {
            var interactable = enterArgs.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
            interactable.isConnected = true;

            interactable.device = gameObject;

            var socket = interactable.extremoCable.GetComponent<GrabInteractableCable>();
            if (!socket.isConnected) return;

            if(socket.device.tag == "Screen")
            {
                ScreenConManager screen = socket.device.GetComponent<ScreenConManager>();
                screen.PcAsociado = gameObject;
            }

            if (!connectedDevices.Contains(socket.device)) connectedDevices.Add(socket.device);

            Debug.Log($"HC Object: {gameObject.name}, Tag: {gameObject.tag}, Interactable Tag: {interactable.gameObject.tag}, Socket Tag: {socket.device.tag}");
            if (interactable.gameObject.tag == "Cable-Con")
            {
                if(socket.device.tag == "Router")
                {
                    var screen = connectedDevices.Find(p => p.gameObject.tag == "Screen");
                    screen.GetComponent<ScreenConManager>().setBuffer(socket.device.GetComponent<RouterConManager>().buffer);
                }
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

            if (connectedDevices.Contains(socket.device)) connectedDevices.Remove(socket.device);

            Debug.Log($"HCD Object: {gameObject.name}, Tag: {gameObject.tag}, Interactable Tag: {interactable.gameObject.tag}, Socket Tag: {socket.device.tag}");
            if (interactable.gameObject.tag == "Cable-Con")
            {
                if (socket.device.tag == "Router")
                {
                    var screen = connectedDevices.Find(p => p.gameObject.tag == "Screen");
                    screen.GetComponent<ScreenConManager>().setBuffer(buffer);

                }
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
            var socket = interactable.GetComponent<GrabInteractableCable>();

            if (!connectedDevices.Contains(interactable.device)) connectedDevices.Add(interactable.device);


            Debug.Log($"HS Object: {gameObject.name}, Tag: {gameObject.tag}, Interactable Tag: {interactable.gameObject.tag}, Socket Tag: {socket.device.tag}");
            if (interactable.gameObject.tag == "Cable-Con")
            {
                if (socket.device.tag == "Router")
                {
                    var screen = connectedDevices.Find(p => p.gameObject.tag == "Screen");
                    screen.GetComponent<ScreenConManager>().setBuffer(socket.device.GetComponent<RouterConManager>().buffer);

                }
            }
        }
        else if (args is SelectExitEventArgs exitArgs)
        {
            var interactable = exitArgs.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
            var socket = interactable.GetComponent<GrabInteractableCable>();

            if (connectedDevices.Contains(interactable.device)) connectedDevices.Remove(interactable.device);

            Debug.Log($"HSD Object: {gameObject.name}, Tag: {gameObject.tag}, Interactable Tag: {interactable.gameObject.tag}, Socket Tag: {socket.device.tag}");
            if (interactable.gameObject.tag == "Cable-Con")
            {
                if (socket.device.tag == "Router")
                {
                    var screen = connectedDevices.Find(p => p.gameObject.tag == "Screen");
                    screen.GetComponent<ScreenConManager>().setBuffer(buffer);

                }
            }
        }
    }

    public void HandleCommand(string command)
    {
        string[] tokens = command.ToLower().Split(" ");
        if (tokens == null || tokens.Length == 0) 
        {
            Debug.Log("[PC] Comando no valido o vacio!");
            return;
        }

        switch(tokens[0])
        {
            case "ping":
            {
                if (tokens[1] == null || tokens[1].Length == 0)
                {
                    Debug.Log("[PC] Ping target no valido o vacio!");
                    return;
                }
                buffer.AddLine(command);
                Ping(tokens[1]);
            }break;
            case "...":
            { 
                buffer.AddLine(command);
            }break;
            case "set":
            {
                switch (tokens[1])
                {
                    case "ip":
                        {
                                buffer.AddLine(command);
                                SetIP(tokens[2]);
                        }
                        break;
                }
            }break;
            default:
                return;
        }
    }

    public void Ping(string targetIPAddress)
    {
        // Crear un paquete ICMP para el ping
        var icmpPacket = new IcmpPacket
        {
            Type = 8, // Echo Request
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

        buffer.AddLine($"{IPAddress} envía ping a {targetIPAddress}");

        //SendPacket(packet, connectedDevices.Find(x => x.tag == "Router"));  // Enviar el paquete al router (null para la interfaz)

        // Simular la transmisión del paquete a través de la capa de enlace de datos
        byte[] frame = DataLinkLayer.Encapsulate(packet);

        // Obtener el router conectado
        var router = connectedDevices.Find(x => x.tag == "Router");

        // Enviar el paquete encapsulado al router
        router.GetComponent<INetworkManager>().ReceiveFrame(frame, "");

    }

    public void ReceiveFrame(byte[] frame, string sourceInterface) // falta parametro interfaz
    {
        buffer.AddLine($"PC {IPAddress} recibe frame en interface {sourceInterface}");

        NetworkPacket packet = DataLinkLayer.Decapsulate(frame);

        //ForwardPacket(packet, sourceInterface);
        // Enviar el paquete al controlador del router para manejarlo
        ReceivePacket(packet, sourceInterface);
    }

    public void ReceivePacket(NetworkPacket packet, string sourceInterface)
    {
        buffer.AddLine($"PC {IPAddress} recibe packet desde {packet.SourceAddress} a {packet.DestinationAddress} en interface {sourceInterface}");

        // Si el paquete es ICMP, manejar el ping
        if (packet.Data.Length >= 8 && packet.Data[0] == 8)  // Echo Request
        {
            HandlePing(packet);
        }
        else if(packet.Data.Length >= 8 && packet.Data[0] == 0) // Echo Response
        {
            HandleResponse(packet);
        }else if(packet.Data.Length >= 8 && packet.Data[0] == 3) // Destination Unreachable
        {
            HandleUnreachable(packet);
        }
        // Agregar lógica para otros tipos de paquetes según sea necesario
    }

    private void HandlePing(NetworkPacket packet)
    {
        buffer.AddLine($"PC {IPAddress} maneja ping desde {packet.SourceAddress}");
        ResponsePing(packet.SourceAddress);
    }

    private void HandleResponse(NetworkPacket packet)
    {
        buffer.AddLine($"PC {IPAddress} maneja respuesta de ping deade {packet.SourceAddress}");
    }

    private void HandleUnreachable(NetworkPacket packet)
    {
        buffer.AddLine($"PC {IPAddress} maneja destino inalcanzable");
    }

    public void SetIP(string nuevaDireccionIP)
    {
        _IPAddress = nuevaDireccionIP;
        buffer.AddLine($"Nueva dirección IP configurada: {_IPAddress}");
    }

    public void ResponsePing(string targetIPAddress)
    {
        // Crear un paquete ICMP para el ping
        var icmpPacket = new IcmpPacket
        {
            Type = 0, // Echo Reply
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

        buffer.AddLine($"{IPAddress} envía respuesta a ping de {targetIPAddress}");

        //SendPacket(packet, connectedDevices.Find(x => x.tag == "Router"));  // Enviar el paquete al router (null para la interfaz)

        // Simular la transmisión del paquete a través de la capa de enlace de datos
        byte[] frame = DataLinkLayer.Encapsulate(packet);

        // Obtener el router conectado
        var router = connectedDevices.Find(x => x.tag == "Router");

        // Enviar el paquete encapsulado al router
        router.GetComponent<INetworkManager>().ReceiveFrame(frame, "");
    }
}
