using UnityEngine;

public class AxeController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAxeController axeController;
    [SerializeField] private FacingDirection facing;

    [Header("Axe Refs")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if(!animator) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        facing = playerController.Facing;
    }

    public void PlayAxeSwing()
    {
        if (playerController == null) return;
        if(facing == FacingDirection.North)
        {
            animator.Play("Axe_SideSwing_N");
        } else if (facing == FacingDirection.South)
        {
            animator.Play("Axe_SideSwing_S");
        } else if (facing == FacingDirection.East)
        {
            animator.Play("Axe_SideSwing_E");
        } else if (facing == FacingDirection.West)
        {
            animator.Play("Axe_SideSwing_W");
        }
    }

}
