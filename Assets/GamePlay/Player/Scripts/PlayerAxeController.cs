using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAxeController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private Animator animator;
    [SerializeField] private FacingDirection faceDir;
    [SerializeField] private Vector2 toLog;
    public FacingDirection FaceDir => faceDir;

    [Header("Axe Refs")]
    [SerializeField] private AxeController axe;
    [SerializeField] private bool isChopping;
    public bool IsChopping => isChopping;

    [Header("Tree Refs")]
    [SerializeField] private ChoppableTreeController currentTree;
    [SerializeField] private ChoppableTreeController lockedTree;

    [Header("Log Refs")]
    [SerializeField] private FelledLogController currentLog;
    [SerializeField] private FelledLogController lockedLog;



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

    public void UseAxe()
    {
        BeginChop();
    }

    public void BeginChop()
    {
        if (playerActionState.IsBusy) return;
        if (isChopping) return;

        lockedTree = currentTree;
        lockedLog = currentLog;

        SetAnimation();
    }

    // --- animation event --- //
    public void OnAxeImpact()
    {
        if (lockedLog != null)
        {
            lockedLog.ProcessHit();
            return;
        }

        if (lockedTree != null)
        {
            lockedTree.RegisterHit();
            return;
        }

        // No target: axe swings through air.
    }

    // --- animation event --- //
    public void EndChop()
    {
        animator.SetBool("IsChopping", false);
        isChopping = false;
        playerActionState.ClearActionState();

        // Phase 4: Release
        lockedTree = null;
        lockedLog = null;
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
        if (currentTree != null && currentTree == tree) currentTree = null;
    }

    public void SetLogTarget(FelledLogController log)
    {
        if (!log) return;
        currentLog = log;
    }

    public void ClearLogTarget(FelledLogController log)
    {
        if (currentLog == log) currentLog = null;
    }

    private void SetAnimation()
    {
        bool treeIsFelled = lockedLog != null;

        if (!treeIsFelled)
        {
            animator.SetBool("IsChopping", true);
            axe?.PlayAxeSideSwing();
            isChopping = true;
            playerActionState.SetActionState(PlayerState.Chopping);
        } else
        {

            Vector2 logPosition = lockedLog.ChopPosition;
            Vector2 playerPosition = transform.position;
            toLog = logPosition - playerPosition;

            animator.SetTrigger(toLog.y > 0f ? "ChopNorth" : "ChopSouth");

            if(toLog.y > 0f)
            {
                axe.PlayAxeOverSwingNorth();
                playerController.FaceNorth();
            } else
            {
                axe.PlayAxeOverSwingSouth();
                playerController.FaceSouth();
            }

            isChopping = true;
            playerActionState.SetActionState(PlayerState.Chopping);
            

        }
    }

}
