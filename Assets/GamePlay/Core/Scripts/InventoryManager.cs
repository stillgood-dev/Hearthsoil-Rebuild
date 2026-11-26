using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private List<InventoryResource> resources = new();

    [Header("Tools")]
    [SerializeField] private List<EquippedTool> tools = new();

    [Header("Debug")]
    [SerializeField] private bool testTool = false;
    [SerializeField] private EquippedTool testToolType = EquippedTool.None;

    public List<InventoryResource> Resources => resources;

    private void Start()
    {
        if (testTool)
        {
            AddTool(testToolType);
        }
    }

    // add tool to player inventory
    public void AddTool(EquippedTool tool)
    {
        if (tool == EquippedTool.None) return;
        // don't add if we've already got it
        if(tools.Contains(tool)) return;

        tools.Add(tool);   
    }

    // check whether a player owns a tool
    public bool HasTool(EquippedTool tool)
    {
        return tools.Contains(tool);
    }

    public bool HasDiscovered(ResourceType resourceType)
    {
        foreach(InventoryResource resource in resources)
        {
            if(resource.resourceType == resourceType)
            {
                return resource.discovered;
            }
        }

        return false;
    }

    public void DiscoverResource(ResourceType resourceType)
    {
        foreach(InventoryResource resource in resources)
        {
            if(resource.resourceType == resourceType)
            {
                resource.discovered = true;
                return;
            }
        }
    }

    public void AddResource(ResourceType resourceType, int amount = 1)
    {
        foreach(InventoryResource resource in resources)
        {
            if(resource.resourceType == resourceType)
            {
                resource.quantity += amount;
                return;
            }
        }
    }
}
