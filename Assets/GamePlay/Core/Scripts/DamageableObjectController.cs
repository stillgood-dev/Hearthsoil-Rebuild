using UnityEngine;

public class DamageableObjectController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerMacheteController playerMacheteController;
    [SerializeField] private bool playerInHitZone;

    [Header("Hit Parameters")]
    [SerializeField] private int hits;
    [SerializeField] private int hitsToComplete = 3;

    [Header("Sprites")]
    [SerializeField] private GameObject undamaged;
    [SerializeField] private GameObject damaged;

    [Header("Colliders")]
    [SerializeField] private GameObject physicalCollider;
    [SerializeField] private GameObject hitZone;

    public void Awake()
    {
        if(undamaged != null) undamaged.SetActive(true);
        if(damaged != null) damaged.SetActive(false);
    }

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

        PersistentStateObject persistentState = 
            GetComponent<PersistentStateObject>();

        if(persistentState != null)
        {
            persistentState.MarkChanged();
            return;
        }

        // Fallback for non-persistent damageable objects
        DisableColliders();
        SwapToDamagedSprite();
    }

    public void DisableColliders()
    {
        if (physicalCollider != null)
            physicalCollider.SetActive(false);

        if (hitZone != null)
            hitZone.SetActive(false);
    }

    public void SwapToDamagedSprite()
    {
        if (undamaged != null)
            undamaged.SetActive(false);

        if (damaged != null)
            damaged.SetActive(true);
    }
}
