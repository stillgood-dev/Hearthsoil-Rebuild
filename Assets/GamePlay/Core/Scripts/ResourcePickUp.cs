using UnityEngine;

public class ResourcePickUp : MonoBehaviour
{

    [SerializeField] private float pickupDelay = 0.1f;
    private bool canPickUp = false;

    private void Start()
    {
        Invoke(nameof(EnablePickup), pickupDelay);
    }

    private void EnablePickup()
    {
        canPickUp = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canPickUp) return;
        if (!other.CompareTag("Player")) return;

        // TO DO: Add to inventory

        Destroy(transform.root.gameObject);
    }
}
