using UnityEngine;

public class ChopZone : MonoBehaviour
{
    [SerializeField] private ChoppableTreeController treeController;
    [SerializeField] private PlayerAxeController playerAxeController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerAxeController = other.GetComponent<PlayerAxeController>();
        treeController?.SetPlayerInChopZone(true, playerAxeController);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerAxeController = other.GetComponent<PlayerAxeController>();
        treeController?.SetPlayerInChopZone(false, playerAxeController);
    }
}
