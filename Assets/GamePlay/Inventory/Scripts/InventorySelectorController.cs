using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySelectorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryUIController inventoryUIController;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private PlayerToolState playerToolState;
    [SerializeField] private PlayerCarryController carryController;

    [Header("Selector")]
    [SerializeField] private RectTransform inventorySelector;

    [Header("Inventory Slots")]
    // RectTransform because it's a UI image
    [SerializeField] private RectTransform[] inventorySlots;



    private int selectedSlotIndex = 0;

    private void Awake()
    {
        if(inventoryUIController == null) 
            inventoryUIController = GetComponent<InventoryUIController>();

        if(!playerToolState)
            playerToolState = GetComponent<PlayerToolState>();

        if(!carryController)
            carryController = GetComponent<PlayerCarryController>();

        if(!inventoryManager)
            inventoryManager = GetComponent<InventoryManager>();    
    }

    public void OnInteract()
    {
        if (!inventoryUIController.IsOpen) return;
        SelectSlot();
    }

    public void OnMove(InputValue value)
    {
        if (!inventoryUIController.IsOpen) return;
        if (inventorySlots == null) return;

        Vector2 movementInput = value.Get<Vector2>();
        
        if(movementInput.x > 0.5f)
        {
            MoveSelector(1);
        }
        else if (movementInput.x < -0.5f)
        {
            MoveSelector(-1);
        }
    }

    private void MoveSelector(int direction)
    {
        selectedSlotIndex += direction; // += means add to the existing value and reassign the result back to the variable

        // if a value falls outside these boundaries, return the nearest boundary, otherwise return original value
        selectedSlotIndex = Mathf.Clamp(
            // value       // min            // max   
            selectedSlotIndex, 0, inventorySlots.Length - 1
        );

        UpdateSelectorPosition();
        
    }

    public void UpdateSelectorPosition()
    {
        if (!inventorySelector) return;
        inventorySelector.position =
            inventorySlots[selectedSlotIndex].position;
    }

    private void SelectSlot()
    {
        if (inventorySlots == null) return;
        if (inventoryManager == null) return;

        InventorySlot inventorySlot = 
            inventorySlots[selectedSlotIndex].GetComponent<InventorySlot>();

        if (inventorySlot == null) return;
        if (inventorySlot.IsEmpty) return;

        InventorySlotType slotType = inventorySlot.SlotType;

        // ------------ tool selection --------------- //
        if(slotType == InventorySlotType.Tool)
        {
            EquippedTool tool = inventorySlot.Tool;
            playerToolState.EquipTool(tool);
            inventoryUIController?.CloseInventory();
            return;
        }

        // ------------- resource selection -------------- //
        else if(slotType == InventorySlotType.Resource)
        {
            // grab the prefab from the slot that was wired in InventorySlot
            GameObject resourcePrefab =
                inventorySlot.ResourcePrefab;

            if (resourcePrefab == null) return;

            // instantiate the object at the player's position
            GameObject resource = Instantiate(
                // current prefab    // player position     // no rotation
                resourcePrefab, transform.position, Quaternion.identity
            );

            // grab the prefab's carryable object controller
            CarryableObjectController carryable = 
                resource.GetComponentInChildren<CarryableObjectController>();

            // destroy it if it's not carryable or doesn't have the component attached
            if(carryable == null)
            {
                Destroy(resource);
                return;
            }

            // close the inventory
            inventoryUIController?.CloseInventory();
            // allow the player to carry it
            carryController.CarryFromInventory(carryable);

            // deplete it from inventory manager
            ResourceType resourceType = carryable.ReceivableController.ResourceType;
            inventoryManager?.DepleteResource(resourceType);

            return;
        }

            

    }


}
