using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReceiveController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform receiveAnchor;
    [SerializeField] private PlayerActionState actionState;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    [Header("Receiveable Object")]
    [SerializeField] private ReceivableObjectController receivableObject;
    [SerializeField] private BoxCollider2D objectCollider;
    [SerializeField] private Animator objectAnimator;
    [SerializeField] private bool hasReceivedObject = false;
    [SerializeField] private bool inInventory = false;

    public bool HasReceivedObject => hasReceivedObject;

    private void Awake()
    {
        if(!animator) animator = GetComponent<Animator>();
        if(!playerController) playerController = GetComponent<PlayerController>();
        if(!actionState) actionState = GetComponent<PlayerActionState>();
    }

    // set object reference when in the object's trigger zone
    public void SetCurrentReceivableObject(ReceivableObjectController obj)
    {
        receivableObject = obj;
        
    }

    // clear object reference when leaving object's trigger zone
    public void ClearCurrentReceivableObject(ReceivableObjectController obj)
    {
        if (hasReceivedObject) return;
        if (receivableObject != null && receivableObject == obj)
            receivableObject = null;

    }

    public void OnInteract()
    {
        if (receivableObject == null) return;

        // --- accept object --- //
        if (hasReceivedObject)
        {
            int x = 0; int y = -1;

            receivableObject.AcceptObject();
            // move object to inventory (just a placeholder bool for now)
            inInventory = true;
            // allow player to move again
            actionState.ClearActionState();
            // force player to face south since that's the receiving animation direction
            animator.SetFloat("LastX", x);
            animator.SetFloat("LastY", y);
            animator.Play("Idle");

            // clear hasReceivedObject
            hasReceivedObject = false;
            return;
        }

        // --- receive object initially --- //

        // trigger receive animation
        animator.SetTrigger("Receive");
        // receive the object above the head
        receivableObject.ReceiveObject(receiveAnchor);
        // set player state to "Receiving" so player can't move
        actionState.SetActionState(PlayerState.Receiving);
        // mark object as received so we can accept it
        hasReceivedObject = true;

    }


}
