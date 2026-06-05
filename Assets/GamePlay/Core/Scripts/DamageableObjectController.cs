using UnityEngine;

public class DamageableObjectController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerMacheteController playerMacheteController;
    [SerializeField] private bool playerInHitZone;

    [Header("Hit Parameters")]
    [SerializeField] private int hits;
    [SerializeField] private int hitsToComplete = 3;

    [Header("Completion Behavior")]
    [SerializeField] private bool markAsDestroyedOnComplete = true;
    [SerializeField] private bool deactivateOnComplete = true;

    public void SetPlayerInHitZone(bool inRange, PlayerMacheteController playerMachete)
    {
        playerInHitZone = inRange;
        playerMacheteController = playerMachete;

        if (playerInHitZone)
            playerMacheteController.SetHitTarget(this);
        else
            playerMacheteController.ClearHitTarget(this);
    }

    public void RegisterHit()
    {
        hits++;

        if (hits >= hitsToComplete)
        {
            CompleteDamage();
        }
    }

    private void CompleteDamage()
    {
        playerMacheteController?.ClearHitTarget(this);

        if (markAsDestroyedOnComplete)
        {
            GetComponent<PersistentDestroyableObject>()?.MarkDestroyed();
            return;
        }

        if (deactivateOnComplete)
        {
            gameObject.SetActive(false);
        }
    }
}
