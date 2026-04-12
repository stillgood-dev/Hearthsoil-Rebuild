using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReceiveController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform receiveAnchor;

    [Header("Dev/Debug")]
    [SerializeField] private bool playerInZone;

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerInZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerInZone = false;
    }

}
