using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private PlayerInventoryState inventoryState;
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private InventoryUIController inventoryUI;

    private void Awake()
    {
        if(inventoryState == null) 
            inventoryState = GetComponent<PlayerInventoryState>();

        if(inventoryUI == null)
            inventoryUI = GetComponent<InventoryUIController>();

        if (!playerActionState) 
            playerActionState = GetComponent<PlayerActionState>();
    }

    public void OnInventory()
    {
        if (inventoryState == null) return;
        if (!inventoryState.HasSatchel) return;

        if (playerActionState.IsBusy &&
            playerActionState.State != PlayerState.Inventory)
        {
            return;
        }

        if (playerActionState.State == PlayerState.Inventory)
        {
            inventoryUI.CloseInventory();
            return;
        }

        inventoryUI.OpenInventory();
    }
}
