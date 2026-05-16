using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReceiveController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform receiveAnchor;
    [SerializeField] private Transform receiveToolAnchor;
    [SerializeField] private Transform anchorToUse;
    [SerializeField] private PlayerActionState actionState;
    [SerializeField] private Animator animator;
    [SerializeField] private string receiveAnimationName;
    [SerializeField] private PlayerToolState toolState;

    [Header("Receiveable Object")]
    [SerializeField] private ReceivableObjectController receivableObject;
    [SerializeField] private bool hasReceivedObject = false;
    [SerializeField] private bool inInventory = false;

    public bool InInventory => inInventory;

    private void Awake()
    {
        if(!animator) animator = GetComponent<Animator>();
        if(!actionState) actionState = GetComponent<PlayerActionState>();
        if (!toolState) toolState = GetComponent<PlayerToolState>();
    }

    // set object reference when in the object's trigger zone
    public void SetCurrentReceivableObject(ReceivableObjectController obj)
    {
        receivableObject = obj;
        
    }

    // clear object reference when leaving object's trigger zone
    public void ClearCurrentReceivableObject(ReceivableObjectController obj)
    {
        if (hasReceivedObject) return;
        if (receivableObject != null && receivableObject == obj)
            receivableObject = null;

    }

    // returns a bool that can be used by PlayerInteractionController to tell it if you're receiving
    public bool Receive()
    {
        if (actionState.IsBusy && actionState.State != PlayerState.Receiving) return false;
        if (receivableObject == null) return false;

        // --- accept object --- //
        if (hasReceivedObject)
        {
            receivableObject.AcceptObject();

            if (receivableObject.IsTool)
            {
                toolState.EquipTool(receivableObject.ToolType);
            }

            inInventory = true;
            actionState.ClearActionState();

            animator.SetFloat("LastX", 0);
            animator.SetFloat("LastY", -1);
            animator.Play("Idle");

            hasReceivedObject = false;
            receivableObject = null; // IMPORTANT
            return true;
        }

        // --- receive object initially --- //
        anchorToUse = receivableObject.IsTool ? receiveToolAnchor : receiveAnchor;

        animator.SetTrigger(receiveAnimationName);
        receivableObject.ReceiveObject(anchorToUse);

        actionState.SetActionState(PlayerState.Receiving);
        hasReceivedObject = true;

        return true;
    }

}
