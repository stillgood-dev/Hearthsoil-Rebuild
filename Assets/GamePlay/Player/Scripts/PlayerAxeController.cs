using System.Collections.Generic;
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
    [SerializeField] private bool hitMultipleTreeTargets = false;
    private readonly List<ChoppableTreeController> treeTargets = new();

    [Header("Log Refs")]
    [SerializeField] private FelledLogController currentLog;
    [SerializeField] private FelledLogController lockedLog;
    [SerializeField] private bool hitMultipleLogTargets = false;
    private readonly List<FelledLogController> logTargets = new();



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

        currentTree = GetClosestTreeTarget();
        currentLog = GetClosestLogTarget();

        lockedTree = currentTree;
        lockedLog = currentLog;

        SetAnimation();
    }

    // --- animation event --- //
    public void OnAxeImpact()
    {

        if (hitMultipleLogTargets)
        {
            for (int i = logTargets.Count - 1; i >= 0; i--)
            {
                if (logTargets[i] == null)
                {
                    logTargets.RemoveAt(i);
                    continue;
                }

                logTargets[i].ProcessHit();
            }

            return;
        }


        if (lockedLog != null)
        {
            lockedLog.ProcessHit();
            return;
        }

        if (hitMultipleTreeTargets)
        {
            for (int i = treeTargets.Count - 1; i >= 0; i--)
            {
                if (treeTargets[i] == null)
                {
                    treeTargets.RemoveAt(i);
                    continue;
                }

                treeTargets[i].RegisterHit();
            }

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

    // ---- TREE ---- //

    // cache the tree
    public void SetChopTarget(ChoppableTreeController tree)
    {
        if (!tree) return;
        
        if(!treeTargets.Contains(tree))
            treeTargets.Add(tree);

        currentTree = GetClosestTreeTarget();
    }

    
    // clear the tree
    public void ClearChopTarget(ChoppableTreeController tree)
    {
        if (!tree) return;
        treeTargets.Remove(tree);

        if (currentTree == tree)
            currentTree = GetClosestTreeTarget();
    }


    private ChoppableTreeController GetClosestTreeTarget()
    {
        ChoppableTreeController closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (ChoppableTreeController tree in treeTargets)
        {
            if (!tree) continue;

            float distance = Vector2.Distance(transform.position, tree.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = tree;
            }
        }

        return closest;
    }


    // --- LOG --- //

    // cache the log
    public void SetLogTarget(FelledLogController log)
    {
        if (!log) return;
        
        if(!logTargets.Contains(log))
            logTargets.Add(log);

        currentLog = GetClosestLogTarget();
    }

    // clear the log
    public void ClearLogTarget(FelledLogController log)
    {
        if (!log) return;
        logTargets.Remove(log);

        if(currentLog == log)
            currentLog = GetClosestLogTarget();
    }


    private FelledLogController GetClosestLogTarget()
    {
        FelledLogController closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (FelledLogController log in logTargets)
        {
            if(!log) continue;

            float distance = Vector2.Distance(transform.position, log.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = log;
            }
        }
        return closest;
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
