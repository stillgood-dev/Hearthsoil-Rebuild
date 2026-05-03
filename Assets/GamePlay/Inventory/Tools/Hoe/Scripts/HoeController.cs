using UnityEngine;

public class HoeController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private FacingDirection facing;

    [Header("Hoe Refs")]
    [SerializeField] private Animator animator;


    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        facing = playerController.Facing;
    }

    public void PlayHoe()
    {
        if (playerController == null) return;
        if (facing == FacingDirection.North)
        {
            animator.Play("Hoe_N");
        }
        else if (facing == FacingDirection.South)
        {
            animator.Play("Hoe_S");
        }
        else if (facing == FacingDirection.East)
        {
            animator.Play("Hoe_E");
        }
        else if (facing == FacingDirection.West)
        {
            animator.Play("Hoe_W");
        }
    }
}
