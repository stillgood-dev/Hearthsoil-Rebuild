using UnityEngine;

public class SpawnResource : MonoBehaviour
{
    [Header("Resources")]
    [Tooltip("Any resources that pop out from damaged/hit object")]
    [SerializeField] private GameObject resource;
    [SerializeField] private Transform resourceDropPoint;
    [SerializeField] private int resourceDropCount = 0;

    [Header("Spawn Style")]
    [SerializeField] private bool cluster;
    [SerializeField] private bool stack;

    [Header("Pop Out Parameters")]
    [SerializeField] private float popDistance = 0.35f;
    [SerializeField] private float popHeight = 0.2f;
    [SerializeField] private float popDuration = 0.15f;

    [Header("Cluster Parameters")]
    [SerializeField] private float clusterSpreadX = 0.15f;
    [SerializeField] private float clusterSpreadY = 0.15f;

    [Header("Persistence")]
    [SerializeField] private string spawnerID;

    public void SpawnResourceOnHit(FacingDirection facing, int hits)
    {
        if (resource == null) return;
        if (resourceDropPoint == null) return;

        GameObject spawnedResource = Instantiate(
            resource, 
            resourceDropPoint.position, 
            Quaternion.identity
         );

        PersistentDestroyableObject persistentObject =
            spawnedResource.GetComponent<PersistentDestroyableObject>();

        if(persistentObject != null)
        {
            persistentObject.SetObjectID($"{spawnerID}_{resourceDropCount}");
        }

        ResourcePopOut popOut = spawnedResource.GetComponent<ResourcePopOut>();

        if (popOut != null)
        {
            popOut.Pop(
                facing, 
                hits, 
                stack, 
                cluster, 
                clusterSpreadX, 
                clusterSpreadY, 
                popDistance, 
                popDuration,
                popHeight
                );
        }
        resourceDropCount++;
    }

    public void RestoreResources(int resourceCount)
    {
        if (resource == null) return;

        for(int i = 0; i < resourceCount; i++)
        {
            string resourceID = $"{spawnerID}_{i}";

            // Don't restore resources the player already collected
            if (WorldState.RemovedObjects.Contains(resourceID))
                continue;

            // Only restore resources that actually spawned before
            if (!WorldState.ObjectPositions.ContainsKey(resourceID))
                continue;

            // restore the resource
            GameObject spawnedResource = Instantiate(
            resource,
            WorldState.ObjectPositions[resourceID],
            Quaternion.identity
            );

            PersistentDestroyableObject persistentObject =
                spawnedResource.GetComponent<PersistentDestroyableObject>();

            // set a distinct objectID for each resource
            if (persistentObject != null)
            {
                persistentObject.SetObjectID(resourceID);
            }

        }
    }
}
