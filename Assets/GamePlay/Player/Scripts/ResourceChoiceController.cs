using UnityEngine;

public class ResourceChoiceController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerInventoryState inventoryState;
    [SerializeField] private PlayerActionCoordinator actionCoordinator;
    [SerializeField] private PlayerCarryController carryController;

    [Header("UI References")]
    [SerializeField] private ResourceChoiceUI resourceChoiceUI;

    public bool IsOpen => resourceChoiceUI != null && resourceChoiceUI.IsOpen;

    private void Awake()
    {
        if (!inventoryState) inventoryState = GetComponent<PlayerInventoryState>();
        if (!actionCoordinator) actionCoordinator = GetComponent<PlayerActionCoordinator>();
        if (!carryController) carryController = GetComponent<PlayerCarryController>();
    }

    public void ShowResourceChoices(CarryableObjectController carryableObject)
    {
        if (carryableObject == null) return;

        if(resourceChoiceUI == null)
        {
            Debug.LogError("Resource Choice UI not wired!", this);
            return;
        }

        ReceivableObjectController resource = carryableObject.ReceivableController;

        if (resource == null) return;

        bool canEat = resource.IsEdible;
        bool canDrop = true;
        bool canStore = resource.IsStorable && inventoryState.HasSatchel;

        resourceChoiceUI.Show("What would you like to do?", canEat, canDrop, canStore);
    }

    public void HandleResourceChoice()
    {
        switch (resourceChoiceUI.CurrentChoice)
        {
            case ResourceChoiceUI.ResourceChoice.Drop:
                resourceChoiceUI?.Hide();
                carryController?.DropObject();
                break;

            case ResourceChoiceUI.ResourceChoice.Eat:
                resourceChoiceUI?.Hide();
                // carryController?.EatObject();
                Debug.Log("Eat Selected");
                break;

            case ResourceChoiceUI.ResourceChoice.Store:
                resourceChoiceUI?.Hide();
                carryController?.StoreObject();
                break;
        }
    }
}
