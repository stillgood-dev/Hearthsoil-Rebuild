using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Current Interaction")]
    [SerializeField] private HearthsoilSign currentSign;
    [SerializeField] private NPCDialogueController currentNPC;

    [Header("Player Refs")]
    [SerializeField] private PlayerActionCoordinator actionCoordinator;
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private PlayerEnvironmentState playerEnvironment;
    [SerializeField] private PlayerInventoryState inventoryState;


    [Header("Inventory")]
    [SerializeField] private InventoryUIController inventoryUI;


    [Header("Debug")]
    [SerializeField] private bool showDebug;

    private void Awake()
    {
        if (!playerActionState) playerActionState = GetComponent<PlayerActionState>();
        if (!playerEnvironment) playerEnvironment = GetComponent<PlayerEnvironmentState>();  
        if (!inventoryState) inventoryState = GetComponent<PlayerInventoryState>();
        if (!inventoryUI) inventoryUI = GetComponent<InventoryUIController>();
        if (!actionCoordinator) actionCoordinator = GetComponent<PlayerActionCoordinator>();

    }

    // -------- on interact button ---------- //
    public void OnInteract()
    {
        if (showDebug) Debug.Log("INTERACT PRESSED");

        // 1. Handle sign interaction.
        if (currentSign != null)
        {
            actionCoordinator.HandleSign(currentSign);
            return; // don't fall through to next action
        }

        // 2. Handle NPC dialogue
        if (currentNPC != null)
        {
            actionCoordinator.HandleDialogue(currentNPC);
            return;
        }

        // 3. Open a door if there's a door to open.
        if (actionCoordinator.HandleDoor()) return;

        // 4. Light a candle or lantern.
        if (actionCoordinator.HandleLight()) return;

        // 5. Block other busy states, but allow multi-step
        // Receiving and Carrying interactions to continue.
        if (playerActionState.IsBusy &&
            playerActionState.State != PlayerState.Receiving &&
            playerActionState.State != PlayerState.Carrying)
        {
            if (showDebug)
                Debug.Log("Player is busy: " + playerActionState.State);

            return;
        }

        // 6. If already carrying, Carry gets first priority.
        if (playerActionState.State == PlayerState.Carrying)
        {
            if (actionCoordinator.HandleCarry()) return;
            
        }

        // 7. Otherwise let Receive handle the interaction.
        if (actionCoordinator.HandleReceive()) return;


        // 8. Carry interaction.
        if (actionCoordinator.HandleCarry()) return;


        // 9. Try receiving a new object if Carry didn't handle it.
        if (actionCoordinator.HandleReceive()) return;

        // 10. Tool use last.

        // Only allow tools if player is outside.
        if (playerEnvironment != null &&
            playerEnvironment.PlayerEnvironment == PlayerEnvironment.Outside)
        {
            actionCoordinator.HandleTool();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (showDebug) Debug.Log("Entered trigger: " + other.name);

        HearthsoilSign sign = other.GetComponentInParent<HearthsoilSign>();

        if (sign != null)
        {
            currentSign = sign;

            if (showDebug) Debug.Log("Current sign set to: " + sign.name);
        }

        NPCDialogueController npc = other.GetComponentInParent <NPCDialogueController>();

        if (npc != null)
        {
            currentNPC = npc;

            if (showDebug)
                Debug.Log("NPC detected: " + npc.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (showDebug) Debug.Log("Exited trigger: " + other.name);

        HearthsoilSign sign = other.GetComponentInParent<HearthsoilSign>();

        if (sign != null && currentSign == sign)
        {
            if (currentSign.IsOpen)
            {
                currentSign.Close();
                playerActionState.ClearActionState();
            }

            currentSign = null;

            if (showDebug) Debug.Log("Current sign cleared");
        }

        NPCDialogueController npc = other.GetComponentInParent<NPCDialogueController>();

        if (npc != null && currentNPC == npc)
        {
            currentNPC = null;
        }
    }
}