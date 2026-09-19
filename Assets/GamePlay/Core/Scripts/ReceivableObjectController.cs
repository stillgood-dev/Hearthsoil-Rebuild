using UnityEngine;

public class ReceivableObjectController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerReceiveController receiveController;

    [Header("Object Refs")]
    [SerializeField] private GameObject receiveWorldGameObject;
    [SerializeField] private SpriteRenderer worldSpriteRenderer;
    [SerializeField] private SpriteRenderer heldSpriteRenderer;


    [SerializeField] private Collider2D receiveObjectCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private string receiveObjectAnimationName;
    [SerializeField] private string receiveObjectName;
    [SerializeField] private CarryableObjectController carryableController;

    public CarryableObjectController CarryableController => carryableController;


    [Header("Tool Data")]
    [SerializeField] private bool isTool;
    [SerializeField] private EquippedTool toolType;
    
    public bool IsTool => isTool;
    public EquippedTool ToolType => toolType;


    [Header("Resource Data")]
    [SerializeField] private ResourceType resourceType = ResourceType.None;
    [SerializeField] private bool isEdible;
    [SerializeField] private bool isStorable;

    public bool IsEdible => isEdible;
    public bool IsStorable => isStorable;
    public ResourceType ResourceType => resourceType;

    public string ReceiveObjectName => receiveObjectName; 

    private void Awake()
    {

        if(receiveWorldGameObject == null)
        {
            receiveWorldGameObject = transform.parent != null ? transform.parent.gameObject : gameObject;
        }

        if (worldSpriteRenderer != null) worldSpriteRenderer.enabled = false;
        if (heldSpriteRenderer != null) heldSpriteRenderer.enabled = false;

        if(receiveObjectCollider == null)
        {
           receiveObjectCollider = transform.parent != null ? transform.parent.GetComponent<BoxCollider2D>() : receiveObjectCollider;
        }

        if(animator == null)
        {
            animator = transform.parent != null ? transform.parent.GetComponent<Animator>() : animator;
        }

        if(carryableController == null)
        {
            carryableController = GetComponent<CarryableObjectController>();

        }
    }

    // Tell the player this object is the receivable object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        receiveController = other.GetComponent<PlayerReceiveController>();
        if(receiveController != null)
        {
            receiveController.SetCurrentReceivableObject(this);
            
        }
       
    }

    // Clear the receivable object once the player leaves the zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        receiveController = other.GetComponent<PlayerReceiveController>();
        if (receiveController != null)
        {
            receiveController.ClearCurrentReceivableObject(this);

        }
    }

    public void ReceiveObject(Transform receiveAnchor)
    {
        if (receiveAnchor == null) return;
        if (receiveController == null) return;

        if (receiveObjectCollider != null) receiveObjectCollider.enabled = false;
        receiveWorldGameObject.transform.position = receiveAnchor.position;
        if(animator != null) animator.Play(receiveObjectAnimationName);

        
    }

    // De-snap from player and reenable collider
    public void DropObject(Vector3 worldPosition)
    {
        receiveWorldGameObject.transform.SetParent(null);
        receiveWorldGameObject.transform.position = worldPosition;

        if (receiveObjectCollider != null && !receiveObjectCollider.enabled)
        {
            receiveObjectCollider.enabled = true;
        }
    }

    public void AcceptObject()
    {
        if (receiveWorldGameObject == null) return;
        // destroy just for now until we get an inventory
        GetComponent<PersistentDestroyableObject>()?.MarkDestroyed();
    }
}
