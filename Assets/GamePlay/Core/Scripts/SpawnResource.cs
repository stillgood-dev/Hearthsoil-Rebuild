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

    public void SpawnResourceOnHit(FacingDirection facing, int hits)
    {
        if (resource == null) return;
        if (resourceDropPoint == null) return;

        GameObject spawnedResource = Instantiate(
            resource, 
            resourceDropPoint.position, 
            Quaternion.identity
         );
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
            resourceDropCount++;
        }
    }
}
