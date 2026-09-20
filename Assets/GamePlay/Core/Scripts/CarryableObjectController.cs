using UnityEngine;

public class CarryableObjectController : MonoBehaviour
{

    [Header("Player Refs")]
    [SerializeField] private PlayerCarryController carryController;

    [Header("Object Refs")]
    [SerializeField] private GameObject carryableObject;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Collider2D dropBlocker;
    [SerializeField] private SpriteRenderer worldSR;
    [SerializeField] private SpriteRenderer heldSR;
    public SpriteRenderer HeldSR => heldSR; // PlayerCarryController only cares about the held sprite renderer

    [SerializeField] private ReceivableObjectController receivableController;

    public ReceivableObjectController ReceivableController => receivableController;

    private void Awake()
    {

        if (carryableObject == null)
        {
            carryableObject = transform.parent != null 
                ? transform.parent.gameObject 
                : gameObject;
        }

        if (worldSR != null) worldSR.enabled = true;
        if (heldSR != null) heldSR.enabled = false;

        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();

            if (boxCollider == null && transform.parent != null)
            {
                boxCollider = transform.parent.GetComponent<BoxCollider2D>();
            }
        }

        if (receivableController == null)
        {
            receivableController = GetComponent<ReceivableObjectController>();
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
        if (boxCollider != null) boxCollider.enabled = false;

        if (dropBlocker != null)
            dropBlocker.enabled = false;


        // swap sprite to held sprite
        if (worldSR != null) worldSR.enabled = false;
        if(heldSR != null) heldSR.enabled = true;
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

        if (dropBlocker != null)
            dropBlocker.enabled = true;

        // swap sprite to world sprite
        if (worldSR != null) worldSR.enabled = true;
        if(heldSR != null) heldSR.enabled = false;
    }
}
