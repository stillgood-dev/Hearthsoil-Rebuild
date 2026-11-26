using UnityEngine;
using UnityEngine.SceneManagement;

public class CarryableObjectController : MonoBehaviour
{

    [Header("Player Refs")]
    [SerializeField] private PlayerCarryController carryController;

    [Header("Object Refs")]
    [SerializeField] private GameObject carryableObject;
    [SerializeField] private Collider2D dropBlocker;
    [Tooltip("Need two different sprite renderers, even if they are the same")]
    [SerializeField] private SpriteRenderer worldSR;
    [SerializeField] private SpriteRenderer heldSR;
    [Tooltip("Required for keeping track of where object went")]
    [SerializeField] private PersistentDestroyableObject persistentObject;

    [Header("Optional")]
    [SerializeField] private BoxCollider2D boxCollider;


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

        // disable sprite renderer in world
        if (worldSR != null) worldSR.enabled = true;
        // enable sprite renderer in player's hands
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

        if (persistentObject == null)
        {
            persistentObject = transform.root.GetComponent<PersistentDestroyableObject>();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // if anything other than the player walks in to this trigger, don't do anything
        if (!other.CompareTag("Player")) return;

        // otherwise, grab the player's carry controller
        carryController = other.GetComponent<PlayerCarryController>();
        if(carryController != null)
        {
            carryController.GetCarryableObject(this);
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if anything other than the player walks into this trigger, don't do anything
        if (!other.CompareTag("Player")) return;

        // clear the object from being able to be carried (unless being carried as defined in ClearCarryableObject())
        carryController = other.GetComponent<PlayerCarryController>();
        if (carryController != null)
        {
            carryController.ClearCarryableObject(this);
        }
    }

    // Snap to player (used by PlayerCarryController)
    public void CarryObject(Transform holdAnchor)
    {
        if (holdAnchor == null) return;
        if (carryController == null) return;

        // Remove original world instance from persistence
        if(persistentObject == null) return;
        // add object parameters to WorldState
        persistentObject.MarkRelocated();

        // optional: remove if present
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

        // move carried object out of DontDestroyOnLoad
        // and into current gameplay scene
        SceneManager.MoveGameObjectToScene(
            carryableObject,
            SceneManager.GetActiveScene()
        );

        carryableObject.transform.position = worldPosition;
        persistentObject?.MarkDropped(worldPosition);

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

    public void DestroyObject()
    {
        Destroy(carryableObject);
    }
}
