using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarryController : MonoBehaviour
{
    // Player
    [Header("Player Refs")]
    [SerializeField] private Transform holdAnchor;
    [SerializeField] private PlayerActionState actionState;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    // facing parameters so we can update the object's position in the world when the player is carrying
    [Header("Facing Refs")]
    [SerializeField] private FacingDirection facing;

    [Tooltip("Where the hold anchor rests by default relative to the player's feet pivot.")]
    [SerializeField] private Vector3 baseHoldPosition = new Vector3(0f, 0.75f, 0f);

    [Tooltip("Extra offset added when facing north.")]
    [SerializeField] private Vector3 northOffset = new Vector3(0f, 0.15f, 0f);

    [Tooltip("Extra offset added when facing south.")]
    [SerializeField] private Vector3 southOffset = new Vector3(0f, -0.10f, 0f);

    [Tooltip("Extra offset added when facing east.")]
    [SerializeField] private Vector3 eastOffset = new Vector3(0.20f, 0f, 0f);

    [Tooltip("Extra offset added when facing west.")]
    [SerializeField] private Vector3 westOffset = new Vector3(-0.20f, 0f, 0f);

    // Where the object should be dropped in the world
    [Header("Drop Offsets")]
    [Tooltip("Where the object will be dropped relative to player feet")]
    [SerializeField] private Vector3 dropNorth = new Vector3(0f, 0.20f, 0f);
    [SerializeField] private Vector3 dropSouth = new Vector3(0f, -0.05f, 0f);
    [SerializeField] private Vector3 dropEast = new Vector3(0.20f, 0f, 0f);
    [SerializeField] private Vector3 dropWest = new Vector3(-0.20f, 0f, 0f);

    // Carryable Object
    [Header("Object Refs")]
    [SerializeField] private CarryableObjectController carryableObject;
    [SerializeField] private bool isCarrying = false;
    [SerializeField] private SpriteRenderer carryingsr;
    [SerializeField] private string ogSortingLayer;
    [SerializeField] private int ogSortingOrder;

    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!actionState) actionState = GetComponent<PlayerActionState>();
        if (!playerController) playerController = GetComponent<PlayerController>();
    }

    // Always update the hold anchor and object sorting when carrying
    private void Update()
    {
        facing = playerController.Facing;
        if (isCarrying)
        {
            AdjustHoldAnchor();
            AdjustObjectSorting();
        }
    }

    // set object reference
    public void GetCarryableObject(CarryableObjectController obj)
    {
        carryableObject = obj;
    }

    // clear object reference if not carrying
    public void ClearCarryableObject(CarryableObjectController obj)
    {
        if (isCarrying) return;
        if (carryableObject != null && carryableObject == obj)
        {
            carryableObject = null;
        }
    }

    // interact button
    public void OnInteract(InputValue value)
    {
        
        if (carryableObject == null) return;
        if (holdAnchor == null) return;

        if (isCarrying)
        {
            DropObject();
            return; // stop here
        }

        PickUpObject();

    }

    // first interaction loop -- pick up the object
    private void PickUpObject()
    {
        if (actionState.IsBusy && actionState.State != PlayerState.Carrying) return;
        if (carryableObject == null) return;

        animator.SetBool("IsCarrying", true);
        actionState.SetActionState(PlayerState.Carrying);

        carryableObject.CarryObject(holdAnchor);

        carryingsr = carryableObject.SpriteRenderer;
        if (carryingsr != null)
        {
            ogSortingLayer = carryingsr.sortingLayerName;
            ogSortingOrder = carryingsr.sortingOrder;
        }
        isCarrying = true;
    }

    // second interaction loop -- drop the object and restore original object parameters
    private void DropObject()
    {
        if (carryableObject == null) return;

        Vector3 dropOffset = Vector3.zero;

        switch (facing)
        {
            case FacingDirection.North:
                dropOffset = dropNorth;
                break;

            case FacingDirection.South:
                dropOffset = dropSouth;
                break;

            case FacingDirection.East:
                dropOffset = dropEast;
                break;

            case FacingDirection.West:
                dropOffset = dropWest;
                break;
        }

        RestoreObjectSorting();

        carryableObject.DropObject(transform.position + dropOffset);

        animator.SetBool("IsCarrying", false);
        actionState.ClearActionState();
        isCarrying = false;
        carryableObject = null;
        carryingsr = null;

    }

    // adjust the hold anchor as the player moves
    private void AdjustHoldAnchor()
    {
        if (holdAnchor == null) return;

        Vector3 facingOffset = Vector3.zero;

        switch (facing)
        {
            case FacingDirection.North:
                facingOffset = northOffset;
                break;

            case FacingDirection.South:
                facingOffset = southOffset;
                break;

            case FacingDirection.East:
                facingOffset = eastOffset;
                break;

            case FacingDirection.West:
                facingOffset = westOffset;
                break;
        }

        holdAnchor.localPosition = baseHoldPosition + facingOffset;
    }

    // adjust the object sorting position and layer when the player is carrying
    private void AdjustObjectSorting()
    {
        if (carryableObject == null) return;
        if (carryingsr == null) return;

        carryingsr.sortingLayerName = "Characters";

        if(facing == FacingDirection.South)
        {
            carryingsr.sortingOrder = 100;
        } else
        {
            carryingsr.sortingOrder = -100;
        }
    }

    // restore the object's sorting layer and order
    private void RestoreObjectSorting()
    {
        if (carryingsr == null) return;
        carryingsr.sortingLayerName = ogSortingLayer;
        carryingsr.sortingOrder = ogSortingOrder;

    }

}
