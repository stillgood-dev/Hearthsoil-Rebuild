using UnityEngine;

public class CabinDoorController : MonoBehaviour
{
    [Header("Cabin Refs")]
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;

    [Header("Player Refs")]
    [SerializeField] private bool playerInInteractZone;
    [Tooltip("Do not wire in Inspector")]
    [SerializeField] private PlayerDoorController playerDoorController;

    // Cabin door detects player, and allows player to set the door as it's target
    public void SetPlayerInInteractZone(bool inRange, PlayerDoorController doorController)
    {
        playerInInteractZone = inRange;
        playerDoorController = doorController;

        if(playerInInteractZone)
        {
            doorController.SetDoorTarget(this);
        } else
        {
            doorController.ClearDoorTarget(this);
        }
    }
 
    // Closed door sprite is de-activated, open door sprite activated
    public void DoorOpen()
    {
        if(closedDoor != null) closedDoor.SetActive(false);
        if (openDoor) openDoor.SetActive(true); // fail safe in case it ever becomes inactive for any reason
    }
    
}
