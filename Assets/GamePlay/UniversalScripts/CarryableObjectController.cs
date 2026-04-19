using UnityEngine;

public class CarryableObjectController : MonoBehaviour
{

    [Header("Player Refs")]
    [SerializeField] private PlayerCarryController carryController;

    [Header("Object Refs")]
    [SerializeField] private GameObject carryableObject;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    private void Awake()
    {

        if (carryableObject == null)
        {
            carryableObject = transform.parent != null 
                ? transform.parent.gameObject 
                : gameObject;
        }

        if (boxCollider == null)
        {
            boxCollider = transform.parent != null 
                ? transform.parent.GetComponent<BoxCollider2D>() 
                : boxCollider;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = transform.parent != null
                ? transform.parent.GetComponent<SpriteRenderer>()
                : GetComponent<SpriteRenderer>();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        carryController = other.GetComponent<PlayerCarryController>();
        if(carryController != null)
        {
            carryController.GetCarryableObject(this);
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        carryController = other.GetComponent<PlayerCarryController>();
        if (carryController != null)
        {
            carryController.ClearCarryableObject(this);
        }
    }

    // Snap to player
    public void CarryObject(Transform holdAnchor)
    {
        if (holdAnchor == null) return;
        if (carryController == null) return;
        boxCollider.enabled = false;
        carryableObject.transform.SetParent(holdAnchor);
        carryableObject.transform.localPosition = Vector3.zero;
    }

    // De-snap from player and reenable collider
    public void DropObject(Vector3 worldPosition)
    {
        carryableObject.transform.SetParent(null);
        carryableObject.transform.position = worldPosition;

        if(boxCollider != null && !boxCollider.enabled)
        {
            boxCollider.enabled = true;
        }
    }
}
