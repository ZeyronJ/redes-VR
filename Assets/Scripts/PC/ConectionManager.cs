using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Assertions;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;

namespace UnityEngine
{
    public interface IConnectionManager
    {
        public abstract void handleSignal(BaseInteractionEventArgs args);
        public abstract void handleConnection(BaseInteractionEventArgs args);
    }

    public interface INetworkManager
    {
        string IPAddress
        {
            get; set;
        }
        public abstract void ReceiveFrame(byte[] frame, string sourceInterface);
    }
}