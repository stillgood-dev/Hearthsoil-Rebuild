using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAxeController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private Animator animator;

    [Header("Axe Refs")]
    [SerializeField] private AxeController axe;
    [SerializeField] private bool isChopping;
    public bool IsChopping => isChopping;

    [Header("Equip Status")]
    [Tooltip("Temporary bool to indicate axe has been equipped, will remove when I build equip system")]
    [SerializeField] private bool isEquipped = false;

    private void Awake()
    {
        if(!animator) animator = GetComponent<Animator>();
        if (!playerActionState) playerActionState = GetComponent<PlayerActionState>();
    }

    private void Update()
    {
        
    }

    public void OnInteract()
    {
        if (!isEquipped) return;
        BeginChop();
    }

    public void BeginChop()
    {
        // prevent chopping if midchop
        if (isChopping) return;
        animator.SetBool("IsChopping", true);
        axe.PlayAxeSwing();
        isChopping = true;
        playerActionState.SetActionState(PlayerState.Chopping);
        
    }

    // --- animation event --- //
    public void EndChop()
    {
        animator.SetBool("IsChopping", false);
        isChopping = false;
        playerActionState.ClearActionState();
    }

}
