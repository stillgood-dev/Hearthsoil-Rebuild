using UnityEngine;

public class ToolSortController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer toolSpriteRenderer;
    [SerializeField] private int originalSortingOrder;

    private void Awake()
    {
        if (!toolSpriteRenderer) toolSpriteRenderer = GetComponent<SpriteRenderer>();
        originalSortingOrder = toolSpriteRenderer.sortingOrder;
    }

    // Update sorting order as animation event
   public void SetSortingOrder(int order)
    {
        if (toolSpriteRenderer == null) return;
        toolSpriteRenderer.sortingOrder = order;
    }

    // refresh sorting order as animation event
    public void RefreshSortingOrder()
    {
        if(toolSpriteRenderer == null) return;
        toolSpriteRenderer.sortingOrder = originalSortingOrder;
    }
}
