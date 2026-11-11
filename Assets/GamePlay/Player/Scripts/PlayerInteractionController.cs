using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Current Interaction")]
    [SerializeField] private HearthsoilSign currentSign;
    [SerializeField] private NPCDialogueController currentNPC;

    [Header("Player Refs")]
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private PlayerToolState toolState;
    [SerializeField] private PlayerEnvironmentState playerEnvironment;

    [Header("Action Controllers")]
    [SerializeField] private PlayerLightSourceController lightController;
    [SerializeField] private PlayerDoorController doorController;
    [SerializeField] private PlayerReceiveController receiveController;
    [SerializeField] private PlayerCarryController carryController;
    [SerializeField] private PlayerAxeController axeController;
    [SerializeField] private PlayerMacheteController macheteController;
    [SerializeField] private PlayerHoeController hoeController;


    [Header("Debug")]
    [SerializeField] private bool showDebug;

    private void Awake()
    {
        if (!playerActionState) playerActionState = GetComponent<PlayerActionState>();
        if (!playerEnvironment) playerEnvironment = GetComponent<PlayerEnvironmentState>();  
        if (!doorController) doorController = GetComponent<PlayerDoorController>();
        if (!receiveController) receiveController = GetComponent<PlayerReceiveController>();
        if (!carryController) carryController = GetComponent<PlayerCarryController>();
        if (!axeController) axeController = GetComponent<PlayerAxeController>();
        if (!macheteController) macheteController = GetComponent<PlayerMacheteController>();
        if (!hoeController) hoeController = GetComponent<PlayerHoeController>();
        if (!toolState) toolState = GetComponent<PlayerToolState>();
        if (!lightController) lightController = GetComponent<PlayerLightSourceController>();
    }

    public void OnInteract()
    {
        if (showDebug) Debug.Log("INTERACT PRESSED");

        // 1. Handle sign interaction.
        if (currentSign != null)
        {
            if (currentSign.IsOpen)
            {
                currentSign.Close();
                playerActionState.ClearActionState();
            }
            else
            {
                if (playerActionState.IsBusy)
                    return;

                currentSign.Open();
            }

            return; // don't fall through to next action
        }

        // 2. Handle NPC dialogue
        if (currentNPC != null)
        {
            // end dialogue if already talking
            if (currentNPC.IsTalking)
            {
                currentNPC.EndDialogue();
                playerActionState.ClearActionState();
            } 
            // otherwise lock player movement and begin conversation
            else
            {
                if (playerActionState.IsBusy) return;

                currentNPC.StartDialogue();
                playerActionState.SetActionState(PlayerState.Dialogue);
            }

            return;
        }

        // 3. Open a door if there's a door to open.
        if (doorController != null && doorController.OpenDoor())
        {
            return;
        }

        // 4. Light a candle or lantern.
        if (lightController != null && lightController.ToggleLight())
        {
            return;
        }

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
            if (carryController != null && carryController.Carry())
            {
                return;
            }
        }

        // 7. Otherwise let Receive handle the interaction.
        if (receiveController != null && receiveController.Receive())
        {
            return;
        }

        // 8. Carry interaction.
        if (carryController != null && carryController.Carry())
        {
            return;
        }


        // 9. Try receiving a new object if Carry didn't handle it.
        if (receiveController != null && receiveController.Receive())
        {
            return;
        }

        // 10. Tool use last.
        if (toolState == null)
        {
            if (showDebug)
                Debug.LogWarning("No PlayerToolState found on Player.");

            return;
        }

        // Only allow tools if player is outside.
        if (playerEnvironment != null &&
            playerEnvironment.PlayerEnvironment == PlayerEnvironment.Outside)
        {
            switch (toolState.EquippedTool)
            {
                case EquippedTool.Axe:
                    axeController?.UseAxe();
                    break;

                case EquippedTool.Machete:
                    macheteController?.UseMachete();
                    break;

                case EquippedTool.Hoe:
                    hoeController?.UseHoe();
                    break;
            }
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