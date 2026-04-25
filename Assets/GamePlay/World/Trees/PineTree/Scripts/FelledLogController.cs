using UnityEngine;

public class FelledLogController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerAxeController playerAxeController;
    [SerializeField] private bool playerInChopZone;

    [Header("Felled Tree Refs")]
    [SerializeField] private ChoppableTreeController treeController;
    [SerializeField] private GameObject felledTree;
    [SerializeField] private Transform felledTreePosition;

    [Header("Felled Branches Refs")]
    [SerializeField] private SpriteRenderer felledBranchesRenderer;
    [SerializeField] private GameObject felledBranchesGameObject;
    [SerializeField] private GameObject felledBranchesShadowGameObject;
    [SerializeField] private GameObject felledBranchesCollider;

    [Header("Felled Log")]
    [SerializeField] private SpriteRenderer logRenderer;
    [SerializeField] private GameObject logCollider;
    [SerializeField] private GameObject felledLogGameObject;

    [Header("Stages")]
    [SerializeField] private GameObject[] branchStages;
    [SerializeField] private GameObject[] logStages;

    [SerializeField] private int branchHits = 0;
    [SerializeField] private int branchHitsToProcess = 4;

    [SerializeField] private int logHits = 0;
    public int LogHits => logHits;

    [SerializeField] private int logHitsToProcess = 4;

    [SerializeField] private bool logOnly = false;

    [Header("Impact Burst")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject impactBurst;

    [Header("Log Resource")]
    [SerializeField] private GameObject logResource;
    [SerializeField] private Transform logDropPoint;
    [SerializeField] private int logs = 0;
    public int Logs => logs;


    public GameObject FelledTree => felledTree;
    public Transform FelledTreePosition => felledTreePosition;

    public Vector2 ChopPosition => felledTreePosition ? felledTreePosition.position : transform.position;

    private void Awake()
    {
        // set inactive so the impact burst can't play when the felled tree activates
        if (impactBurst != null) impactBurst.SetActive(false);
    }

    private void UpdateBranchStageOnHit()
    {
        if (branchStages == null) return;
        if (felledBranchesCollider == null) return;
        if (logCollider == null) return;
        if (animator == null) return;

        int index = Mathf.Clamp(branchHits - 1, 0, branchStages.Length - 1);
        if (index != 0)
        {
            branchStages[index - 1].SetActive(false);
        }

        branchStages[index].SetActive(true);
        impactBurst.SetActive(true);
        if(treeController != null)
        {
            if (treeController.lastHitDirection == FacingDirection.East || 
                treeController.lastHitDirection == FacingDirection.South)
            {
                animator.SetTrigger("ShowLeafBurstEast");
            } else
            {
                animator.SetTrigger("ShowLeafBurstWest");
            }
        }
        if (branchHits == branchHitsToProcess)
        {
            felledBranchesGameObject.SetActive(false);
            felledBranchesCollider.SetActive(false);
            logCollider.SetActive(true);
            logOnly = true;
        }
    }

    public void ProcessHit()
    {
        if (!logOnly)
        {
            branchHits++;

            if (felledBranchesRenderer != null)
                felledBranchesRenderer.enabled = false;

            if (felledBranchesShadowGameObject != null)
                felledBranchesShadowGameObject.SetActive(false);

            UpdateBranchStageOnHit();
        }
        else
        {
            logHits++;

            if (logRenderer != null)
                logRenderer.enabled = false;

            UpdateLogStageOnHit();
        }
    }

    private void UpdateLogStageOnHit()
    {
        if (logStages == null) return;
        int index = Mathf.Clamp(logHits - 1, 0, logStages.Length - 1);
        if (index != 0)
        {
            logStages[index - 1].SetActive(false);
        }

        logStages[index].SetActive(true);

        if (logHits == logHitsToProcess)
        {
            felledLogGameObject.SetActive(false);
            logCollider.SetActive(false);
        }

    }


    public void SetPlayerInChopZone(bool inRange, PlayerAxeController playerAxe)
    {
        playerInChopZone = inRange;
        playerAxeController = playerAxe;

        if (playerInChopZone)
        {
            playerAxeController.SetLogTarget(this);
        }
        else
        {
            playerAxeController.ClearLogTarget(this);
        }
    }
}
