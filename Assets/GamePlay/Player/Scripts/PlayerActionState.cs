using UnityEngine;

public enum PlayerState
{
    Free,
    Receiving,
    Chopping,
    Carrying,
    Macheting,
    Hoeing,
    Tilling, // walking while hoeing
    Interacting,
    Dialogue 
}

public class PlayerActionState : MonoBehaviour
{
    [SerializeField] PlayerState state = PlayerState.Free;
    public PlayerState State => state;

    // flag to determine if we are busy with another action
    public bool IsBusy =>
        state == PlayerState.Receiving ||
        state == PlayerState.Chopping ||
        state == PlayerState.Dialogue ||
        state == PlayerState.Macheting ||
        state == PlayerState.Hoeing ||
        state == PlayerState.Tilling ||
        state == PlayerState.Interacting || 
        state == PlayerState.Carrying;

    public bool LockMovement =>
        state == PlayerState.Receiving ||
        state == PlayerState.Chopping ||
        state == PlayerState.Macheting ||
        state == PlayerState.Hoeing ||
        state == PlayerState.Interacting ||
        state == PlayerState.Dialogue;

    // other scripts can set states
    public void SetActionState(PlayerState newState)
    {
        state = newState;
    }

    // other scripts can can clear states
    public void ClearActionState()
    {
        state = PlayerState.Free;
    }
}
