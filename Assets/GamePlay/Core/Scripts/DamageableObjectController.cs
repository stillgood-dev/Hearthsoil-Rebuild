using UnityEngine;
using System.Collections;

public class DamageableObjectController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerMacheteController playerMacheteController;
    [SerializeField] private bool playerInHitZone; // for debugging

    [Header("Persistence")]
    [SerializeField] private PersistentStateObject persistentState;

    [Header("Hit Parameters")]
    [SerializeField] private int hits;
    [SerializeField] private int hitsToComplete = 3;

    [Header("Sprites")]
    [SerializeField] private GameObject[] undamaged;
    [SerializeField] private GameObject damaged;
    [SerializeField] private bool hideDamagedSpritesOnAwake = false;

    [Header("Colliders")]
    [SerializeField] private GameObject physicalCollider;
    [SerializeField] private GameObject hitZone;

    [Header("Intermediate Damage Stages")]
    [SerializeField] private GameObject[] damagedStages;

    [Header("Impact FX")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject impactBurst;
    [SerializeField] private string impactBurstName = "ShowImpactBurst";
    [SerializeField] private ShakeObject shakeObject;
    // sound fx here eventually

    [Header("Spawned Resources")]
    [SerializeField] private SpawnResource spawnResource;

    [Header("Debug")]
    [SerializeField] private bool showDebug = false;

    private void Awake()
    {
        if (persistentState == null) persistentState = GetComponent<PersistentStateObject>();

        // set impact animation inactive so it can't play automatically
        if (impactBurst != null) impactBurst.SetActive(false);

        if (shakeObject == null) shakeObject = GetComponent<ShakeObject>();

        // don't show damaged sprites if undamaged for those you don't want to show (ie., cliff roots)
        if (hideDamagedSpritesOnAwake &&
             (persistentState == null || !persistentState.HasChanged()))
        {
            if (damaged != null)
                damaged.SetActive(false);

            foreach (var obj in damagedStages)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }

    // respawn resources in scene if player left them there and moved to a different scene
    private void Start()
    {
        if (persistentState != null && persistentState.HasChanged())
        {
            spawnResource?.RestoreResources(hitsToComplete);
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

            if (shakeObject != null) shakeObject.Shake();

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

        if (showDebug) Debug.Log($"Hits: {hits} | Current index: {index}");

        if (index >= damagedStages.Length) return;

        // on first hit
        if (index == 0)
        {
            // disable undamaged sprites
            foreach (var obj in undamaged)
            {
                if (obj != null)
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
        if (impactBurst != null) impactBurst.SetActive(true);
        if (animator != null) animator.SetTrigger("ShowImpactBurst");

    }

    private void CompleteDamage()
    {
        playerMacheteController?.ClearHitTarget(this);

        if (persistentState != null)
        {
            switch (persistentState.Persistence)
            {
                case PersistenceType.None:
                    DisableColliders();
                    SwapToDamagedSprite();
                    break;

                case PersistenceType.Persist:
                    persistentState.MarkChanged();
                    break;

                case PersistenceType.Regrow:
                    persistentState.MarkForRegrowth();
                    break;
            }
        }
        else
        {
            // Truly no PersistentStateObject attached
            DisableColliders();
            SwapToDamagedSprite();
        }

        if (impactBurst != null)
            impactBurst.SetActive(false);
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

        foreach (var obj in damagedStages)
        {
            if (obj != null) obj.SetActive(false);
        }


        if (damaged != null)
            damaged.SetActive(true);
    }
}
