using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMacheteController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private Animator animator;
    [SerializeField] private FacingDirection faceDir;

    public FacingDirection FaceDir => faceDir;

    [Header("Machete Refs")]
    [SerializeField] private MacheteController machete;
    [SerializeField] private bool isMacheting;
    public bool IsMacheting => isMacheting;

    [Header("Object Refs")]
    [SerializeField] private DamageableObjectController currentObject;
    [SerializeField] private DamageableObjectController lockedObject;

    [SerializeField] private bool hitMultipleTargets = false;
    private readonly List<DamageableObjectController> hitTargets = new();

    private void Awake()
    {
        if (!playerController) playerController = GetComponent<PlayerController>();
        if (!animator) animator = GetComponent<Animator>();
        if (!playerActionState) playerActionState = GetComponent<PlayerActionState>();
    }


    private void Update()
    {
        faceDir = playerController.Facing;

    }

    public void UseMachete()
    {
        BeginSwing();
    }

    public void SetHitTarget(DamageableObjectController obj)
    {
        if (!obj) return;

        if (!hitTargets.Contains(obj))
            hitTargets.Add(obj);

        currentObject = GetClosestHitTarget();
    }

    public void ClearHitTarget(DamageableObjectController obj)
    {
        if (!obj) return;

        hitTargets.Remove(obj);

        if (currentObject == obj)
            currentObject = GetClosestHitTarget();
    }

    private DamageableObjectController GetClosestHitTarget()
    {
        DamageableObjectController closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (DamageableObjectController obj in hitTargets)
        {
            if (!obj) continue;

            float distance = Vector2.Distance(transform.position, obj.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = obj;
            }
        }

        return closest;
    }

    public void BeginSwing()
    {
        if (playerActionState.IsBusy) return;
        if (isMacheting) return;

        currentObject = GetClosestHitTarget();

        // don't allow swing if nothing is there
        //if (currentObject == null) return;

        lockedObject = currentObject;

        animator.SetBool("IsMacheting", true);
        machete.PlayMacheteSideSwing();
        isMacheting = true;
        playerActionState.SetActionState(PlayerState.Macheting);
    }

    // --- animation events --- //
    public void OnImpact()
    {
        if (hitMultipleTargets)
        {
            for (int i = hitTargets.Count - 1; i >= 0; i--)
            {
                if (hitTargets[i] == null)
                {
                    hitTargets.RemoveAt(i);
                    continue;
                }

                hitTargets[i].RegisterHit();
            }

            return;
        }

        if (lockedObject == null) return;
        lockedObject.RegisterHit();
    }

    public void EndSwing()
    {
        lockedObject = null;
        animator.SetBool("IsMacheting", false);
        isMacheting = false;
        playerActionState.ClearActionState();
    }

}
