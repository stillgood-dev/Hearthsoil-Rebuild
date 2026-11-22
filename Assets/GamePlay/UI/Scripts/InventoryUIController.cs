using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("Inventory Canvas")]
    [SerializeField] private GameObject inventoryCanvas;

    [Header("Inventory Icons")]
    [SerializeField] private GameObject macheteIcon;



    private void Awake()
    {
        if(!inventoryManager) inventoryManager = GetComponent<InventoryManager>();

        if(inventoryCanvas != null) inventoryCanvas.SetActive(false);
    }

    public void OpenInventory()
    {
        if(inventoryCanvas == null) return;
        inventoryCanvas.SetActive(true);

        RefreshInventory();
    }

    public void RefreshInventory()
    {
        if(inventoryManager == null) return;
        if (macheteIcon == null) return;

        macheteIcon.SetActive(
            inventoryManager.HasTool(EquippedTool.Machete)
        );
    }

    public void CloseInventory()
    {
        if (inventoryCanvas == null) return;
        inventoryCanvas.SetActive(false);
    }


}
