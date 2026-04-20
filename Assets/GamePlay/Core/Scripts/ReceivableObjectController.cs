using UnityEngine;

public class ReceivableObjectController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerReceiveController receiveController;

    [SerializeField] private GameObject receiveGameObject;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Animator animator;

    private void Awake()
    {

        if(receiveGameObject == null)
        {
            receiveGameObject = transform.parent != null ? transform.parent.gameObject : gameObject;
        }

        if(boxCollider == null)
        {
            boxCollider = transform.parent != null ? transform.parent.GetComponent<BoxCollider2D>() : boxCollider;
        }

        if(animator == null)
        {
            animator = transform.parent != null ? transform.parent.GetComponent<Animator>() : animator;
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
        boxCollider.enabled = false;
        receiveGameObject.transform.position = receiveAnchor.position;
        animator.Play("ReceiveSatchel");

        
    }

    public void AcceptObject()
    {
        if (receiveGameObject == null) return;
        receiveGameObject.SetActive(false);
    }
}
