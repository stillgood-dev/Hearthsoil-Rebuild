using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private InventorySelectorController inventorySelectorController;
    [SerializeField] private PlayerActionState playerActionState;

    [Header("Inventory Canvas")]
    [SerializeField] private GameObject inventoryCanvas;

    [Header("Inventory Icons")]
    [SerializeField] private GameObject[] toolIcons;
    [Tooltip("Must be in same order as toolIcons")]
    [SerializeField] private EquippedTool[] toolTypes;
    [SerializeField] private GameObject[] resourceIcons;

    public bool IsOpen => inventoryCanvas.activeSelf;
    public GameObject[] ToolIcons => toolIcons;
    public GameObject[] ResourceIcon => resourceIcons;   

    private void Awake()
    {
        if(!inventoryManager) 
            inventoryManager = GetComponent<InventoryManager>();

        if(!inventorySelectorController) 
            inventorySelectorController = GetComponent<InventorySelectorController>();

        if(inventoryCanvas != null) 
            inventoryCanvas.SetActive(false);

        if(!playerActionState)
            playerActionState = GetComponent<PlayerActionState>();

        foreach (GameObject icon in resourceIcons)
        {
            if (icon == null)
            {
                Debug.LogError("A resource icon is missing!", this);
                return;
            }
        }

        foreach (GameObject icon in toolIcons)
        {
            if (icon == null)
            {
                Debug.LogError("A tool icon is missing!", this);
                return;
            }
        }
    }

    public void OpenInventory()
    {
        if(inventoryCanvas == null) return;
        inventoryCanvas.SetActive(true);
        playerActionState.SetActionState(PlayerState.Inventory);

        RefreshInventory();

        inventorySelectorController.UpdateSelectorPosition();

    }

    

    private void RefreshInventory()
    {
        if(inventoryManager == null) return;
        if(toolIcons == null)
        {
            Debug.LogError("Need to wire up tool icons!", this);
            return;
        }
        if (resourceIcons == null)
        {
            Debug.LogError("Need to wire up resource icons!", this);
            return;
        }

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
                resourceIcons[i].SetActive(true);
            }
            else
            {
                resourceIcons[i].SetActive(false);
            }
        }
        
    }

    public void CloseInventory()
    {
        if (inventoryCanvas == null) return;
        inventoryCanvas.SetActive(false);
        playerActionState.ClearActionState();
        
    }


}
