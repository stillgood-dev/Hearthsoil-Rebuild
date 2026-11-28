using UnityEngine;

public class ReceivableObjectController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerReceiveController receiveController;

    [Header("Object References")]
    [SerializeField] private GameObject receivableGameObject;
    [SerializeField] private SpriteRenderer worldSR;
    [SerializeField] private SpriteRenderer heldSR;
    [SerializeField] private bool isSatchel = false;


    [SerializeField] private Collider2D receivableObjectCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private string receivableObjectAnimationName;
    [SerializeField] private string receivableObjectIdleAnimationName;
    [SerializeField] private string receivableObjectName;
    [SerializeField] private CarryableObjectController carryableController;
    public CarryableObjectController CarryableController => carryableController;
    public bool IsSatchel => isSatchel;


    [Header("Tool References")]
    [SerializeField] private EquippedTool toolType;


    [Header("Resource References")]
    [SerializeField] private ResourceData resourceData;

    // make resource data public
    public bool IsTool => toolType != EquippedTool.None;
    public EquippedTool ToolType => toolType;

    public bool IsEdible => resourceData != null && resourceData.isEdible;
    public bool IsStorable => resourceData != null && resourceData.isStorable;
    public ResourceType ResourceType => resourceData.resourceType;
    public float SatietyRestoration => resourceData != null 
        ? resourceData.satietyRestoration 
        : 0f;
    public string ResourceName => resourceData != null ? 
        resourceData.resourceName 
        : null;  


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
        if (receivableObjectCollider == null)
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

        // disable object colliders
        if (receivableObjectCollider != null)
            receivableObjectCollider.enabled = false;

        // swap from world sprite to held sprite
        if (worldSR != null) worldSR.enabled = false;
        if (heldSR != null) heldSR.enabled = true;

        // snap the received object above the player's head
        receivableGameObject.transform.position = receiveAnchor.position;

        // play animation if there is one
        if (animator != null && !string.IsNullOrEmpty(receivableObjectAnimationName))
        {
            animator.Play(receivableObjectAnimationName);
        }


    }

    // De-snap from player and reenable collider
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

        // destroy for objects you want to destroy
        GetComponent<PersistentDestroyableObject>()?.MarkDestroyed();

        // put in inventory
        // for later
    }
}
