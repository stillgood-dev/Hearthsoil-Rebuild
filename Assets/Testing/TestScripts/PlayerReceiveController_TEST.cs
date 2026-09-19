using UnityEngine;

public class PlayerReceiveController_TEST : MonoBehaviour
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
    [SerializeField] private bool inInventory = false; // is the current receiveable object in our inventory yet?

    [Header("Notification")]
    [SerializeField] private NotificationUI notificationUI; // show/hide notification
    [SerializeField] private ResourceChoiceUI resourceChoiceUI; // notification choices

    public bool InInventory => inInventory; // other scripts can read if this object is in the player inventory

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

        // ---- INTERACTION 3. DECIDE WHAT TO DO WHEN RESOURCE CHOICE UI IS OPEN ---- //
        if (resourceChoiceUI.IsOpen)
        {
            // pipeline when Resource Choices is open
            switch (resourceChoiceUI.CurrentChoice)
            {
                // drop resource
                case ResourceChoiceUI.ResourceChoice.Drop:
                    DropResource();
                    break;


                // eat resource
                case ResourceChoiceUI.ResourceChoice.Eat:
                    Debug.Log("Eat selected");
                    break;

                // store resource
                case ResourceChoiceUI.ResourceChoice.Store:
                    Debug.Log("Store selected");
                    break;
            }

            return true; // receiving = true
        }

        // --------------- INTERACTION 2. ACCEPT OBJECT OR HAND OFF TO CARRY --------------- //
        // If the object has been received (is currently being held above the head by the player)
        if (receivingObject)
        {
            notificationUI.Hide(); // as soon as the player accepts the object, hide the "You have received ... !" notifcation

            // ---- tool ---- //
            if (receivableObject.IsTool)
            {
                receivableObject.AcceptObject(); // only accept the object
                toolState.EquipTool(receivableObject.ToolType); // get the tool type on the receivable object tool
                PlayIdle(); // return to idle once tool is accepted

                receivingObject = false; // the tool has been accepted, player is no longer holding the object above their head
                receivableObject = null; // set null status since the object has been accepted

                return true;

            }

            // --- newly discovered resource --- //
            if (awaitingCarryHandoff)
            {
                HandOffToCarry(); // start using the object's carryable object controller

                awaitingCarryHandoff = false;
                receivingObject = false;
                receivableObject = null;
               
                return true; // stop here, carryable object controller's got it now
            }

            // ---- existing resource ---- //
            // change this later because we want the object to be handed straight to carry controller
            bool canEat = receivableObject.IsEdible;
            bool canDrop = true;
            bool canStore =
                receivableObject.IsStorable &&
                inventoryState.HasSatchel;

            resourceChoiceUI.Show(
                "What would you like to do?",
                canEat,
                canDrop,
                canStore
            );

            return true;

        }

        // ----- INTERACTION 1: RECEIVE OBJECT WHEN HANDS ARE EMPTY ---- //

        anchorToUse = receivableObject.IsTool ? receiveToolAnchor : receiveAnchor; // use the tool anchor if a tool, otherwise the receive anchor

        // ---- tool --- //
        if (receivableObject.IsTool)
        {
            animator.SetTrigger("ReceiveTool"); // show player receiving tool animation
            receivableObject.ReceiveObject(anchorToUse); // put the tool in the tool anchor
            notificationUI.Show($"You have received the {receivableObject.ToolType}!"); // show notification
            actionState.SetActionState(PlayerState.Receiving); // set player as busy receiving
            receivingObject = true; // store bool for next interaction
            return true; // stop here
        }

        // ---- non-tool resource --- //
        else
        {
            // ---- existing resource pipeline ----- //

            // determine if the player has already discovered the resource
            bool hasDiscovered = inventoryManager.HasDiscovered(receivableObject.ResourceType);

            // if the resource has already been discovered once in the game
            if (hasDiscovered)
            {
                animator.SetTrigger("Receive"); // show player receiving object animation
                receivableObject.ReceiveObject(anchorToUse); // put the object in the receive anchor

                actionState.SetActionState(PlayerState.Receiving); // player is busy receiving the object, can't do anything else
                receivingObject = true; // player is receiving the object, store for next time the player hits interact

                // set receivable object parameters, can you eat, drop, or store the object?
                bool canEat = receivableObject.IsEdible;
                bool canDrop = true;
                bool canStore = 
                    receivableObject.IsStorable &&
                    inventoryState.HasSatchel;

                // show choices based on what you can do with the object, this now sets ResourceChoiceUI.IsOpen to true
                resourceChoiceUI.Show(
                   "What would you like to do?",
                   canEat,
                   canDrop,
                   canStore
               );

                return true;
            }

            // ----- new resource pipeline ----- //
            animator.SetTrigger("Receive"); // show player receiving object animation
            receivableObject.ReceiveObject(anchorToUse); // put the object in the receive anchor
            notificationUI.Show($"You have received the {receivableObject.ReceiveObjectName}!"); // show notification

            inventoryManager.DiscoverResource(receivableObject.ResourceType);
            awaitingCarryHandoff = true; // store for next interaction

            actionState.SetActionState(PlayerState.Receiving); // player is busy receiving
            receivingObject = true; // store for next interaction

            return true;
        }


    }

    public void DropResource()
    {
        if (receivableObject == null) return;

        Vector3 dropPosition = transform.position + new Vector3(0f, -0.25f, 0f);

        receivableObject.DropObject(dropPosition);

        resourceChoiceUI.Hide();

        receivingObject = false;
        receivableObject = null;

        PlayIdle();
    }

    private void PlayIdle()
    {
        actionState.ClearActionState(); // player no longer busy receiving

        animator.SetFloat("LastX", 0);
        animator.SetFloat("LastY", -1);
        animator.Play("Idle");
    }


    // pass this object to carryable object controller
    private void HandOffToCarry()
    {
        // bail out if nothing to hand off
        if (receivableObject == null) return;
        if (carryController == null) return;
        if (receivableObject.CarryableController == null) return;

        animator.ResetTrigger("Receive"); // stop receiving object animation
        animator.Play("Idle"); // reset player back to idle

        carryController.StartCarrying(receivableObject.CarryableController);
    }


}
