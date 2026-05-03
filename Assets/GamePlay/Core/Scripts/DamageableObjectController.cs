using UnityEngine;

public class DamageableObjectController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerMacheteController playerMacheteController;
    [SerializeField] private bool playerInHitZone;

    [Header("Object Refs")]
    [SerializeField] private GameObject damageableObject;
    [SerializeField] private SpriteRenderer undamagedSR;
    [SerializeField] private SpriteRenderer damagedSR;
    [SerializeField] private PolygonCollider2D damageCollider;

    [Header("Hit Parameters")]
    [SerializeField] private int hitsToDamagedSR = 1;
    [SerializeField] private int hits;
    [SerializeField] private bool isDamaged;
    [SerializeField] private bool isMiddleSprite;

    private void Awake()
    {
        if(damagedSR) damagedSR.enabled = false;
    }



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

        if (isDamaged)
        {
            damageableObject.SetActive(false);
        }

        if (hits == hitsToDamagedSR)
        {
            if (isMiddleSprite)
            {
                damageableObject.SetActive(false);

            } else
            {
                SwapToDamagedSprite();
                isDamaged = true;
            }
        }
        
    }

    private void SwapToDamagedSprite()
    {
        if (!undamagedSR) return;
        if (!damagedSR) return;
        if (!damageCollider) return;

        undamagedSR.enabled = false;
        damagedSR.enabled = true;
        damageCollider.enabled = false;

    }

    
}
