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

    [Header("Equip Status")]
    [Tooltip("Temporary bool to indicate axe has been equipped, will remove when I build equip system")]
    [SerializeField] private bool isEquipped = false;

    [Header("Object Refs")]
    [SerializeField] private DamageableObjectController currentObject;
    [SerializeField] private DamageableObjectController lockedObject;

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

    public void OnInteract()
    {
        if (!isEquipped) return;
        BeginSwing();
    }

    public void SetHitTarget(DamageableObjectController obj)
    {
        if (!obj) return;

        if (!hitTargets.Contains(obj))
            hitTargets.Add(obj);

        currentObject = obj;
    }

    public void ClearHitTarget(DamageableObjectController obj)
    {
        if (!obj) return;

        hitTargets.Remove(obj);

        if (currentObject == obj)
        {
            currentObject = GetBestHitTarget();
        }
    }

    private DamageableObjectController GetBestHitTarget()
    {
        // Simple version: use the most recently added remaining target
        for (int i = hitTargets.Count - 1; i >= 0; i--)
        {
            if (hitTargets[i] != null)
                return hitTargets[i];
        }

        return null;
    }

    public void BeginSwing()
    {
        if (playerActionState.IsBusy) return;
        if (isMacheting) return;

        currentObject = GetBestHitTarget();

        if (currentObject == null) return;

        lockedObject = currentObject;

        animator.SetBool("IsMacheting", true);
        machete.PlayMacheteSideSwing();
        isMacheting = true;
        playerActionState.SetActionState(PlayerState.Macheting);
    }

    public void OnImpact()
    {
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
