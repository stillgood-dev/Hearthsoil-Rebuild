using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabinDoorController : MonoBehaviour
{
    [Header("Cabin Refs")]
    [SerializeField] private bool inside = false;
    [SerializeField] private float insideDoorOpenSeconds = 0.25f;
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;

    public bool Inside => inside;


    [Header("Player Refs")]
    [SerializeField] private bool playerInInteractZone;
    [Tooltip("Do not wire in Inspector")]
    [SerializeField] private PlayerDoorController playerDoorController;


    [Header("Scene Transition")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string spawnPointName;
    [SerializeField] private float sceneTransitionSeconds = 0.25f;

    private void Start()
    {
        if (inside)
        {
           StartCoroutine(StartInsideDoorRoutine());
        }
        else
        {
            DoorClosed();
        }
    }


    private IEnumerator StartInsideDoorRoutine()
    {
        // don't use DoorOpen() here because we only want that true if the player does it to take them to the next scene
        if (closedDoor != null) closedDoor.SetActive(false);
        if (openDoor) openDoor.SetActive(true); // fail safe in case it ever becomes inactive for any reason

        yield return new WaitForSeconds(insideDoorOpenSeconds);

        DoorClosed();
    }

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
        StartCoroutine(WaitForSceneTransition());
    }

    private IEnumerator WaitForSceneTransition()
    {
        yield return new WaitForSeconds(sceneTransitionSeconds);

        SceneTransitionData.SpawnPointName = spawnPointName;
        SceneManager.LoadScene(sceneToLoad);
    }



    public void DoorClosed()
    {
        if (openDoor != null) openDoor.SetActive(false);
        if (closedDoor != null) closedDoor.SetActive(true);
    }
    
}
