using UnityEngine;

public enum PlayerState
{
    Free,
    Receiving,
    Equipping,
    Chopping,
    Dialogue // add more as we build action states
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
        state == PlayerState.Equipping;

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
