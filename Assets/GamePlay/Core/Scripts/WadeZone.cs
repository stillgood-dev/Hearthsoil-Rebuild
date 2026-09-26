using UnityEngine;

public class WadeZone : MonoBehaviour
{
    [Header("Player Refs")]
    [Tooltip("Do not wire, viewing only")]
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private PlayerController playerController;

    [Header("Shake Object")]
    [SerializeField] private ShakeObject shake;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerActionState = other.GetComponent<PlayerActionState>();
        playerController = other.GetComponent<PlayerController>();

        if(playerActionState != null )
        {
            playerActionState.SetActionState(PlayerState.Wading);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (playerController == null) return;

        if (playerController.IsMoving)
        {
            if (shake != null) shake.Shake();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerActionState = other.GetComponent<PlayerActionState>();

        if (playerActionState != null && 
            playerActionState.State == PlayerState.Wading)
            playerActionState.ClearActionState();

        playerActionState = null;
        playerController = null;
    }
}
