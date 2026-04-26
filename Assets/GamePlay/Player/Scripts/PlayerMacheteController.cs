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
       currentObject = obj;
    }

    // clear the tree
    public void ClearHitTarget(DamageableObjectController obj)
    {
        if (currentObject == obj) currentObject = null;
    }

    public void BeginSwing()
    {
        if (playerActionState.IsBusy) return;
        // prevent player from using tool if there is nothing to hit
        if (currentObject == null) return;

        // prevent swinging mid swing
        if (isMacheting) return;

        lockedObject = currentObject;

        animator.SetBool("IsMacheting", true);
        machete.PlayMacheteSideSwing();
        isMacheting = true;
        playerActionState.SetActionState(PlayerState.Macheting);
        
    }

    // --- animation event --- //
    public void OnImpact()
    {
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
