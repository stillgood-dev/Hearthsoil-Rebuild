using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("Inventory Canvas")]
    [SerializeField] private GameObject inventoryCanvas;


    [Header("Inventory Icons")]
    [SerializeField] private GameObject[] toolIcons;
    [Tooltip("Must be in same order as toolIcons")]
    [SerializeField] private EquippedTool[] toolTypes;
    [SerializeField] private GameObject[] resourceIcon;



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

    private void RefreshInventory()
    {
        if(inventoryManager == null) return;
        if(toolIcons == null) return;

        RefreshTools();

        RefreshResources();

       
    }

    private void RefreshTools()
    {
        if(inventoryManager == null) return;

        for(int i = 0; i < toolIcons.Length; i++)
        {
            toolIcons[i].SetActive(
                inventoryManager.HasTool(toolTypes[i])  
            );
        }
        
    }

    private void RefreshResources()
    {
        if( inventoryManager == null) return;
        
        for(int i = 0; i < inventoryManager.Resources.Count; i++)
        {
            InventoryResource resource = inventoryManager.Resources[i];
            if(resource == null) continue;
            if(resource.discovered && resource.quantity > 0)
            {
                resourceIcon[i].SetActive(true);
            }
        }
        
    }

    public void CloseInventory()
    {
        if (inventoryCanvas == null) return;
        inventoryCanvas.SetActive(false);
    }


}
