//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketSlotWithTag : XRSocketInteractor
{
    public string targetTag = string.Empty;

    public bool isConnected = false;

    public override bool CanHover(IXRHoverInteractable interactable) {
        return base.CanHover(interactable) && interactable.transform.CompareTag(targetTag);
    }
    public override bool CanSelect(IXRSelectInteractable interactable) {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(targetTag);
    }
}

//public override bool CanHover(XRBaseInteractable interactable)
//{
//    return base.CanHover(interactable) && MatchUsingTag(interactable);
//}

//public override bool CanSelect(XRBaseInteractable interactable)
//{
//    isConnected = base.CanSelect(interactable) && MatchUsingTag(interactable);

//    return base.CanSelect(interactable) && MatchUsingTag(interactable);
//}

//private bool MatchUsingTag(XRBaseInteractable interactable)
//{
//    return interactable.CompareTag(targetTag);
//}