using System.Collections;
using UnityEngine;

public class ChoppableTreeController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerAxeController playerAxeController;
    [SerializeField] private bool playerInChopZone;
    [SerializeField] public FacingDirection playerFaceDirection;
    [SerializeField] public FacingDirection lastHitDirection;

    [Header("Tree Refs")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer hitFlashSprite;
    [SerializeField] private int hitFlashFrameCount = 15;
    [SerializeField] private GameObject fadeZone;
    [SerializeField] private SpriteRenderer stumpShadow;
    [SerializeField] private BoxCollider2D standingCollider;

    [Header("Chop Refs")]
    [SerializeField] private int hitsToFall = 3;
    [SerializeField] private int treeHits;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isFelled;

    // --- hit flash coroutine --- //
    private Coroutine hitFlashCoroutine;

    private void Awake()
    {
        if (!hitFlashSprite)
        {
            var root = transform.parent.parent;
            hitFlashSprite = root?.Find("Tree/HitFlash")?.GetComponent<SpriteRenderer>();
        }

        if (hitFlashSprite)
        {
            hitFlashSprite.enabled = false;
        }

        if (!animator)
        {
            var root = transform.parent.parent;
            animator = root?.Find("Tree")?.GetComponent<Animator>();
        }

        if (!fadeZone)
        {
            var root = transform.parent.parent;
            fadeZone = root?.Find("Colliders/FadeZone")?.gameObject;
        }

        if (!stumpShadow)
        {
            var root = transform.parent.parent;
            stumpShadow = root?.Find("Stump/Shadow")?.GetComponent<SpriteRenderer>();
        }
        if (!standingCollider)
        {
            var root = transform.parent.parent;
            standingCollider = root?.Find("Colliders/Standing")?.GetComponent<BoxCollider2D>();
        }

    }

    private void Update()
    {
        playerFaceDirection = playerAxeController.FaceDir;
    }

    // set player in range of tree
    public void SetPlayerInChopZone(bool inRange, PlayerAxeController playerAxe)
    {
        playerInChopZone = inRange;
        playerAxeController = playerAxe;

        if (playerInChopZone)
        {
            playerAxeController.SetChopTarget(this);
        } else
        {
            playerAxeController.ClearChopTarget(this);
        }
    }
    // --- HIT --- //

    public void RegisterHit()
    {
        treeHits++;
        ShowHitFlash();
        BeginTreeFall();
    }

    // --- HIT FLASH COROUTINE --- //

    // show hit flash on hit
    private void ShowHitFlash()
    {
        if (!hitFlashSprite) return;
        if(hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
            hitFlashSprite.enabled=false;
        }
        hitFlashCoroutine = StartCoroutine(HitFlash());
        
    }

    // show hit flash for a very brief time
    private IEnumerator HitFlash()
    {
        hitFlashSprite.enabled = true; // enable flash sprite

        // show hit flash sprite for duration of frame count
        for(int i = 0; i < Mathf.Max(1, hitFlashFrameCount); i++)
        {
            yield return null;
        }

        hitFlashSprite.enabled = false;
        hitFlashCoroutine = null;
    }

    // --- TREE FALL --- //
    private void BeginTreeFall()
    {
        if (treeHits == hitsToFall)
        {
            isFalling = true;
            lastHitDirection = playerFaceDirection;
            if(lastHitDirection == FacingDirection.East ||  lastHitDirection == FacingDirection.South)
            {
                animator.Play("PineTree_Fell_E", 0, 0f);
            } else
            {
                animator.Play("PineTree_Fell_W", 0, 0f);
            }
            
        }
    }

    // animation event
    public void OnTreeFall()
    {
        isFalling = true;
        if (stumpShadow) stumpShadow.enabled = false;
        DisableFadeZone();
    }

    // animation event
    public void OnTreeFell()
    {
        isFalling = false;
        isFelled = true;
        DisableStandingCollider();
    }

    private void DisableFadeZone()
    {
        if (fadeZone) fadeZone.SetActive(false);
    }

    private void DisableStandingCollider()
    {
        if (!standingCollider) return;
        standingCollider.enabled = false;
    }




}
