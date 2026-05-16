using UnityEngine;

public class PlayerToolState : MonoBehaviour
{
    [SerializeField] private EquippedTool equippedTool = EquippedTool.None;

    public EquippedTool EquippedTool => equippedTool;

    public void EquipTool(EquippedTool tool)
    {
        equippedTool = tool;
    }

    public void UnequipTool()
    {
        equippedTool = EquippedTool.None;
    }
}