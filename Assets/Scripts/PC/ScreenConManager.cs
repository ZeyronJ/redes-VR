using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Assertions;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using System.Net.Sockets;
using NSU;
using System.Linq;

public class ScreenConManager : MonoBehaviour, IConnectionManager
{
    private ConsoleBuffer buffer;
    private TextMeshProUGUI textMesh;
    public GameObject PcAsociado;

    private void Start()
    {
        // Busca el componente por nombre
        var go = transform.Find("Monitor/Plane/Canvas/ScreenText");
        if (go == null)
        {
            Debug.LogError("No se encontro el ScreenText!");
        }
        else
        {
            textMesh = go.GetComponent<TextMeshProUGUI>();
            // Verifica si se encontró el componente
            if (textMesh == null)
            {
                Debug.LogError($"No se pudo encontrar el componente de ScreenText!");
            }
        }
    }

    private void OnDisable()
    {
        if (buffer != null)
        {
            this.buffer.OnBufferUpdated -= HandleBufferUpdated;
        }
    }
    private void HandleBufferUpdated()
    {
        string[] lines = this.buffer.GetLines();
        string text = string.Join("\n", lines);
        textMesh.text = text;
    }

    public void setBuffer(ConsoleBuffer otherBuffer)
    {
        if(buffer != null)
        {
            buffer.OnBufferUpdated -= HandleBufferUpdated;
        }
        buffer = otherBuffer;
        if (buffer != null) buffer.OnBufferUpdated += HandleBufferUpdated;
        else Debug.Log("no buffer");
        HandleBufferUpdated();
    }

    private void clearBuffer()
    {
        if (this.buffer != null)
        {
            this.buffer.OnBufferUpdated -= HandleBufferUpdated;
            this.buffer = null;
        }
        Debug.Log("clear con");
        textMesh.text = "No signal";
    }

    public void addLine(string line)
    {
        if(this.buffer != null)
        {
            this.buffer.AddLine(line);
        }
    }

    public void clearScreen()
    {
        if (this.buffer != null)
        {
            Debug.Log("clear1");
            this.buffer.ClearBufferExceptLast();
        }
        else
        {
            Debug.Log("clear2");
        }
    }

    private void handleDevice(GrabInteractableCable other)
    {
        switch (other.device.tag)
        {
            case "PC":
                {
                    var pc = other.device.GetComponent<PCConManager>();
                    if (pc == null) Debug.Log("no pc");
                    if (pc.buffer == null) Debug.Log("no buffer1");
                    setBuffer(pc.buffer);
                }
                break;
        }
    }

    public void handleConnection(BaseInteractionEventArgs args)
    {
        if (args is SelectEnterEventArgs enterArgs)
        {
            var interactable = enterArgs.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
            interactable.isConnected = true;
            interactable.device = gameObject;

            var socket = interactable.extremoCable.GetComponent<GrabInteractableCable>();
            if (!socket.isConnected) return;

            PcAsociado = socket.device;

            var other = socket.device.GetComponent<IConnectionManager>();
            other?.handleSignal(enterArgs);

            handleDevice(socket);
        }
        else if (args is SelectExitEventArgs exitArgs)
        {
            var interactable = exitArgs.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
            interactable.isConnected = false;
            interactable.device = null;

            var socket = interactable.extremoCable.GetComponent<GrabInteractableCable>();
            if (!socket.isConnected) return;

            var other = socket.device.GetComponent<IConnectionManager>();
            other?.handleSignal(exitArgs);

            clearBuffer();
        }
    }

    public void handleSignal(BaseInteractionEventArgs args)
    {
        if (args is SelectEnterEventArgs enterArgs)
        {
            var interactable = enterArgs.interactableObject.transform.gameObject.GetComponent<GrabInteractableCable>();
            handleDevice(interactable);
        }
        else if (args is SelectExitEventArgs exitArgs)
        {
            clearBuffer();
        }
    }
}
