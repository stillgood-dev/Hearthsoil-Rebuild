using UnityEngine;
using System.Collections;

public class DamageableObjectController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerMacheteController playerMacheteController;
    [SerializeField] private bool playerInHitZone;

    [Header("Hit Parameters")]
    [SerializeField] private int hits;
    [SerializeField] private int hitsToComplete = 3;

    [Header("Sprites")]
    [SerializeField] private GameObject[] undamaged;
    [SerializeField] private GameObject damaged;
    [Tooltip("Don't keep the damaged sprites active")]
    [SerializeField] private bool hideDamagedSpritesOnAwake = false;

    [Header("Stages")]
    [SerializeField] private GameObject[] damagedStages;

    [Header("Impact Burst")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject impactBurst;
    [SerializeField] private string impactBurstName = "ShowImpactBurst";

    [Header("Impact Shake")]
    [SerializeField] private ShakeObject shakeObject;

    [Header("Colliders")]
    [SerializeField] private GameObject physicalCollider;
    [SerializeField] private GameObject hitZone;

    [Header("Resources")]
    [SerializeField] private SpawnResource spawnResource;

    private void Awake()
    {
        // set inactive so the impact burst can't play automatically
        if (impactBurst != null) impactBurst.SetActive(false);
        if (shakeObject == null) GetComponent<ShakeObject>();
        if (hideDamagedSpritesOnAwake)
        {
            damaged.SetActive(false);
            foreach(var obj in damagedStages)
            {
                if (obj != null) obj.SetActive(false);
            }
        }
    }

    // tell PlayerMacheteController we're in the hit zone
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
            if (animator != null) 
                animator.SetTrigger(impactBurstName);

            if(shakeObject != null) shakeObject.Shake();

            if (spawnResource != null)
                spawnResource.SpawnResourceOnHit(playerMacheteController.FaceDir, hits);

            CompleteDamage();
            return;
        }

        UpdateStageOnHit();
        if (shakeObject != null) shakeObject.Shake();
    }

    private void UpdateStageOnHit()
    {
        if (damagedStages == null || damagedStages.Length == 0) return;

        int index = hits - 1;

        Debug.Log($"Hits: {hits} | Current index: {index}");

        if (index >= damagedStages.Length) return;

        // on first hit
        if (index == 0)
        {
            // disable undamaged sprites
            foreach (var obj in undamaged)
            {
                if(obj != null)
                    obj.SetActive(false);
            }
            
            // spawn resource if available
            if (spawnResource != null)
                spawnResource.SpawnResourceOnHit(playerMacheteController.FaceDir, hits);
        }
        // on any subsequent hit
        else
        {
            // set the previous damage stage inactive
            damagedStages[index - 1].SetActive(false);
            
            // spawn resource if available
            if (spawnResource != null)
                spawnResource.SpawnResourceOnHit(playerMacheteController.FaceDir, hits);
        }

        // set current damage stage
        damagedStages[index].SetActive(true);

        // show animation if available
        if(impactBurst != null) impactBurst.SetActive(true);
        if(animator != null) animator.SetTrigger("ShowImpactBurst");

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
        if (impactBurst != null) impactBurst.SetActive(false);
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
        foreach (var obj in undamaged)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        foreach(var obj in damagedStages)
        {
            if(obj != null) obj.SetActive(false);    
        }


        if (damaged != null)
            damaged.SetActive(true);
    }
}
