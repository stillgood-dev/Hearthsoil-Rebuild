using UnityEngine;

public class HitZone : MonoBehaviour
{
    
    [SerializeField] private PlayerMacheteController playerMacheteController;
    [SerializeField] private DamageableObjectController damageableObject;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerMacheteController = other.GetComponent<PlayerMacheteController>();
        damageableObject?.SetPlayerInHitZone(true, playerMacheteController);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerMacheteController = other.GetComponent<PlayerMacheteController>();
        damageableObject?.SetPlayerInHitZone(false, playerMacheteController);
    }
}
