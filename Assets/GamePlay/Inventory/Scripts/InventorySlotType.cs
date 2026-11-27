using UnityEngine;

public enum InventorySlotType
{
    Tool,
    Resource
}

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private InventorySlotType slotType;
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private EquippedTool toolType = EquippedTool.None;
    
    [Header("Resource")]
    [SerializeField] private GameObject resourcePrefab;

    public GameObject ResourcePrefab => resourcePrefab;

    public InventorySlotType SlotType => slotType;
    public bool IsEmpty => !itemIcon.activeSelf;
    public EquippedTool Tool => toolType;

}
