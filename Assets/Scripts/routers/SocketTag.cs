using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWitchTagCheck : XRSocketInteractor
{
    public string targetTag = string.Empty;

    public bool isConnected = false;

    public CableRef cable;
    public Receiver scriptSignalPC;

    new void Start()
    {
        scriptSignalPC = transform.parent.gameObject.GetComponent<Receiver>();
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.tag == targetTag;
    }
    public override bool CanSelect(IXRSelectInteractable interactable) {
        bool canSelect = base.CanSelect(interactable) && interactable.transform.tag == targetTag;
        if (canSelect && !isConnected)
        {
            isConnected = true;

            cable = interactable.transform.GetComponent<CableRef>();
            //cable=cable.extremoCable;
            Debug.Log("if");
            //if(scriptSignalPC != null) scriptSignalPC.CheckConnections();
            scriptSignalPC.CheckConnections();
        }
        else if(!canSelect && isConnected)
        {
            isConnected = false;
            Debug.Log("else");
            //if (scriptSignalPC != null) scriptSignalPC.CheckConnections();
            scriptSignalPC.CheckConnections();
        }

        return canSelect; 
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