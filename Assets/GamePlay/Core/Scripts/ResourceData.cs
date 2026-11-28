using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Hearthsoil/Resource")]
public class ResourceData : ScriptableObject
{
    // eventually move the parameters in ReceivableObjectController entirely to here
    [Header("Resource")]
    public ResourceType resourceType;
    public string resourceName;
    public Sprite inventorySprite;

    [Header("Consumption")]
    public bool isEdible;
    public float satietyRestoration;

    [Header("Storage")]
    public bool isStorable = true;
}
