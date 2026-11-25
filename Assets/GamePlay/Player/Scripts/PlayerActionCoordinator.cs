using UnityEngine;

public class PlayerActionCoordinator : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private PlayerToolState toolState;

    [Header("Action Controllers")]
    [SerializeField] private PlayerReceiveController receiveController;
    [SerializeField] private PlayerCarryController carryController;

    [Header("Tool Controllers")]
    [SerializeField] private PlayerAxeController axeController;
    [SerializeField] private PlayerMacheteController macheteController;
    [SerializeField] private PlayerHoeController hoeController;

    [Header("Item Controllers")]
    [SerializeField] private PlayerDoorController doorController;
    [SerializeField] private PlayerLightSourceController lightController;

    private void Awake()
    {
        if (!playerActionState) playerActionState = GetComponent<PlayerActionState>();
        if (!toolState) toolState = GetComponent<PlayerToolState>();

        if (!receiveController) receiveController = GetComponent<PlayerReceiveController>();
        if (!carryController) carryController = GetComponent<PlayerCarryController>();

        if (!axeController) axeController = GetComponent<PlayerAxeController>();
        if (!macheteController) macheteController = GetComponent<PlayerMacheteController>();
        if (!hoeController) hoeController = GetComponent<PlayerHoeController>();

        if (!doorController) doorController = GetComponent<PlayerDoorController>();
        if (!lightController) lightController = GetComponent<PlayerLightSourceController>();

    }

    // ---- Actions ---- //
    public bool HandleReceive()
    {
        if (receiveController == null) return false;
        return receiveController.Receive();
    }

    public bool HandleCarry()
    {
        if (carryController == null) return false;
        return carryController.Carry();
    }


    // ---- Signs ---- //
    public void HandleSign(HearthsoilSign sign)
    {
        if (sign == null) return;

        if (sign.IsOpen)
        {
            sign.Close();
            playerActionState.ClearActionState();
        }
        else
        {
            if (playerActionState.IsBusy) return;
            sign.Open();
        }
    }

    // --- Dialogue --- //
    public void HandleDialogue(NPCDialogueController npc)
    {
        if (npc == null) return;

        // is the conversation open?
        if (npc.IsTalking)
        {
            // are the characters still being revealed?
            if (npc.IsTyping)
            {
                // reveal the remaining characters all at once
                npc.CompleteDialogueLines();
            }
            else
            {
                // advance conversation
                npc.AdvanceDialogue();

                // allow player to move when conversation ends
                if (!npc.IsTalking)
                {
                    playerActionState.ClearActionState();
                }
            }
        }
        // otherwise lock player movement and begin conversation
        else
        {
            if (playerActionState.IsBusy) return;

            npc.StartDialogue();
            playerActionState.SetActionState(PlayerState.Dialogue);
        }
    }

     // ----- Tools ---- //
     public void HandleTool()
    {
        if (toolState == null) return;

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

    // ---- Doors ---- //
    public bool HandleDoor()
    {
        if(doorController == null) return false;
        return doorController.OpenDoor();
    }

    // --- Lights --- //
    public bool HandleLight()
    {
        if(lightController == null) return false;
        return lightController.ToggleLight();
    }


}
