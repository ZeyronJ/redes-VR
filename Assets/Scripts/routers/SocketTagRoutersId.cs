using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketTagRoutersId : XRSocketInteractor
{
    public string targetTag = string.Empty;

    public bool isConnected = false;

    public int cableId = 0;

    public SignalRoutersId scriptRouter;

    private new void Start()
    {
        scriptRouter = transform.parent.gameObject.GetComponent<SignalRoutersId>();
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

            CableId cable = interactable.transform.GetComponent<CableId>();
            cableId = cable.id;
            Debug.Log("if");
            //Debug.Log("cable id: " + cableId);
            //if(scriptRouter != null) scriptRouter.CheckConecctions();
            scriptRouter.CheckConecctions();
        }
        else if (!canSelect && isConnected)
        {
            isConnected = false;
            Debug.Log("else");
            //if (scriptRouter != null) scriptRouter.CheckConecctions();
            scriptRouter.CheckConecctions();
        }

        return canSelect;
    }
}

