using UnityEngine;

public class PlayerInventoryState : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private bool hasSatchel;

    public bool HasSatchel => hasSatchel;
}
