using UnityEngine;

public class PlayerHoeController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private Animator animator;
    [SerializeField] private FacingDirection faceDir;

    public FacingDirection FaceDir => faceDir;



    [Header("Hoe Refs")]
    [SerializeField] private HoeController hoe;
    [SerializeField] private bool isHoeing;
    public bool IsHoeing => isHoeing;

    [Header("Equip Status")]
    [Tooltip("Temporary bool to indicate hoe has been equipped, will remove when I build equip system")]
    [SerializeField] private bool isEquipped = false;

    private void Awake()
    {
        if (!playerController) playerController = GetComponent<PlayerController>();
        if (!animator) animator = GetComponent<Animator>();
        if (!playerActionState) playerActionState = GetComponent<PlayerActionState>();
    }

    private void Update()
    {
        faceDir = playerController.Facing;

    }

    public void OnInteract()
    {
        if (!isEquipped) return;
        BeginHoeing();
    }

    public void BeginHoeing()
    {
        if (playerActionState.IsBusy) return;
        if (isHoeing) return;
        animator.SetBool("IsHoeing", true);
        hoe.PlayHoe();
        isHoeing = true;
        playerActionState.SetActionState(PlayerState.Hoeing);

    }

    // ---- animation events --- //
    public void EndHoeing()
    {
        animator.SetBool("IsHoeing", false);
        isHoeing = false;
        playerActionState.ClearActionState();
    }
}
