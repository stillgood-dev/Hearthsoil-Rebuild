using System.Runtime.CompilerServices;
using UnityEngine;

public class ReceivableObjectController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerReceiveController receiveController;

    [Header("Object Refs")]
    [SerializeField] private GameObject receiveGameObject;
    [SerializeField] private Collider2D receiveObjectCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private string receiveObjectAnimationName;


    [Header("Tool Data")]
    [SerializeField] private bool isTool;
    [SerializeField] private EquippedTool toolType;
    
    public bool IsTool => isTool;
    public EquippedTool ToolType => toolType;

    private void Awake()
    {

        if(receiveGameObject == null)
        {
            receiveGameObject = transform.parent != null ? transform.parent.gameObject : gameObject;
        }

        if(receiveObjectCollider == null)
        {
           receiveObjectCollider = transform.parent != null ? transform.parent.GetComponent<BoxCollider2D>() : receiveObjectCollider;
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
        //if (receiveObjectCollider != null) receiveObjectCollider.enabled = false;
        receiveGameObject.transform.position = receiveAnchor.position;
        animator.Play(receiveObjectAnimationName);

        
    }

    public void AcceptObject()
    {
        if (receiveGameObject == null) return;
        receiveGameObject.SetActive(false);
    }
}
