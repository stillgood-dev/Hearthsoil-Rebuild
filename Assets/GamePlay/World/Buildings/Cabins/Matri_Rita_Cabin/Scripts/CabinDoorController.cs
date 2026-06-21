using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabinDoorController : MonoBehaviour
{
    [Header("Cabin Refs")]
    [SerializeField] private float doorOpenSeconds = 0.3f;
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;
    [SerializeField] private string doorID;          // this door's ID
    [SerializeField] private string arrivalDoorID;   // door ID in the next scene

    [Header("Player Refs")]
    [SerializeField] private bool playerInInteractZone;
    [Tooltip("Do not wire in Inspector")]
    [SerializeField] private PlayerDoorController playerDoorController;

    [Header("Scene Transition")]
    [SerializeField] private float sceneTransitionSeconds = 0.2f;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string spawnPointName;

    private bool isTransitioning;

    private void Start()
    {
        if (SceneTransitionData.ArrivalDoorID == doorID)
        {
            StartCoroutine(StartDoorRoutine());
        }
        else
        {
            DoorClosed();
        }
    }

    public void TryEnterCabin()
    {
        if (isTransitioning) return;
        if (!playerInInteractZone) return;

        StartCoroutine(DoorRoutine());
    }

    private IEnumerator DoorRoutine()
    {
        isTransitioning = true;

        DoorOpen();

        yield return new WaitForSeconds(sceneTransitionSeconds);

        SceneTransitionData.SpawnPointName = spawnPointName;
        SceneTransitionData.ArrivalDoorID = arrivalDoorID;

        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator StartDoorRoutine()
    {
        DoorOpen();

        yield return new WaitForSeconds(doorOpenSeconds);

        DoorClosed();
    }

    public void SetPlayerInInteractZone(bool inRange, PlayerDoorController doorController)
    {
        playerInInteractZone = inRange;
        playerDoorController = doorController;

        if (playerInInteractZone)
        {
            doorController.SetDoorTarget(this);
        }
        else
        {
            doorController.ClearDoorTarget(this);
        }
    }

    public void DoorOpen()
    {
        if (closedDoor != null) closedDoor.SetActive(false);
        if (openDoor != null) openDoor.SetActive(true);
    }

    public void DoorClosed()
    {
        if (openDoor != null) openDoor.SetActive(false);
        if (closedDoor != null) closedDoor.SetActive(true);
    }
}