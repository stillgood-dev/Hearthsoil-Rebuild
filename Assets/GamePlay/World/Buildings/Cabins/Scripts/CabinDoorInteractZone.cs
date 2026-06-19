using UnityEngine;

public class CabinDoorInteractZone : MonoBehaviour
{
    [SerializeField] private PlayerDoorController playerDoorController;
    [SerializeField] private CabinDoorController cabinDoorController;

    // When player enters this interact zone, a child of The Cabin,
    // grab cabinDoorController of the cabin and use it as the input to
    // playerDoorController.SetDoorTarget and playerDoorController.ClearDoorTarget

    // Pass cabinDoorController to playerDoorController via the Interact Zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerDoorController = other.GetComponent<PlayerDoorController>();

        if (playerDoorController != null)
        {
            cabinDoorController.SetPlayerInInteractZone(true, playerDoorController);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerDoorController = other.GetComponent<PlayerDoorController>();

        if (playerDoorController != null)
        {
            cabinDoorController.SetPlayerInInteractZone(false, playerDoorController);
        }
    }
}
