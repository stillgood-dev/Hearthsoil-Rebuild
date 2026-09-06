using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private List<InventoryResource> resources = new();

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
