using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FadeOccludersController : MonoBehaviour
{
    [SerializeField] public SpriteRenderer[] occluderSpriteRenderers;
    [Tooltip("Secondary occluders to disable")]
    [SerializeField] public SpriteRenderer[] secondarySpriteRenderers;
    [SerializeField] public PolygonCollider2D fadeZone;
    [SerializeField] private string occluderSortingLayer = "Occluders";
    [SerializeField] private string objectSortingLayer = "Objects";
    [SerializeField] public float alpha = 0.45f;
    private float originalAlpha;
    private Dictionary<SpriteRenderer, float> occluderAlphas;

    private void Awake()
    {
        if(!fadeZone) fadeZone = GetComponent<PolygonCollider2D>();
        if (occluderSpriteRenderers == null) Debug.LogError("Missing occluder sprite renderers", this);

        // cache initial alpha values to restore them later
        occluderAlphas = new Dictionary<SpriteRenderer, float>();
        foreach(var sr in occluderSpriteRenderers)
        {
            if(!sr) continue;
            occluderAlphas[sr] = sr.color.a;
        }
    }

    public void SortOccluders(SpriteRenderer[] occluderSprites)
    {
        if(occluderSprites == null) return;
        foreach(var sr in occluderSprites)
        {
            if (!sr) continue;
            // update sorting layer to be in front of player
            sr.sortingLayerName = occluderSortingLayer;
        }
    }

    public void FadeOccluders(SpriteRenderer[] occluderSprites, float alphaValue)
    {
        if (occluderSprites == null) return;
        foreach(var sr in occluderSprites)
        {
            if (!sr) continue;
            // update alpha to fade object
            Color c = sr.color;
            c.a = alphaValue;
            sr.color = c;
        }
    }

    public void DisableSecondarySpriteRenderers(SpriteRenderer[] secondarySpriteRenderers)
    {
        if (secondarySpriteRenderers == null) return;
        foreach(var sr in secondarySpriteRenderers)
        {
            if (!sr) continue;
            // disable object
            sr.enabled = false;
        }
    }

    public void RefreshOccluders(SpriteRenderer[] occluderSprites, SpriteRenderer[] secondaryOccluderSprites)
    {
        // Fade occluders
        if( occluderSprites == null) return;
        foreach(var sr in occluderSprites)
        {
            if (!sr) continue;
            // set original sorting layer
            sr.sortingLayerName = objectSortingLayer;
            // return original alphas
            Color c = sr.color;
            c.a = occluderAlphas[sr];
            sr.color = c;
        }

        // Disable secondary occluders
        if (secondaryOccluderSprites == null) return;
        foreach(var sr in secondaryOccluderSprites)
        {
            if (!sr) continue;
            sr.enabled = true;
        }
    }

}
