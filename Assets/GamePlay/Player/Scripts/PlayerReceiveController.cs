using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReceiveController : MonoBehaviour
{
    // Player Objects
    [Header("Player References")]

    // object anchors for where the object appears on the player
    [SerializeField] private Transform receiveAnchor;
    [SerializeField] private Transform receiveToolAnchor;
    [SerializeField] private Transform anchorToUse;

    // player controllers
    [SerializeField] private PlayerActionState actionState; // what is the player currently doing?
    [SerializeField] private Animator animator; // for receive animation
    [SerializeField] private PlayerToolState toolState; // which tool is the player holding, if any?
    [SerializeField] private InventoryManager inventoryManager; // player inventory
    [SerializeField] private PlayerInventoryState inventoryState; // does the player have the satchel?
    [SerializeField] private PlayerCarryController carryController; // player carry controller to pass responsibility to

    [Header("Receiveable Object")]
    [SerializeField] private ReceivableObjectController receivableObject; // receivable object to use in this script
    [SerializeField] private bool receivingObject = false; // bool of whether the object is in the receive state or has been received
    [SerializeField] private bool awaitingCarryHandoff = false; // is the player holding an object to be carried?
    

    [Header("Notification")]
    [SerializeField] private NotificationUI notificationUI; // show/hide notification


    private void Awake()
    {
        // get all player components on awake
        if (!animator) animator = GetComponent<Animator>();
        if (!actionState) actionState = GetComponent<PlayerActionState>();
        if (!toolState) toolState = GetComponent<PlayerToolState>();
        if (!inventoryManager) inventoryManager = GetComponent<InventoryManager>();
        if (!inventoryState) inventoryState = GetComponent<PlayerInventoryState>();
        if (!carryController) carryController = GetComponent<PlayerCarryController>();
    }

    // set object reference when in the object's trigger zone
    public void SetCurrentReceivableObject(ReceivableObjectController obj)
    {
        receivableObject = obj;

    }

    // clear object reference when leaving object's trigger zone
    public void ClearCurrentReceivableObject(ReceivableObjectController obj)
    {
        if (receivingObject) return; // if player is holding a received object, don't clear it
        if (receivableObject != null && receivableObject == obj)
            receivableObject = null;

    }

    // public method for PlayerInteractionController
    public bool Receive()
    {
        if (actionState.IsBusy && actionState.State != PlayerState.Receiving)
            return false; // is doing something other than receiving, bail out

        if (receivableObject == null) return false; // nothing to receive, receiving = false


        // --------------- INTERACTION 2. ACCEPT OBJECT OR HAND OFF TO CARRY --------------- //
        // If the object has been received (is currently being held above the head by the player)
        if (receivingObject)
        {
            return AcceptOrHandOff();

        }

        // ----- INTERACTION 1: RECEIVE OBJECT WHEN HANDS ARE EMPTY ---- //

        return ReceiveWhenEmpty();


    }

    // first interaction
    private bool ReceiveWhenEmpty()
    {
        anchorToUse = receivableObject.IsTool ? receiveToolAnchor : receiveAnchor; // use the tool anchor if a tool, otherwise the receive anchor
        string animationToShow = receivableObject.IsTool ? "ReceiveTool" : "Receive";

        // Only check discovery for ordinary resources.
        if (!receivableObject.IsTool && !receivableObject.IsSatchel)
        {
            bool hasDiscovered = inventoryManager.HasDiscovered(receivableObject.ResourceType);

            if (hasDiscovered)
            {
                return false; // do not receive it, go straight to carry
            }

            // First-time resource discovery.
            inventoryManager.DiscoverResource(receivableObject.ResourceType);
            awaitingCarryHandoff = true;
        }

        // Only receive the satchel if it's unlocked
        if (receivableObject.IsSatchel)
        {
            bool satchelUnlocked = WorldState.SatchelUnlocked;
            if (!satchelUnlocked) return false; // don't receive it

        }

        animator.SetTrigger(animationToShow); // play appropriate receive animation
        receivableObject.ReceiveObject(anchorToUse); // place object at appropriate receive anchor
        actionState.SetActionState(PlayerState.Receiving); // set player as busy receiving
        receivingObject = true; // store bool for next interaction

        if (receivableObject.IsTool)
        {
            notificationUI.Show($"You have received the {receivableObject.ToolType}!"); // show notification
        }
        else
        {
            notificationUI.Show($"You have received the {receivableObject.ResourceName}!"); // show notification
        }

        return true;
    }

    // second interaction
    private bool AcceptOrHandOff()
    {
        // --- newly discovered resource --- //
        if (!receivableObject.IsTool && !receivableObject.IsSatchel)
        {
            if (awaitingCarryHandoff)
            {
                if(!HandOffToCarry()) return false; // start using the object's carryable object controller

                notificationUI.Hide(); // as soon as the player accepts the object, hide the "You have received ... !" notifcation

                awaitingCarryHandoff = false;
                receivingObject = false;
                receivableObject = null;

                return true; // stop here, carryable object controller's got it now
            }
        }

        notificationUI.Hide(); // as soon as the player accepts the object, hide the "You have received ... !" notifcation

        if (receivableObject.IsTool)
        {
            // get the tool type on the receivable object tool
            toolState.EquipTool(receivableObject.ToolType); 
            inventoryManager.AddTool(receivableObject.ToolType);
        }

        if (receivableObject.IsSatchel)
        {
            inventoryState.ReceiveSatchel();
        }

        receivableObject.AcceptObject(); // only accept the object
        PlayIdle(); // return to idle once tool is accepted

        receivingObject = false; // the tool has been accepted, player is no longer holding the object above their head
        receivableObject = null; // set null status since the object has been accepted

        return true;
    }
    

    private void PlayIdle()
    {
        actionState.ClearActionState(); // player no longer busy receiving

        animator.SetFloat("LastX", 0);
        animator.SetFloat("LastY", -1);
        animator.Play("Idle");
    }


    // pass this object to carryable object controller
    private bool HandOffToCarry()
    {
        // bail out if nothing to hand off
        if (receivableObject == null) return false;
        if (carryController == null) return false;
        if (receivableObject.CarryableController == null) return false;

        animator.ResetTrigger("Receive"); // stop receiving object animation
        animator.Play("Idle"); // reset player back to idle

        return carryController.StartCarrying(receivableObject.CarryableController);

    }

}
