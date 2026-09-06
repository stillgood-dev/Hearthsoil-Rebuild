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

    [Header("Stages")]
    [SerializeField] private GameObject[] objectStages;

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
        if (objectStages == null || objectStages.Length == 0) return;

        int index = hits - 1;

        if (index >= objectStages.Length) return;

        if (index == 0)
        {
            foreach (var obj in undamaged)
            {
                if(obj != null)
                    obj.SetActive(false);
            }
            
            if (spawnResource != null)
                spawnResource.SpawnResourceOnHit(playerMacheteController.FaceDir, hits);
        }
        else
        {
            objectStages[index - 1].SetActive(false);
            
            if (spawnResource != null)
                spawnResource.SpawnResourceOnHit(playerMacheteController.FaceDir, hits);
        }

        objectStages[index].SetActive(true);
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

        if (damaged != null)
            damaged.SetActive(true);
    }
}
