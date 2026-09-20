using UnityEngine;
using UnityEngine.UIElements;

public class ReceivableObjectController_TEST : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerReceiveController_TEST receiveController;

    [Header("Object References")]
    [SerializeField] private GameObject receivableGameObject;
    [SerializeField] private SpriteRenderer worldSR;
    [SerializeField] private SpriteRenderer heldSR;

    [SerializeField] private Collider2D receivableObjectCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private string receivableObjectAnimationName;
    [SerializeField] private string receivableObjectIdleAnimationName;
    [SerializeField] private string receivableObjectName;
    [SerializeField] private CarryableObjectController carryableController;
    public CarryableObjectController CarryableController => carryableController;


    [Header("Tool References")]
    [SerializeField] private bool isTool;
    [SerializeField] private EquippedTool toolType;
    public bool IsTool => isTool;
    public EquippedTool ToolType => toolType;


    [Header("Resource References")]
    [SerializeField] private ResourceType resourceType = ResourceType.None;
    [SerializeField] private bool isEdible;
    [SerializeField] private bool isStorable;
    public bool IsStorable => isStorable;
    public bool IsEdible => isEdible;
    public ResourceType ResourceType => resourceType;
    public string ReceivableObjectName => receivableObjectName;

    private void Awake()
    {
        // get receivable game object
        if (receivableGameObject == null)
        {
            receivableGameObject = 
                transform.parent != null ? 
                transform.parent.gameObject : 
                gameObject;
        }

        // ensure world sprite is enabled
        if (worldSR != null) worldSR.enabled = true;

        // disable held sprite on awake
        if (heldSR != null) heldSR.enabled = false;  

        // get object collider
        if(receivableObjectCollider == null)
        {
            receivableObjectCollider = 
                transform.parent != null ? 
                transform.parent.GetComponent<BoxCollider2D>() : 
                receivableObjectCollider;
        }

        // get object animator
        if (animator == null)
        {
            animator =
                transform.parent != null ? 
                transform.parent.GetComponent<Animator>() : 
                animator;
        }

        // get object carryable controller
        if (carryableController == null)
        {
            carryableController = GetComponent<CarryableObjectController>();

        }
    }


    // Tell the player this object is the receivable object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        receiveController = other.GetComponent<PlayerReceiveController_TEST>();
        if (receiveController != null)
        {
            receiveController.SetCurrentReceivableObject(this);

        }

    }

    // Clear the receivable object once the player leaves the zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        receiveController = other.GetComponent<PlayerReceiveController_TEST>();
        if (receiveController != null)
        {
            receiveController.ClearCurrentReceivableObject(this);

        }
    }

    public void ReceiveObject(Transform receiveAnchor)
    {
        if (receiveAnchor == null) return;
        if (receiveController == null) return;

        // disable object colliders
        if (receivableObjectCollider != null)
            receivableObjectCollider.enabled = false;

        // swap from world sprite to held sprite
        if(worldSR != null) worldSR.enabled = false;
        if (heldSR != null) heldSR.enabled = true;

        // snap the received object above the player's head
        receivableGameObject.transform.position = receiveAnchor.position;

        // play animation if there is one
        if (animator != null && !string.IsNullOrEmpty(receivableObjectAnimationName))
        {
            animator.Play(receivableObjectAnimationName);
        }

    }

    public void DropObject(Vector3 worldPosition)
    {
        // de-snap from anchor above player's head
        receivableGameObject.transform.SetParent(null);

        // swap back to world sprite
        if (worldSR != null && !worldSR.enabled) worldSR.enabled = true;
        if (heldSR != null && heldSR.enabled) heldSR.enabled = false;

        // drop the object into the world
        receivableGameObject.transform.position = worldPosition;

        // re-enable collider, if there is one
        if (receivableObjectCollider != null && !receivableObjectCollider.enabled)
        {
            receivableObjectCollider.enabled = true;
        }

        // stop animation if there is one
        if (animator != null && !string.IsNullOrEmpty(receivableObjectAnimationName))
        {
            animator.Play(receivableObjectIdleAnimationName);
        }
        
    }

    public void AcceptObject()
    {
        if (receivableGameObject == null) return;

        // destroy for now until we get an inventory
        GetComponent<PersistentDestroyableObject>()?.MarkDestroyed();
    }

}
