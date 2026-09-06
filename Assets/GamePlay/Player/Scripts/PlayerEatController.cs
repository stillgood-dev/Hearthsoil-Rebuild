using UnityEngine;

public class PlayerEatController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform eatAnchor;
    [SerializeField] private PlayerActionState actionState;
    [SerializeField] private Animator animator;

    [Header("Object to Eat")]
    [SerializeField] private PlayerEatController eatController;
}
