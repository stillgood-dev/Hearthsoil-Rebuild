using UnityEngine;

public class HitZone : MonoBehaviour
{
    [SerializeField] private PlayerMacheteController playerMacheteController;
    [SerializeField] private DamageableObjectController damageableObject;
    [SerializeField] private bool showDebug = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (showDebug)
        {
            Debug.Log("HIT ZONE TRIGGER ENTER: " + other.name);

            Debug.Log("Entering object's tag: " + other.tag);
        }

        if (!other.CompareTag("Player"))
        {
            if(showDebug) Debug.Log("Object was NOT tagged Player");
            return;
        }

        if (showDebug)
        {
            Debug.Log("PLAYER TAG CONFIRMED");
        }

        playerMacheteController = other.GetComponent<PlayerMacheteController>();

        if (showDebug)
        {
            Debug.Log("Machete controller found: " + playerMacheteController);
        }

        damageableObject?.SetPlayerInHitZone(true, playerMacheteController);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (showDebug)
        {
            Debug.Log("HIT ZONE TRIGGER EXIT: " + other.name);
        }

        if (!other.CompareTag("Player")) return;

        playerMacheteController = other.GetComponent<PlayerMacheteController>();

        damageableObject?.SetPlayerInHitZone(false, playerMacheteController);
    }
}
