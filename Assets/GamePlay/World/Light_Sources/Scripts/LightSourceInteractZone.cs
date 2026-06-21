using UnityEngine;

public class LightSourceInteractZone : MonoBehaviour
{

    [Header("Player Refs")]
    [SerializeField] private PlayerLightSourceController playerLightController;
    [SerializeField] private LightSourceController lightSource;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerLightController = other.GetComponent<PlayerLightSourceController>(); 
        lightSource?.SetPlayerInInteractZone(true, playerLightController);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerLightController = other.GetComponent<PlayerLightSourceController>();
        lightSource?.SetPlayerInInteractZone(false, playerLightController);
    }
}
