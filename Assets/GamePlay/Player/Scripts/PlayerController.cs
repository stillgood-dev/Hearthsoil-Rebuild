using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    private Vector2 movementInput;
    private Rigidbody2D rb; // must be Dynamic to work with Unity 2D Physics

    [Header("Animator Parameters")]
    private Animator animator;
    [SerializeField] private string facingX = "X";
    [SerializeField] private string facingY = "Y";
    [SerializeField] private string lastFacingX = "LastX";
    [SerializeField] private string lastFacingY = "LastY";
    [SerializeField] private string speedParam = "Speed";

    private const float FaceDeadZone = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool showDebugMessages;
    [SerializeField] private bool debugIsMoving;

    public bool IsMoving { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Default to south on startup
        animator.SetFloat(lastFacingX, 0f);
        animator.SetFloat(lastFacingY, -1f);
        animator.SetFloat(facingX, 0f);
        animator.SetFloat(facingY, -1f);
    }

    private void Update()
    {
        if (!animator) return;

        float x = movementInput.x;
        float y = movementInput.y;
        // moving when input.x or input.y > 0
        bool moving = Mathf.Abs(x) > FaceDeadZone || Mathf.Abs(y) > FaceDeadZone;
        IsMoving = moving;
        debugIsMoving = moving;

        int faceX = 0;
        int faceY = -1;

        // keep north and south facing if diagonal ties
        if (moving)
        {
            if (y < -FaceDeadZone)
            {
                faceX = 0;
                faceY = -1;
            }
            else if (y > FaceDeadZone)
            {
                faceX = 0;
                faceY = 1;
            }
            else if (x > FaceDeadZone)
            {
                faceX = 1;
                faceY = 0;
            }
            else if (x < -FaceDeadZone)
            {
                faceX = -1;
                faceY = 0;
            }

            animator.SetFloat(lastFacingX, faceX);
            animator.SetFloat(lastFacingY, faceY);
            animator.SetFloat(facingX, faceX);
            animator.SetFloat(facingY, faceY);
        }
        else
        {
            animator.SetFloat(facingX, animator.GetFloat(lastFacingX));
            animator.SetFloat(facingY, animator.GetFloat(lastFacingY));
        }

        animator.SetFloat(speedParam, moving ? 1f : 0f);
    }

    private void FixedUpdate()
    {
        Vector2 move = movementInput;

        if (move.sqrMagnitude > 1f)
            move = move.normalized;
        // move player
        rb.linearVelocity = move * moveSpeed;
    }

    public void OnMove(InputValue value)
    {
        // cache movement input
        movementInput = value.Get<Vector2>();

        if (showDebugMessages)
            Debug.Log($"Move Input: {movementInput}");
    }
}