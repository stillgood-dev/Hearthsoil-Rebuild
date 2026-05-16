using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Current Interaction")]
    [SerializeField] private HearthsoilSign currentSign;

    [Header("Player Refs")]
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private PlayerToolState toolState;

    [Header("Action Controllers")]
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
        if (!receiveController) receiveController = GetComponent<PlayerReceiveController>();
        if (!carryController) carryController = GetComponent<PlayerCarryController>();
        if (!axeController) axeController = GetComponent<PlayerAxeController>();
        if (!macheteController) macheteController = GetComponent<PlayerMacheteController>();
        if (!hoeController) hoeController = GetComponent<PlayerHoeController>();
        if (!toolState) toolState = GetComponent<PlayerToolState>();
    }

    public void OnInteract()
    {
        if (showDebug) Debug.Log("INTERACT PRESSED");

        // 1. If a sign is already open, close it first.
        if (currentSign != null && currentSign.IsOpen)
        {
            currentSign.Close();
            playerActionState.ClearActionState();
            return;
        }

        // 2. Allow Receiving to continue so the second E press can accept.
        if (playerActionState.IsBusy && playerActionState.State != PlayerState.Receiving)
        {
            if (showDebug) Debug.Log("Player is busy: " + playerActionState.State);
            return;
        }

        // 3. If already receiving, let Receive handle accept and STOP.
        if (receiveController != null && receiveController.Receive())
        {
            return;
        }

        // 4. If near a sign, open it.
        if (currentSign != null)
        {
            currentSign.Open();
            playerActionState.SetActionState(PlayerState.Interacting);
            return;
        }

        // 5. Carry interactions.
        carryController?.Carry();

        // 6. Try receiving new object and STOP if it happened.
        if (receiveController != null && receiveController.Receive())
        {
            return;
        }

        // 7. Tool use last.
        if (toolState == null)
        {
            if (showDebug) Debug.LogWarning("No PlayerToolState found on Player.");
            return;
        }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (showDebug) Debug.Log("Entered trigger: " + other.name);

        HearthsoilSign sign = other.GetComponentInParent<HearthsoilSign>();

        if (sign != null)
        {
            currentSign = sign;

            if (showDebug) Debug.Log("Current sign set to: " + sign.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (showDebug) Debug.Log("Exited trigger: " + other.name);

        HearthsoilSign sign = other.GetComponentInParent<HearthsoilSign>();

        if (sign != null && currentSign == sign)
        {
            currentSign = null;

            if (showDebug) Debug.Log("Current sign cleared");
        }
    }
}