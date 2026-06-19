using UnityEngine;

public class PlayerDoorController : MonoBehaviour
{

    [SerializeField] private CabinDoorController currentDoor;
    [SerializeField] private CabinDoorController lockedDoor;
    [SerializeField] private PlayerEnvironmentState environmentState;

    // To null out when player leaves the scene, or do I even need to if I don't have it persist? I don't think I need to, but I'll investigate later
    public CabinDoorController LockedDoor => lockedDoor;

    private void Awake()
    {
        if (!environmentState) environmentState = GetComponent<PlayerEnvironmentState>();
    }


    // player targets the door for interaction when entering trigger zone
    public void SetDoorTarget(CabinDoorController cabinDoor)
    {
        if (!cabinDoor) return;
        currentDoor = cabinDoor;

    }

    // player stops targeting the door for interaction if target was set when leaving trigger zone
    public void ClearDoorTarget(CabinDoorController cabinDoor)
    {
        if (!cabinDoor) return;
        if (currentDoor != null && currentDoor == cabinDoor)
        {
            currentDoor = null;
        }

    }

    // player tells cabin door controller to open the door
    public bool OpenDoor()
    {
        if (currentDoor == null) return false;

        lockedDoor = currentDoor;
        lockedDoor.DoorOpen();

            // return a bool so InteractionController can read it
            return true;
    }



    
}
