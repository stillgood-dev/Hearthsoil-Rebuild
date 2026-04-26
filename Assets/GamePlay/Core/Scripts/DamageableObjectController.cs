using UnityEngine;

public class DamageableObjectController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerMacheteController playerMacheteController;
    [SerializeField] private bool playerInHitZone;

    [Header("Object Refs")]
    [SerializeField] private SpriteRenderer undamagedSR;
    [SerializeField] private SpriteRenderer damagedSR;
    [SerializeField] private GameObject objectColliders;

    [Header("Hit Parameters")]
    [SerializeField] private int hitsToDamagedSR = 1;
    [SerializeField] private int hits;



    public void SetPlayerInHitZone(bool inRange, PlayerMacheteController playerMachete)
    {
        playerInHitZone = inRange;
        playerMacheteController = playerMachete;

        if (playerInHitZone)
        {
            playerMacheteController.SetHitTarget(this);
        }
        else
        {
            playerMacheteController.ClearHitTarget(this);
        }
    }

    public void RegisterHit()
    {
        hits++;
        SwapToDamagedSprite();
    }

    private void SwapToDamagedSprite()
    {
        if (!undamagedSR) return;
        if (!damagedSR) return;
        if (!objectColliders) return;

        if(hits == hitsToDamagedSR)
        {
            undamagedSR.enabled = false;
            objectColliders.SetActive(false);
            damagedSR.enabled = true;
        }
    }

    
}
