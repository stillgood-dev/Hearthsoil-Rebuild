using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAxeController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private Animator animator;
    [SerializeField] private FacingDirection faceDir;
    public FacingDirection FaceDir => faceDir;

    [Header("Axe Refs")]
    [SerializeField] private AxeController axe;
    [SerializeField] private bool isChopping;
    public bool IsChopping => isChopping;

    [Header("Equip Status")]
    [Tooltip("Temporary bool to indicate axe has been equipped, will remove when I build equip system")]
    [SerializeField] private bool isEquipped = false;

    [Header("Tree Refs")]
    [SerializeField] private ChoppableTreeController currentTree;
    [SerializeField] private ChoppableTreeController lockedTree;



    private void Awake()
    {
        if(!playerController) playerController = GetComponent<PlayerController>();
        if(!animator) animator = GetComponent<Animator>();
        if (!playerActionState) playerActionState = GetComponent<PlayerActionState>();
    }

    private void Update()
    {
        faceDir = playerController.Facing;
    }

    public void OnInteract()
    {
        if (!isEquipped) return;
        BeginChop();
    }

    public void BeginChop()
    {
        // prevent chopping if midchop
        if (isChopping) return;
        // don't allow chopping if there isn't a tree nearby
        if (currentTree == null) return;

        lockedTree = currentTree;

        animator.SetBool("IsChopping", true);
        axe.PlayAxeSwing();
        isChopping = true;
        playerActionState.SetActionState(PlayerState.Chopping);
        
        
    }

    // --- animation event --- //
    public void OnAxeImpact()
    {
        lockedTree?.RegisterHit();

    }

    // --- animation event --- //
    public void EndChop()
    {
        animator.SetBool("IsChopping", false);
        isChopping = false;
        playerActionState.ClearActionState();
        lockedTree = null;
    }

    // cache the tree 
    public void SetChopTarget(ChoppableTreeController tree)
    {
        if (!tree) return;
        currentTree = tree;
    }

    // clear the tree
    public void ClearChopTarget(ChoppableTreeController tree)
    {
        if (currentTree == tree) currentTree = null;
    }

}
