using UnityEngine;

public class MacheteController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAxeController axeController;
    [SerializeField] private FacingDirection facing;

    [Header("Machete Refs")]
    [SerializeField] private Animator animator;


    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        facing = playerController.Facing;
    }

    public void PlayMacheteSideSwing()
    {
        if (playerController == null) return;
        if (facing == FacingDirection.North)
        {
            animator.Play("Machete_SideSwing_N");
        }
        else if (facing == FacingDirection.South)
        {
            animator.Play("Machete_SideSwing_S");
        }
        else if (facing == FacingDirection.East)
        {
            animator.Play("Machete_SideSwing_E");
        }
        else if (facing == FacingDirection.West)
        {
            animator.Play("Machete_SideSwing_W");
        }
    }
}
