using UnityEngine;

public enum PlayerEnvironment
{
    Outside,
    Inside
    // Cave
    // Forest, etc?
}

public class PlayerEnvironmentState : MonoBehaviour
{
    [SerializeField] PlayerEnvironment environment = PlayerEnvironment.Outside;

    public PlayerEnvironment PlayerEnvironment => environment;

    public void SetPlayerEnvironment(PlayerEnvironment playerEnvironment)
    {
        environment = playerEnvironment;
    }
}

