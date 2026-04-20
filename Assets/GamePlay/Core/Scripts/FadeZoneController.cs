using UnityEngine;

public class FadeZoneController : MonoBehaviour
{
    [SerializeField] private FadeOccludersController fade;

    private void Awake()
    {
        if (!fade) fade = GetComponent<FadeOccludersController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        fade?.SortOccluders(fade.occluderSpriteRenderers);
        fade?.FadeOccluders(fade.occluderSpriteRenderers, fade.alpha);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        fade?.RefreshOccluders(fade.occluderSpriteRenderers);
    }

}
