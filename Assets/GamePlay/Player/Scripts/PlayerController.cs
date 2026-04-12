using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // --- movement --- //
    [Header("Speed")]
    [SerializeField] public float moveSpeed = 2f;
    private Vector2 movementInput;
    private Rigidbody2D rb;

    // --- animator --- //
    [Header("Animator Parameters")]
    private Animator animator;
    [SerializeField] string facingX = "X";
    [SerializeField] string facingY = "Y";
    [SerializeField] string lastFacingX = "LastX";
    [SerializeField] string lastFacingY = "LastY";

    [Header("Debug")]
    [SerializeField] bool showDebugMessages;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!animator) return;

        float x = movementInput.x;
        float y = movementInput.y;

        animator.SetFloat(facingX, x);
        animator.SetFloat(facingY, y); 

    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
        if (showDebugMessages) Debug.Log($"Move Input: {movementInput}");
        
    }


}
