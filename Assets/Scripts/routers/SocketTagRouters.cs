using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketTagRouters : XRSocketInteractor
{
    public string targetTag = string.Empty;

    public bool isConnected = false;

    public CableRef cable;
    public SignalRouters scriptRouter;

    private new void Start()
    {
        scriptRouter = transform.parent.gameObject.GetComponent<SignalRouters>();
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.tag == targetTag;
    }
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        bool canSelect = base.CanSelect(interactable) && interactable.transform.tag == targetTag;
        if (canSelect && !isConnected)
        {
            isConnected = true;

            cable = interactable.transform.GetComponent<CableRef>();
            Debug.Log("if");
            //Debug.Log("cable id: " + cableId);
            //if(scriptRouter != null) scriptRouter.CheckConecctions();
            scriptRouter.CheckConnections();
        }
        else if (!canSelect && isConnected)
        {
            isConnected = false;
            Debug.Log("else");
            //if (scriptRouter != null) scriptRouter.CheckConecctions();
            scriptRouter.CheckConnections();
        }

        return canSelect;
    }
}

